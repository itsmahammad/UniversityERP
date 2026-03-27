using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Infrastructure.Dtos.AttendanceRecordDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[ApiController]
[Route("api/attendance-sessions/{attendanceSessionId:guid}/records")]
[Authorize(Roles = "SuperAdmin,AcademicAdmin")]
public class AttendanceRecordsController : ControllerBase
{
    private readonly IAttendanceRecordService _service;

    public AttendanceRecordsController(IAttendanceRecordService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid attendanceSessionId, [FromBody] AttendanceRecordCreateDto dto)
    {
        var result = await _service.CreateAsync(attendanceSessionId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid attendanceSessionId)
    {
        var result = await _service.GetAllAsync(attendanceSessionId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid attendanceSessionId, Guid id)
    {
        var result = await _service.GetByIdAsync(attendanceSessionId, id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid attendanceSessionId, Guid id, [FromBody] AttendanceRecordUpdateDto dto)
    {
        dto.Id = id;
        var result = await _service.UpdateAsync(attendanceSessionId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid attendanceSessionId, Guid id)
    {
        var result = await _service.DeleteAsync(attendanceSessionId, id);
        return StatusCode(result.StatusCode, result);
    }
}
