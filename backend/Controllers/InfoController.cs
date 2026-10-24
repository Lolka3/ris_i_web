using Microsoft.AspNetCore.Mvc;
using backend.Services;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoController : ControllerBase
{
    private readonly IHealthService _healthService;

    public InfoController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new { status = _healthService.GetStatus() });
    }
}