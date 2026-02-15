using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos.UserDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,AcademicAdmin,FinanceAdmin,HrAdmin")]
public class UsersController : ControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users)
    {
        _users = users;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
    {
        var result = await _users.CreateAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("import-excel")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        var result = await _users.ImportUsersFromExcelAsync(file);
        return StatusCode(result.StatusCode, result);
    }


    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] UserRole? role = null,
        [FromQuery] bool? isActive = null)
    {
        var result = await _users.GetPagedAsync(page, pageSize, search, role, isActive);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _users.GetByIdAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> Activate([FromRoute] Guid id)
    {
        var result = await _users.ActivateAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        var result = await _users.DeactivateAsync(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("{id:guid}/role")]
    public async Task<IActionResult> ChangeRole([FromRoute] Guid id, [FromBody] ChangeRoleDto dto)
    {
        var result = await _users.ChangeRoleAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{id:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromBody] ResetPasswordDto dto)
    {
        var result = await _users.ResetPasswordAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateDto dto)
    {
        var result = await _users.UpdateAsync(id, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _users.DeleteAsync(id);
        return StatusCode(result.StatusCode, result);
    }


}