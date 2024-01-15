using AutoMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using MarketerSystem.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BonusPaymentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBonusPaymentService _bonusPaymentService;
        private readonly ISellService _sellService;
        private readonly IDistributorService _distributorService;
        public BonusPaymentController(IMapper mapper, IBonusPaymentService bonusPaymentService, ISellService sellService, IDistributorService distributorService)
        {
            _mapper = mapper;
            _bonusPaymentService = bonusPaymentService;
            _sellService = sellService;
            _distributorService = distributorService;
        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<BonusPaymentDTO>>> GetSoldProductsAsync([FromQuery] PaymentParameters paramseters)
        {
            var soldProducts = (await _sellService.SetAsync())
            .Where(a => a.SoldDate >= paramseters.FromDate && a.SoldDate <= paramseters.Todate).ToList();

            foreach (var soldProduct in soldProducts)
            {
                soldProduct.UsedForPayment = true;
                await _sellService.SaveAsync(soldProduct);
                var bonusPayment = new BonusPayment
                {
                    FromDate = paramseters.FromDate,
                    ToDate = paramseters.Todate,
                    DistributorID = soldProduct.DistributorID,
                    BonusPay = 01m * soldProduct.ProductTotalPrice
                };
                await _bonusPaymentService.SaveAsync(bonusPayment);

                //distributorebi other gen
                var distributor = await _distributorService.FetchAsync(soldProduct.DistributorID);
                if (distributor.GenerationLinker != null)
                {
                    await CalculateBonusPaymentsAsync(distributor.DistributorID, paramseters.FromDate.DateTime, paramseters.Todate.DateTime, soldProduct.ProductTotalPrice);
                }
            }

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BonusPaymentDTO>>> GetSoldProductsAsync([FromQuery] PaymentFilterParameters paramseters)
        {
            var soldProducts = await _bonusPaymentService.FilterPaymentsProducts(paramseters);
            return Ok(_mapper.Map<IEnumerable<BonusPaymentDTO>>(soldProducts));
        }


        private async Task CalculateBonusPaymentsAsync(int distributorId, DateTime fromDate, DateTime toDate, decimal productTotalPrice, int currentLevel = 0)
        {
            var distributor = await _distributorService.FetchAsync(distributorId);

            if (distributor.GenerationLinker != null)
            {
                string[] elements = distributor.GenerationLinker.Split(',');
                currentLevel = Array.IndexOf(elements, distributorId);
                if (currentLevel < elements.Length)
                {
                    decimal bonusPercentage = GetBonusPercentage(currentLevel);

                    var bonusPaymentForRecommender = new BonusPayment
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        DistributorID = distributor.DistributorID,
                        BonusPay = bonusPercentage * productTotalPrice
                    };

                    await _bonusPaymentService.SaveAsync(bonusPaymentForRecommender);
                }

                if (currentLevel + 1 < elements.Length)
                {
                    foreach (var recommendedDistributorId in elements.Select(int.Parse))
                    {
                        await CalculateBonusPaymentsAsync(recommendedDistributorId, fromDate, toDate, productTotalPrice, currentLevel + 1);
                    }
                }
            }
        }

        private decimal GetBonusPercentage(int level)
        {
            switch (level)
            {
                case 0:
                    return 0.05m;
                case 1:
                    return 0.01m;
                default:
                    return 0m;
            }
        }
    }
}
