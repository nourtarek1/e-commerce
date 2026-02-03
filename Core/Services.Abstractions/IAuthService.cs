using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthService
    {
    Task<UserResultDto> LoginAsync(LoginDto loginDto);
    Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
    Task<ForgotPasswordDto> ResetPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<string> SendResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<UserResultDto> GetCurrentUserAsync(string email);


    Task<bool> CheckEmailExistAsync(string email);


  }
}
