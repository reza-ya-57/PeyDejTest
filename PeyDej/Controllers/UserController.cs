using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeyDej.Data;
using PeyDej.Models.Users;

namespace PeyDej.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly PeyDejContext _context;
    private readonly SignInManager<PeyDejUser> _signInManager;
    private readonly UserManager<PeyDejUser> _userManage;

    public UserController(PeyDejContext context, UserManager<PeyDejUser> userManager,
        SignInManager<PeyDejUser> signInManager)
    {
        _context = context;
        _userManage = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View(new LoginPage());
    }

    [HttpPost]
    [ActionName("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginPage(LoginPage login)
    {
        if (login.Username == null || login.Password == null)
        {
            ModelState.Clear();
            return View(login);
        }

        if (_userManage.Users.ToList().Count == 0)
        {
            PeyDejUser tblUser = new PeyDejUser()
            {
                FirstName = "کاربر",
                LastName = "ارشد",
                UserName = "admin",
                Email = "admin@localhost.local",
                Department = "فناوری اطلاعات",
                Enable = true,
            };
            var resultUser = _userManage.CreateAsync(tblUser, "Admin@123");
            if (resultUser.Result != IdentityResult.Success)
            {
                ModelState.AddModelError("", "خطا در ایجاد کاربر");
            }

            await _userManage.AddClaimAsync(tblUser, new Claim(ClaimTypes.Role, "Admin"));
        }

        if (ModelState.IsValid)
        {
            var result =
                await _signInManager.PasswordSignInAsync(login.Username, login.Password, login.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Username", "کاربر به صورت موقت از دسترس خارج شد.");
            }
            else if (result.Succeeded)
            {
                var user = await _userManage.FindByNameAsync(login.Username);
                if (user.Enable)
                {
                    if (user.UserChanePassword)
                    {
                        //_notifyService.Warning("شما در حال استفاده از کلمه عبور پیش فرض می باشید. جهت ارتقای امنیت لطفا نسبت به تغییر کلمه عبور اقدام نمایید.");
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "کاربری شما غیر فعال شده است. لطفا با راهبر سیستم تماس بگیرید.");
                }
            }
            else
            {
                ModelState.AddModelError("", "نام کاربری یا کلمه عبور صحیح نمی باشد.");
            }
        }

        login.RememberMe = true;
        return View(login);
    }
    
    public IActionResult Logout()
    {
        _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}