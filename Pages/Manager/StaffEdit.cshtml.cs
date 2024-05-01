using gol_razor.GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

public class StaffEditModel(GolManager golManager) : PageModel
{
    [BindProperty]
    public Staff staff { get; set; }
    [TempData]
    public string message { get; set; }

    public SelectList WardList { get; set; }
    public SelectList RoleList { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            staff = await golManager.GetStaff(id);
            var wards = await golManager.GetWards();
            var roles = await golManager.GetRoles();
            WardList = new SelectList(wards, "Id", "Name");
            RoleList = new SelectList(roles, "Id", "Name");
            return Page();
        }
        catch (GolManagerException e)
        {
            message = e.Message;
           return RedirectToPage("staff");
        }
        

    }
    // update staff 
    public IActionResult OnPost()
    {
        try
        {
            golManager.EditStaff(staff);
            message = "staff updated successfully";
            return RedirectToPage();
        }
        catch (GolManagerException e)
        {
            message = e.Message;
            return Page();
        }

    }

}