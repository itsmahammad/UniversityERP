using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityERP.Infrastructure.Dtos.UserDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IUserService _users;

    public AccountController(IUserService users)
    {
        _users = users;
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var result = await _users.GetMeAsync();
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var result = await _users.ChangeMyPasswordAsync(dto);
        return StatusCode(result.StatusCode, result);
    }
}