using AutoMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using MarketerSystem.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers
{
    public class SellController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISellService _sellService;
        public SellController(IMapper mapper, ISellService sellService)
        {
            _mapper = mapper;
            _sellService = sellService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SellDTO>>> GetSoldProductsAsync([FromQuery] SellResourceParameters sellResourceParameters )
        {
            var soldProducts = await _sellService.FilterSoldProducts(sellResourceParameters);
            return Ok(_mapper.Map<IEnumerable<SellDTO>>(soldProducts));
        }

        [HttpGet("{id}", Name = "GetSoldProduct")]
        public async Task<ActionResult<ProductDTO>> GetSoldProductAsync(int id)
        {
            var soldProduct = await _sellService.FetchAsync(id);
            return Ok(_mapper.Map<SellDTO>(soldProduct));
        }

        [HttpPost]
        public async Task<IActionResult> SellProductAsync(SellDTO sellDTO)
        {
            var sellEntity = _mapper.Map<Sell>(sellDTO);

            await _sellService.SaveAsync(sellEntity);

            var soldProduct = _mapper.Map<SellDTO>(sellEntity);
            return CreatedAtRoute("GetSoldProduct",
                new { id = soldProduct.ID },
                soldProduct);
        }
    }
}
