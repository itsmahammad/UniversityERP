using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Infrastructure.Dtos.FacultyDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[Authorize(Roles = "SuperAdmin")]
[Route("api/[controller]")]
[ApiController]
public class FacultiesController(IFacultyService _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateFaculty(FacultyCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] FacultyUpdateDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteFaculty([FromRoute] Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }
}
