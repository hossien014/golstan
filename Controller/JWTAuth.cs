using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;


[ApiController]
[Route("api/auth")]
public class JWTAuth : ControllerBase
{
    private readonly UserManager<IdentityUser> UserManager;

    public JWTAuth(UserManager<IdentityUser> userManager)
    {
        UserManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {

        var user = await UserManager.FindByEmailAsync(model.Email);

        if (user == null || !await UserManager.CheckPasswordAsync(user, model.Password))
        {

            return Unauthorized("Invalid credentials \n you can sign up in '/api/auth/sinup");
        }

        string token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
    [HttpPost("sinup")]
    public async Task<IActionResult> Signup(LoginModel model)
    {
        var userInDB = await UserManager.FindByEmailAsync(model.Email);
        if (userInDB != null)
        {
            return Conflict("Email already exists");
        }
        IdentityUser newUser = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await UserManager.CreateAsync(newUser, model.Password);
        if (result.Succeeded)
        {
            return Ok(new { SuccessfullyCreated = newUser });
        }
        else
        {
            return NotFound(result.Errors);
        }


    }
    private string GenerateJwtToken(IdentityUser user)
    {
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkeyawqjkqjwlkqjwoiqcojwoiqjwoijofoqjwoqjoljsakp"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var Claims = new Claim[]
        {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"Admin")
                // new Claim(ClaimTypes.Role,"user")
          };

        var Issuer = "issuer";
        var Audience = "audience";
        var Expire = DateTime.Now.AddMinutes(10);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: Claims,
            expires: Expire,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
public class LoginModel
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}