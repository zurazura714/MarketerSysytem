using MapsterMapper;
using MarketerSystem.Abstractions.Service;
using MarketerSystem.Common.DTO;
using MarketerSystem.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace MarketerSysytem.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BonusPaymentController(IMapper mapper, IBonusPaymentService bonusPaymentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GenerateAsync([FromQuery] PaymentParameters parameters)
    {
        await bonusPaymentService.GenerateBonusPaymentsForPeriodAsync(parameters);
        return Created();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BonusPaymentDTO>>> GetAsync([FromQuery] PaymentFilterParameters parameters)
    {
        var payments = await bonusPaymentService.FilterPaymentsProducts(parameters);
        return Ok(mapper.Map<IEnumerable<BonusPaymentDTO>>(payments));
    }
}
