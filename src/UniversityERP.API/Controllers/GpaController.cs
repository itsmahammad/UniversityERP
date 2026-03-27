using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,AcademicAdmin")]
public class GpaController : ControllerBase
{
    private readonly IGpaService _service;

    public GpaController(IGpaService service)
    {
        _service = service;
    }

    [HttpGet("semester/{studentSemesterEnrollmentId:guid}")]
    public async Task<IActionResult> GetSemesterGpa(Guid studentSemesterEnrollmentId)
    {
        var result = await _service.GetSemesterGpaAsync(studentSemesterEnrollmentId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("cumulative/student/{studentId:guid}")]
    public async Task<IActionResult> GetCumulativeGpa(Guid studentId)
    {
        var result = await _service.GetCumulativeGpaAsync(studentId);
        return StatusCode(result.StatusCode, result);
    }
}
