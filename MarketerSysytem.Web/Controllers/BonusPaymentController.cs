using AutoMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using MarketerSystem.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers
{
    public class BonusPaymentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBonusPaymentService _bonusPaymentService;
        private readonly ISellService _sellService;
        public BonusPaymentController(IMapper mapper, IBonusPaymentService bonusPaymentService, ISellService sellService)
        {
            _mapper = mapper;
            _bonusPaymentService = bonusPaymentService;
            _sellService = sellService;
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
                var bonus = 01m * soldProduct.ProductTotalPrice;
                var bonusPayment = new BonusPayment
                {
                    FromDate = paramseters.FromDate,
                    ToDate = paramseters.Todate,
                    DistributorID = soldProduct.DistributorID,
                    BonusPay = bonus
                };

                //distributorebi other gen
            }

            //var soldProducts = await _bonusPaymentService.PaymentsDistributors(paramseters);
            return Ok(_mapper.Map<IEnumerable<BonusPaymentDTO>>(soldProducts));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BonusPaymentDTO>>> GetSoldProductsAsync([FromQuery] PaymentFilterParameters paramseters)
        {
            var soldProducts = await _bonusPaymentService.FilterPaymentsProducts(paramseters);
            return Ok(_mapper.Map<IEnumerable<BonusPaymentDTO>>(soldProducts));
        }
    }
}
