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
    public class SellController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISellService _sellService;
        private readonly IDistributorService _distributorService;
        private readonly IProductService _productService;
        public SellController(IMapper mapper, ISellService sellService, IDistributorService distributorService, IProductService productService)
        {
            _mapper = mapper;
            _sellService = sellService;
            _distributorService = distributorService;
            _productService = productService;
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
        public async Task<IActionResult> SellProductAsync(SellCreateDTO sellDTO)
        {
            var sellEntity = _mapper.Map<Sell>(sellDTO);
            sellEntity.Product = await _productService.FetchAsync(sellEntity.ProductID);
            sellEntity.Distributor = await _distributorService.FetchAsync(sellEntity.DistributorID);
            sellEntity.ProductPrice = sellEntity.Product.Price;
            sellEntity.ProductTotalPrice = sellEntity.Product.Price;
            sellEntity.ProductUnitPrice = sellEntity.Product.Price;
            await _sellService.SaveAsync(sellEntity);

            var soldProduct = _mapper.Map<SellDTO>(sellEntity);
            return CreatedAtRoute("GetSoldProduct",
                new { id = soldProduct.ID },
                soldProduct);
        }
    }
}
