using System.ComponentModel.DataAnnotations;

namespace PeyDej.Models.Users;

public class LoginPage
{
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Display(Name = "نام کاربری")]
    [MaxLength(256)]
    public string Username { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "{0} را وارد کنید")]
    [Display(Name = "کلمه عبور")]
    [MaxLength(256)]
    public string Password { get; set; }
    
    [Display(Name = "مرا به خاطر بسپار")]
    public bool RememberMe { get; set; } = true;
}