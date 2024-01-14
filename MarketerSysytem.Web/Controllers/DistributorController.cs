using AutoMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.Model;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Text.Json;

namespace MarketerSysytem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistributorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDistributorService _distributorService;
        private readonly IPictureService _pictureService;
        private readonly IContactInfoService _contactInfoService;
        private readonly IAddressService _addressService;
        private readonly IPassportService _passportService;
        public DistributorController(IMapper mapper, IDistributorService distributorService,
            IPictureService pictureService, IContactInfoService contactInfoService, IAddressService addressService,
            IPassportService passportService)
        {
            _mapper = mapper;
            _distributorService = distributorService;
            _pictureService = pictureService;
            _contactInfoService = contactInfoService;
            _addressService = addressService;
            _passportService = passportService;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<DistributorDTO>>> GetDistributorsAsync()
        {
            var distributors = await _distributorService.SetAsync();
            foreach (var distributor in distributors)
            {
                distributor.Pictures = (await _pictureService.SetAsync())
                    .Where(x => x.DistributorID == distributor.DistributorID).ToList();
                distributor.ContactInfos = (await _contactInfoService.SetAsync())
                    .Where(x => x.DistributorID == distributor.DistributorID).ToList();
                distributor.Addresses = (await _addressService.SetAsync())
                    .Where(x => x.DistributorID == distributor.DistributorID).ToList();
                distributor.Passport = await _passportService.FetchAsync(distributor.PassportID);
            }
            return Ok(_mapper.Map<IEnumerable<DistributorDTO>>(distributors));
        }


        [HttpGet("{id}", Name = "GetDistributor")]
        [HttpHead]
        public async Task<IActionResult> GetDistributor(int id)
        {
            var distributor = await _distributorService.FetchAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }
            distributor.Pictures = (await _pictureService.SetAsync())
                .Where(x => x.DistributorID == distributor.DistributorID).ToList();
            distributor.ContactInfos = (await _contactInfoService.SetAsync())
                .Where(x => x.DistributorID == distributor.DistributorID).ToList();
            distributor.Addresses = (await _addressService.SetAsync())
                .Where(x => x.DistributorID == distributor.DistributorID).ToList();
            distributor.Passport = await _passportService.FetchAsync(distributor.PassportID);

            return Ok(_mapper.Map<DistributorDTO>(distributor));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDistributor(DistributorCreateDTO distributorDTO)
        {
            var distributorEntity = _mapper.Map<Distributor>(distributorDTO);
            distributorEntity.DistributorGuid = Guid.NewGuid();
            if (distributorEntity.RecomendatorID != null && distributorEntity.RecomendatorID != 0)
            {
                var recomendator = await _distributorService.FetchAsync(distributorEntity.RecomendatorID.Value);
                if (recomendator.GenerationLinker != null)
                {
                    string[] elements = recomendator.GenerationLinker.Split(',');
                    if(elements.Count() == 5)
                        return BadRequest(new { error = "Try Other Recomendator, Its limit has been reached" });
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(recomendator.GenerationLinker);
                    stringBuilder.Append(",");
                    stringBuilder.Append(recomendator.DistributorID);
                    distributorEntity.GenerationLinker = stringBuilder.ToString();
                }
                else
                    distributorEntity.GenerationLinker = recomendator.DistributorID.ToString();
            }
            await _distributorService.SaveAsync(distributorEntity);

            var distributorReturn = _mapper.Map<DistributorDTO>(distributorEntity);
            return CreatedAtRoute("GetDistributor",
                new { id = distributorReturn.DistributorID },
                distributorReturn);
        }
    }
}
