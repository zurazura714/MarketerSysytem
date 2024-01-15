using AutoMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MarketerSysytem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public ProductController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsAsync()
        {
            var products = await _productService.SetAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDTO>>(products));
        }


        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var product = await _productService.FetchAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductDTO>(product));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync(ProductCreateDTO productDTO)
        {
            var productEntity = _mapper.Map<Product>(productDTO);
            
            await _productService.SaveAsync(productEntity);

            var productReturn = _mapper.Map<ProductDTO>(productEntity);
            return CreatedAtRoute("GetProduct",
                new { id = productReturn.ID },
                productReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductCreateDTO productDTO)
        {
            var product = await _productService.FetchAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Price = productDTO.Price;
            product.Name = productDTO.Name;

            await _productService.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var product = _productService.FetchAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
