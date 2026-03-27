using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Infrastructure.Dtos.ExamResultDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[ApiController]
[Route("api/enrollment-courses/{enrollmentCourseId:guid}/exam-results")]
[Authorize(Roles = "SuperAdmin,AcademicAdmin")]
public class ExamResultsController : ControllerBase
{
    private readonly IExamResultService _service;

    public ExamResultsController(IExamResultService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid enrollmentCourseId, [FromBody] ExamResultCreateDto dto)
    {
        var result = await _service.CreateAsync(enrollmentCourseId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid enrollmentCourseId)
    {
        var result = await _service.GetAllAsync(enrollmentCourseId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid enrollmentCourseId, Guid id)
    {
        var result = await _service.GetByIdAsync(enrollmentCourseId, id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid enrollmentCourseId, Guid id, [FromBody] ExamResultUpdateDto dto)
    {
        dto.Id = id;
        var result = await _service.UpdateAsync(enrollmentCourseId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid enrollmentCourseId, Guid id)
    {
        var result = await _service.DeleteAsync(enrollmentCourseId, id);
        return StatusCode(result.StatusCode, result);
    }
}
