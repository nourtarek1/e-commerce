using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Sherd;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
  public class AuthService(UserManager<AppUser> userManager, IOptions<JwtOptions> options) : IAuthService


    
  {


   

    public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
    {
      var user = await userManager.FindByEmailAsync(loginDto.Email);
      if (user is null) throw new Exception("User not found");

      var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
      if (!flag) throw new Exception("Invalid password"); ;
      return new UserResultDto
      {
        DisplayName = user.DisplayName,
        Email = user.Email,
        Token = await GenerateJWTTokenAsync(user),
      };
    }

    public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
    {
      // التحقق من وجود الإيميل مسبقًا
      if (await CheckEmailExistAsync(registerDto.Email))
        throw new Exception($"Email '{registerDto.Email}' is already in use.");

      // إعداد بيانات المستخدم
      var user = new AppUser
      {
        DisplayName = registerDto.DisplayName,
        Email = registerDto.Email.Trim(),
        UserName = string.IsNullOrWhiteSpace(registerDto.UserName)
              ? registerDto.Email
              : registerDto.UserName,
        PhoneNumber = string.IsNullOrWhiteSpace(registerDto.PhoneNumber)
              ? null
              : registerDto.PhoneNumber
      };

      // إنشاء المستخدم باستخدام Identity
      var result = await userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded)
      {
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new Exception($"Registration failed: {errors}");
      }

      // النتيجة النهائية
      return new UserResultDto
      {
        DisplayName = user.DisplayName,
        Email = user.Email,
        Token = await GenerateJWTTokenAsync(user)

      };
    }


    public async Task<bool> CheckEmailExistAsync(string email)
    {
      var user = await userManager.FindByEmailAsync(email);
      return user != null;
    }


    public async Task<ForgotPasswordDto> ResetPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
      var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

      if (user is null)
        throw new Exception("User not found");

      var token = await userManager.GeneratePasswordResetTokenAsync(user);

      var resetLink = $"https://localhost:7097/reset-password?email={Uri.EscapeDataString(forgotPasswordDto.Email)}&token={Uri.EscapeDataString(token)}";

      // مؤقتًا نطبع التوكن واللينك في اللوج أو نرجعهم للعميل
      return new ForgotPasswordDto
      {
        Email = forgotPasswordDto.Email,
        Token = token,
        ResetLink = resetLink
      };
    }



    public async Task<string> SendResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
      var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);

      if (user is null)
        throw new Exception("User not found");

      // تنفيذ إعادة تعيين كلمة المرور
      var result = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

      if (!result.Succeeded)
      {
        var errors = result.Errors.Select(e => e.Description);
        throw new Exception(string.Join(", ", errors));
      }

      // ممكن ترجّع نفس الـ DTO أو تعمل Response خاص
      return "Password reset successfully";
    }

    public async Task<UserResultDto> GetCurrentUserAsync(string email)
    {
      var user = await userManager.FindByEmailAsync(email);
      if (user is null) throw new Exception("User email not found in token.");
      return new UserResultDto
      {
        DisplayName = user.DisplayName,
        Email = user.Email,
        Token = await GenerateJWTTokenAsync(user),
      };

    }


    private async Task<string> GenerateJWTTokenAsync(AppUser user)
    {
      var jwtOptions = options.Value;

      var authClaim = new List<Claim>()
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id), // 👈 أضف ده
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
    };

      var roles = await userManager.GetRolesAsync(user);
      foreach (var role in roles)
      {
        authClaim.Add(new Claim(ClaimTypes.Role, role));
      }

      var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

      var token = new JwtSecurityToken(
          issuer: jwtOptions.Issuer,
          audience: jwtOptions.Audience,
          claims: authClaim,
          expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
          signingCredentials: new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256Signature)
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }


  }

}
