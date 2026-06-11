using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.Policies;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.EntityFrameworkCore;

namespace MarketerSystem.Service.Service;

public class BonusPaymentService : ServiceBase<BonusPayment, IBonusPaymentRepository>, IBonusPaymentService
{
    private readonly ISellRepository _sellRepository;
    private readonly IDistributorRepository _distributorRepository;

    public BonusPaymentService(
        IUnitOfWork context,
        IBonusPaymentRepository bonusPaymentRepository,
        ISellRepository sellRepository,
        IDistributorRepository distributorRepository)
        : base(context, bonusPaymentRepository)
    {
        _sellRepository = sellRepository ?? throw new ArgumentNullException(nameof(sellRepository));
        _distributorRepository = distributorRepository ?? throw new ArgumentNullException(nameof(distributorRepository));
    }

    public Task<List<BonusPayment>> FilterPaymentsProducts(PaymentFilterParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        return _repository.Set()
            .Include(a => a.Distributor)
            .Where(a =>
                (parameters.MinPrice == null || parameters.MinPrice <= a.BonusPay) &&
                (parameters.MaxPrice == null || parameters.MaxPrice >= a.BonusPay) &&
                (parameters.Name == null || a.Distributor!.FirstName.Contains(parameters.Name)) &&
                (parameters.LastName == null || a.Distributor!.LastName.Contains(parameters.LastName)))
            .ToListAsync();
    }

    public async Task<List<BonusPayment>> GenerateBonusPaymentsForPeriodAsync(PaymentParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        var unpaidSales = await _sellRepository.Set()
            .Where(s => s.SoldDate >= parameters.FromDate
                     && s.SoldDate <= parameters.Todate
                     && !s.UsedForPayment)
            .ToListAsync();

        var sellerIds = unpaidSales.Select(s => s.DistributorID).Distinct().ToList();
        var sellers = await _distributorRepository.Set()
            .Where(d => sellerIds.Contains(d.DistributorID))
            .ToDictionaryAsync(d => d.DistributorID);

        var createdPayments = new List<BonusPayment>();
        foreach (var sale in unpaidSales)
        {
            // The sale is tracked by the query above — mutating it is enough,
            // the change persists on the single commit below.
            sale.UsedForPayment = true;

            await CreatePaymentAsync(createdPayments, sale.DistributorID,
                BonusPercentages.DirectSeller * sale.ProductTotalPrice, parameters);

            if (sellers.TryGetValue(sale.DistributorID, out var seller))
            {
                await PayUplineAsync(createdPayments, seller.GenerationLinker, sale.ProductTotalPrice, parameters);
            }
        }

        // One commit: marked sales and all payments land atomically.
        await _context.CommitAsync();
        return createdPayments;
    }

    private async Task PayUplineAsync(
        List<BonusPayment> createdPayments, string? generationLinker, decimal saleTotal, PaymentParameters parameters)
    {
        var upline = GenerationChain.Parse(generationLinker);

        // The chain is root-first; level 0 is the immediate recommender at the end.
        for (int level = 0; level < upline.Count; level++)
        {
            decimal percentage = BonusPercentages.ForUplineLevel(level);
            if (percentage <= 0m)
            {
                break;
            }

            int ancestorDistributorId = upline[upline.Count - 1 - level];
            await CreatePaymentAsync(createdPayments, ancestorDistributorId, percentage * saleTotal, parameters);
        }
    }

    private async Task CreatePaymentAsync(
        List<BonusPayment> createdPayments, int distributorId, decimal bonusPay, PaymentParameters parameters)
    {
        var payment = new BonusPayment
        {
            FromDate = parameters.FromDate,
            ToDate = parameters.Todate,
            DistributorID = distributorId,
            BonusPay = bonusPay
        };

        await _repository.SaveAsync(payment);
        createdPayments.Add(payment);
    }
}
