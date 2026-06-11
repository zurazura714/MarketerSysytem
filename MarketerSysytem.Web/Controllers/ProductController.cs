using MapsterMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IMapper mapper, IProductService productService) : ControllerBase
{
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsAsync()
    {
        var products = await productService.ListAsync();
        return Ok(mapper.Map<IEnumerable<ProductDTO>>(products));
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<IActionResult> GetProductAsync(int id)
    {
        var product = await productService.FetchAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<ProductDTO>(product));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(ProductCreateDTO productDTO)
    {
        var productEntity = mapper.Map<Product>(productDTO);

        await productService.SaveAsync(productEntity);

        var productReturn = mapper.Map<ProductDTO>(productEntity);
        return CreatedAtRoute("GetProduct",
            new { id = productReturn.ID },
            productReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductAsync(int id, ProductCreateDTO productDTO)
    {
        var product = await productService.FetchAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        product.Price = productDTO.Price;
        product.Name = productDTO.Name;

        await productService.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductAsync(int id)
    {
        var product = await productService.FetchAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        await productService.DeleteAsync(product);
        return NoContent();
    }
}
