using AutoMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MarketerSysytem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public  class DistributorController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDistributorService _distributorService;
        public DistributorController(IMapper mapper, IDistributorService distributorService)
        {
            _distributorService = distributorService;
            _mapper = mapper;
        }

        

    }
}
