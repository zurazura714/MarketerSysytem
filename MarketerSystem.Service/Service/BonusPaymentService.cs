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

    public async Task<List<BonusPayment>> FilterPaymentsProducts(PaymentFilterParameters parameters)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var payments = (await SetAsync()).AsQueryable().Include(a => a.Distributor)
            .Where(a =>
                (parameters.MinPrice == null || parameters.MinPrice <= a.BonusPay) &&
                (parameters.MaxPrice == null || parameters.MaxPrice >= a.BonusPay) &&
                (parameters.Name == null || a.Distributor.FirstName.Contains(parameters.Name)) &&
                (parameters.LastName == null || a.Distributor.LastName.Contains(parameters.LastName)))
            .ToList();

        return payments;
    }

    public async Task GenerateBonusPaymentsForPeriodAsync(PaymentParameters parameters)
    {
        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        var unpaidSales = (await _sellRepository.SetAsync())
            .Where(s => s.SoldDate >= parameters.FromDate
                     && s.SoldDate <= parameters.Todate
                     && !s.UsedForPayment)
            .ToList();

        foreach (var sale in unpaidSales)
        {
            sale.UsedForPayment = true;
            await _sellRepository.SaveAsync(sale);

            await _repository.SaveAsync(new BonusPayment
            {
                FromDate = parameters.FromDate,
                ToDate = parameters.Todate,
                DistributorID = sale.DistributorID,
                BonusPay = BonusPercentages.DirectSeller * sale.ProductTotalPrice
            });

            var seller = await _distributorRepository.FetchAsync(sale.DistributorID);
            if (seller != null && !string.IsNullOrWhiteSpace(seller.GenerationLinker))
            {
                await PayUplineAsync(seller.GenerationLinker, sale.ProductTotalPrice, parameters);
            }
        }

        await _context.CommitAsync();
    }

    private async Task PayUplineAsync(string generationLinker, decimal saleTotal, PaymentParameters parameters)
    {
        var upline = generationLinker.Split(',', StringSplitOptions.RemoveEmptyEntries);

        for (int i = upline.Length - 1; i >= 0; i--)
        {
            int level = upline.Length - 1 - i;
            decimal percentage = BonusPercentages.ForUplineLevel(level);
            if (percentage <= 0m)
            {
                break;
            }

            if (!int.TryParse(upline[i], out int ancestorDistributorId))
            {
                continue;
            }

            await _repository.SaveAsync(new BonusPayment
            {
                FromDate = parameters.FromDate,
                ToDate = parameters.Todate,
                DistributorID = ancestorDistributorId,
                BonusPay = percentage * saleTotal
            });
        }
    }
}
