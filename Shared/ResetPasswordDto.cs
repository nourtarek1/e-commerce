using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherd
{
  public class ResetPasswordDto
  {
    public string Email { get; set; }
    public string Token { get; set; }

    [Required(ErrorMessage = "New Password Is Requird")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }


    [Required(ErrorMessage = "New Password Is Requird")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password dose not match the New Password")]
    public string ConfirmPassword { get; set; }
  }
}
