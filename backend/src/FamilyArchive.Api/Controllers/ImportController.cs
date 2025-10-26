using Microsoft.AspNetCore.Mvc;

namespace FamilyArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportController : ControllerBase
{
    private readonly IMassImportService _importService;

    public ImportController(IMassImportService importService)
    {
        _importService = importService;
    }

    [HttpPost]
    [Route("mass")]
    public async Task<IActionResult> MassImport([FromForm] IFormFile file)
    {
        var result = await _importService.ImportAsync(file);
        return Ok(result);
    }
}