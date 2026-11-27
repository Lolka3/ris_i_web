using System.Security.Claims;
using backend.Data;
using backend.Dtos;
using backend.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    AppDbContext dbContext,
    IPasswordHasher<User> passwordHasher) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticatedUserResponse>> Login(
        LoginRequest input,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .SingleOrDefaultAsync(item => item.Login == input.Login.Trim(), cancellationToken);
        if (user is null ||
            passwordHasher.VerifyHashedPassword(user, user.PasswordHash, input.Password) ==
            PasswordVerificationResult.Failed)
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Неверный логин или пароль"
            });
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        return Ok(ToResponse(user));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<AuthenticatedUserResponse> GetCurrentUser()
    {
        if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ||
            User.Identity?.Name is not { } login ||
            User.FindFirstValue(ClaimTypes.GivenName) is not { } fullName ||
            User.FindFirstValue(ClaimTypes.Role) is not { } role)
        {
            return Unauthorized();
        }

        return Ok(new AuthenticatedUserResponse(id, login, fullName, role));
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    private static AuthenticatedUserResponse ToResponse(User user) =>
        new(user.Id, user.Login, user.FullName, user.Role);
}
