using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BoardGamesApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BoardGamesApi.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : Controller
{
    private readonly IConfiguration config;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public LoginController(IConfiguration config,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        this.config = config;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 401)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var user = await userManager.FindByNameAsync(loginModel.Username ?? "");
        if (user == null)
        {
            return NotFound(new ErrorResponse { Error = "User not found" });
        }

        var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new ErrorResponse { Error = "Invalid login credentials" });
        }

        return Json(new TokenResponse { Token = GenerateToken(user) });
    }

    private string GenerateToken(IdentityUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384);
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id) };

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"]!,
            config["Jwt:Audience"]!,
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}