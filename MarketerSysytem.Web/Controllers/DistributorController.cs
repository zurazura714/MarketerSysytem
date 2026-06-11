using System.Text;
using MapsterMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DistributorController(
    IMapper mapper,
    IDistributorService distributorService,
    IPictureService pictureService,
    IContactInfoService contactInfoService,
    IAddressService addressService,
    IPassportService passportService) : ControllerBase
{
    private const int MaxGenerationDepth = 5;

    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<IEnumerable<DistributorDTO>>> GetDistributorsAsync()
    {
        var distributors = (await distributorService.SetAsync()).ToList();
        var allPictures = await pictureService.SetAsync();
        var allContactInfos = await contactInfoService.SetAsync();
        var allAddresses = await addressService.SetAsync();

        foreach (var distributor in distributors)
        {
            distributor.Pictures = allPictures.Where(x => x.DistributorID == distributor.DistributorID).ToList();
            distributor.ContactInfos = allContactInfos.Where(x => x.DistributorID == distributor.DistributorID).ToList();
            distributor.Addresses = allAddresses.Where(x => x.DistributorID == distributor.DistributorID).ToList();
            distributor.Passport = await passportService.FetchAsync(distributor.PassportID);
        }

        return Ok(mapper.Map<IEnumerable<DistributorDTO>>(distributors));
    }

    [HttpGet("{id}", Name = "GetDistributor")]
    [HttpHead]
    public async Task<IActionResult> GetDistributorAsync(int id)
    {
        var distributor = await distributorService.FetchAsync(id);
        if (distributor == null)
        {
            return NotFound();
        }

        distributor.Pictures = (await pictureService.SetAsync())
            .Where(x => x.DistributorID == distributor.DistributorID).ToList();
        distributor.ContactInfos = (await contactInfoService.SetAsync())
            .Where(x => x.DistributorID == distributor.DistributorID).ToList();
        distributor.Addresses = (await addressService.SetAsync())
            .Where(x => x.DistributorID == distributor.DistributorID).ToList();
        distributor.Passport = await passportService.FetchAsync(distributor.PassportID);

        return Ok(mapper.Map<DistributorDTO>(distributor));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDistributorAsync(DistributorCreateDTO distributorDTO)
    {
        var distributorEntity = mapper.Map<Distributor>(distributorDTO);
        distributorEntity.DistributorGuid = Guid.NewGuid();

        if (distributorEntity.RecomendatorID is int recomendatorId && recomendatorId != 0)
        {
            var recomendator = await distributorService.FetchAsync(recomendatorId);
            if (recomendator?.GenerationLinker != null)
            {
                var elements = recomendator.GenerationLinker.Split(',');
                if (elements.Length == MaxGenerationDepth)
                {
                    return BadRequest(new { error = "Try Other Recomendator, Its limit has been reached" });
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(recomendator.GenerationLinker);
                stringBuilder.Append(',');
                stringBuilder.Append(recomendator.DistributorID);
                distributorEntity.GenerationLinker = stringBuilder.ToString();
            }
            else if (recomendator != null)
            {
                distributorEntity.GenerationLinker = recomendator.DistributorID.ToString();
            }
        }

        await distributorService.SaveAsync(distributorEntity);

        var distributorReturn = mapper.Map<DistributorDTO>(distributorEntity);
        return CreatedAtRoute("GetDistributor",
            new { id = distributorReturn.DistributorID },
            distributorReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDistributorAsync(int id, DistributorCreateDTO distributorDTO)
    {
        var distributor = await distributorService.FetchAsync(id);
        if (distributor == null)
        {
            return NotFound();
        }

        distributor.FirstName = distributorDTO.FirstName;
        distributor.LastName = distributorDTO.LastName;
        distributor.BirthDate = distributorDTO.BirthDate;
        distributor.Gender = distributorDTO.Gender;

        if (distributorDTO.Addresses != null)
        {
            foreach (var addressDto in distributorDTO.Addresses)
            {
                var address = mapper.Map<Address>(addressDto);
                address.DistributorID = distributor.DistributorID;
                await addressService.SaveAsync(address);
            }
        }
        if (distributorDTO.ContactInfos != null)
        {
            foreach (var contactInfoDto in distributorDTO.ContactInfos)
            {
                var contactInfo = mapper.Map<ContactInfo>(contactInfoDto);
                contactInfo.DistributorID = distributor.DistributorID;
                await contactInfoService.SaveAsync(contactInfo);
            }
        }
        if (distributorDTO.Pictures != null)
        {
            foreach (var pictureDto in distributorDTO.Pictures)
            {
                var picture = mapper.Map<Picture>(pictureDto);
                picture.DistributorID = distributor.DistributorID;
                await pictureService.SaveAsync(picture);
            }
        }

        await distributorService.SaveChangesAsync();
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
        await distributorService.DeleteAsync(id);
        return NoContent();
    }
}
