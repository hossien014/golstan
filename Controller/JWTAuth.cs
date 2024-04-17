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

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {

        var user = await UserManager.FindByEmailAsync(model.Email);

        if (user == null || !await UserManager.CheckPasswordAsync(user, model.Password))
        {

            return Unauthorized("Invalid credentials");
        }

        string token = GenerateJwtToken(user);
        return Ok(new {Token = token});
    }

    public string GenerateJwtToken(IdentityUser user)
    {

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretkeyawqjkqjwlkqjwoiqcojwoiqjwoijofoqjwoqjoljsakp"));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var Claims = new Claim[]
        {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"Admin")
          };

        var Issuer = "issuer";
        var Audience = "audience";
        var Expire = DateTime.Now.AddMinutes(1);

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