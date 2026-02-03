using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Sherd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


namespace Presentaion
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController(IServiceManager serviceManager) : ControllerBase
  {
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
      var result = await serviceManager.AuthService.LoginAsync(loginDto);
      return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
      var result = await serviceManager.AuthService.RegisterAsync(registerDto);
      return Ok(result);
    }

    [HttpPost("Forget-Password")]
    public async Task<IActionResult> ForgetPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
      var result = await serviceManager.AuthService.ResetPasswordAsync(forgotPasswordDto);
      return Ok(result);

    }
    [HttpPost("Reset-Password")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {

      var result = await serviceManager.AuthService.SendResetPasswordAsync(resetPasswordDto);
      return Ok(result);
    }




    [HttpGet("EmailExists")]
    public async Task<IActionResult> CheckEmailExists(string email)
    {
      var result = await serviceManager.AuthService.CheckEmailExistAsync(email);
      return Ok(result);
    }



    [HttpGet("GetCurrentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
      var email = User.FindFirstValue(ClaimTypes.Email);
      var result = await serviceManager.AuthService.GetCurrentUserAsync(email);
      return Ok(result);
    }
    


  }
}
