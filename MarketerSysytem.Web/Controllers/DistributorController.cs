using MapsterMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DistributorController(IMapper mapper, IDistributorService distributorService) : ControllerBase
{
    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<DistributorDTO>>> GetDistributorsAsync()
    {
        var distributors = await distributorService.ListWithDetailsAsync();
        return Ok(mapper.Map<IEnumerable<DistributorDTO>>(distributors));
    }

    [HttpGet("{id}", Name = "GetDistributor")]
    [HttpHead("{id}")]
    public async Task<IActionResult> GetDistributorAsync(int id)
    {
        var distributor = await distributorService.FetchWithDetailsAsync(id);
        if (distributor == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<DistributorDTO>(distributor));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDistributorAsync(DistributorCreateDTO distributorDTO)
    {
        var distributorEntity = mapper.Map<Distributor>(distributorDTO);
        var created = await distributorService.CreateAsync(distributorEntity);

        var distributorReturn = mapper.Map<DistributorDTO>(created);
        return CreatedAtRoute("GetDistributor",
            new { id = distributorReturn.DistributorID },
            distributorReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDistributorAsync(int id, DistributorCreateDTO distributorDTO)
    {
        var updated = mapper.Map<Distributor>(distributorDTO);
        var result = await distributorService.UpdateAsync(id, updated);
        if (result == null)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDistributorAsync(int id)
    {
        var distributor = await distributorService.FetchAsync(id);
        if (distributor == null)
        {
            return NotFound();
        }

        await distributorService.DeleteAsync(distributor);
        return NoContent();
    }
}
