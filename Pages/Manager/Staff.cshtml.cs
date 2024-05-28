using gol_razor._GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

public class StaffModel(GolManager golManager) : PageModel
{
    [BindProperty]
    public Staff staff { get; set; }
    public List<Staff> staff_list { get; set; }
    public SelectList WardList { get; set; }
    public SelectList RoleList { get; set; }
    [TempData]
    public string message { get; set; }
    public async Task OnGetAsync(int? Id)
    {
        staff_list = await golManager.GetStaffs();
        var wards = await golManager.GetWards();
        var roles = await golManager.GetRoles();
        WardList = new SelectList(wards, "Id", "Name");
        RoleList = new SelectList(roles, "Id", "Name");
    }

    public async Task<IActionResult> OnPostAsync()
    {

        System.Console.WriteLine("");
        //create new staff 
        try
        {
            await golManager.CreateStaff(staff);
            message="new staff added";
            return RedirectToPage();
        }
        catch (GolManagerException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }

    }
    public async Task<IActionResult> OnPostDelete(int id)
    {
        golManager.DeleteStaff(id);
        message="staff deleted";
        return RedirectToPage();
    }
}