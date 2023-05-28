using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

using PeyDej.Enums;

namespace PeyDej.Models.Users;

[Table("PeyDejUsers")]
public class PeyDejUser : IdentityUser
{
    [Display(Name = "نام")]
    [MaxLength(128)]
    public string FirstName { get; set; } = "";

    [Display(Name = "نام خانوادگی")]
    [MaxLength(128)]
    public string LastName { get; set; } = "";

    [Display(Name = "واحد مرتبط")]
    [MaxLength(128)]
    public string Department { get; set; } = "";

    [Display(Name = "وضعیت کاربر")] 
    public bool Enable { get; set; } = true;

    [Display(Name = "نقش کاربر")] 
    public UserRoles Role { get; set; } = UserRoles.User;

    [Display(Name = "کاربر باید کلمه عبور خود را تغییر دهد")]
    public bool UserChanePassword { get; set; }
}

