using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

public class LogoutModel : PageModel
{

    private readonly SignInManager<IdentityUser> _signInManager;

    public LogoutModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
    }

    [TempData]
    public string _message { get; set; }

    public async Task OnGetAsync()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        System.Console.WriteLine("aa");
        await _signInManager.SignOutAsync();
        _message = "با موفقیت از حساب کاربری خارج شدید";
        return RedirectToPage();
    }

}