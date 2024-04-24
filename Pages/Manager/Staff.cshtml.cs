using gol_razor.GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class StaffModel(GolManager golManager) : PageModel
{
    [BindProperty]
    Staff staff { get; set; }

    public async Task OnGetAsync()
    {

    }

    public async Task OnPostAsync()
    {

        
    }
}