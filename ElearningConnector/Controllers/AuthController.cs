using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElearningConnector.Data;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using ElearningConnector.Models.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/authen")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    /// <summary>
    /// Đăng nhập hệ thống
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AuthRequest request)
    {
        if (request == null)
        {
            return Ok(ApiResponse<string>.Error(ApiErrorCode.InvalidCredentials));
        }
        try
        {
            // 1. Mã hóa password
            var hashedPassword = PasswordHelper.EncodePassword(request.PassWord);

            // 2. Truy vấn user
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == request.UserName && u.Password == hashedPassword);
            if (user == null)
            {
                return Ok(ApiResponse<string>.Error(ApiErrorCode.InvalidCredentials));
            }

            // 3. Sinh JWT token
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSection["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(2);
            var claims = new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
                new System.Security.Claims.Claim("UserId", user.PK_UserID.ToString()),
                new System.Security.Claims.Claim("UserType", user.UserType.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 4. Trả về token trong ApiResponse
            return Ok(ApiResponse<string>.Success(tokenString, "Đăng nhập thành công"));
        }
        catch (Exception ex)
        {
            return Ok(ApiResponse<string>.Error(ApiErrorCode.SystemError));
        }
    }
}