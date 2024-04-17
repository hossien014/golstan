using gol_razor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using gol_razor;
using Microsoft.AspNetCore.Mvc;
public class WardModel(GolestanContext context) : PageModel
{
    private readonly GolestanContext Context = context;

    public List<Ward> wards { get; set; }

    public async Task<IActionResult> OnGet()
    {
        wards = await Context.Wards.ToListAsync();
        return Page();
    }

}