using MapsterMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SellController(IMapper mapper, ISellService sellService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SellDTO>>> GetSoldProductsAsync([FromQuery] SellResourceParameters parameters)
    {
        var soldProducts = await sellService.FilterSoldProducts(parameters);
        return Ok(mapper.Map<IEnumerable<SellDTO>>(soldProducts));
    }

    [HttpGet("{id}", Name = "GetSoldProduct")]
    public async Task<ActionResult<SellDTO>> GetSoldProductAsync(int id)
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
        var created = await sellService.CreateSellAsync(sellEntity);

        var soldProduct = mapper.Map<SellDTO>(created);
        return CreatedAtRoute("GetSoldProduct",
            new { id = soldProduct.ID },
            soldProduct);
    }
}
