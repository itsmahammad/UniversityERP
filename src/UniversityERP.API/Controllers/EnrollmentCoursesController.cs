using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[ApiController]
[Route("api/student-semester-enrollments/{studentSemesterEnrollmentId:guid}/courses")]
[Authorize(Roles = "SuperAdmin,AcademicAdmin")]
public class EnrollmentCoursesController : ControllerBase
{
    private readonly IEnrollmentCourseService _service;

    public EnrollmentCoursesController(IEnrollmentCourseService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid studentSemesterEnrollmentId, [FromBody] EnrollmentCourseCreateDto dto)
    {
        var result = await _service.CreateAsync(studentSemesterEnrollmentId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid studentSemesterEnrollmentId)
    {
        var result = await _service.GetAllAsync(studentSemesterEnrollmentId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid studentSemesterEnrollmentId, Guid id)
    {
        var result = await _service.GetByIdAsync(studentSemesterEnrollmentId, id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid studentSemesterEnrollmentId, Guid id)
    {
        var result = await _service.DeleteAsync(studentSemesterEnrollmentId, id);
        return StatusCode(result.StatusCode, result);
    }
}
