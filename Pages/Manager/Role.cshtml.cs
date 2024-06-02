using gol_razor._GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;



public class RoleModel(GolManager golManager) : PageModel
{
    public List<IdentityRole> roles = new();

    [BindProperty]
    public string roleName { get; set; }
    [TempData]
    public string massage { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {

        roles = await golManager.GetRoles();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await golManager.CreateRole(roleName);
            massage = "رول با موفقیت اضافه شد ";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            massage = e.Message;
            return RedirectToPage();
        }
    }


    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {

        try
        {
            await golManager.DeleteRole(id);
            massage = "رول با موفقیت حذف شد";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            massage = e.Message;
            return RedirectToPage();
        }

    }

}
