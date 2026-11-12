
using EmployeeManagement.API.DTOs;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(result);
    }

    [HttpPost("setup")]
    public async Task<IActionResult> SetupInitialAdmin([FromBody] SetupAdminDto setupDto)
    {
        var created = await _authService.SetupInitialAdminAsync(setupDto);
        if (!created)
        {
            return Conflict(new { message = "Initial administrator setup is unavailable." });
        }

        return StatusCode(StatusCodes.Status201Created,
            new { message = "Administrator created. Sign in to continue." });
    }
}
