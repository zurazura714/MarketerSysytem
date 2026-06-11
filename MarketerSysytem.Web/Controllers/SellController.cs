using MapsterMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SellController(
    IMapper mapper,
    ISellService sellService,
    IDistributorService distributorService,
    IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SellDTO>>> GetSoldProductsAsync([FromQuery] SellResourceParameters parameters)
    {
        var soldProducts = await sellService.FilterSoldProducts(parameters);
        return Ok(mapper.Map<IEnumerable<SellDTO>>(soldProducts));
    }

    [HttpGet("{id}", Name = "GetSoldProduct")]
    public async Task<ActionResult<ProductDTO>> GetSoldProductAsync(int id)
    {
        var soldProduct = await sellService.FetchAsync(id);
        if (soldProduct == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<SellDTO>(soldProduct));
    }

    [HttpPost]
    public async Task<IActionResult> SellProductAsync(SellCreateDTO sellDTO)
    {
        var sellEntity = mapper.Map<Sell>(sellDTO);

        var product = await productService.FetchAsync(sellEntity.ProductID);
        if (product == null)
        {
            return BadRequest(new { error = $"Product {sellEntity.ProductID} not found" });
        }
        var distributor = await distributorService.FetchAsync(sellEntity.DistributorID);
        if (distributor == null)
        {
            return BadRequest(new { error = $"Distributor {sellEntity.DistributorID} not found" });
        }

        sellEntity.Product = product;
        sellEntity.Distributor = distributor;
        sellEntity.ProductPrice = product.Price;
        sellEntity.ProductTotalPrice = product.Price;
        sellEntity.ProductUnitPrice = product.Price;
        await sellService.SaveAsync(sellEntity);

        var soldProduct = mapper.Map<SellDTO>(sellEntity);
        return CreatedAtRoute("GetSoldProduct",
            new { id = soldProduct.ID },
            soldProduct);
    }
}
