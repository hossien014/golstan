using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class WardModel(DbContext context) : PageModel
{
    private readonly DbContext Context = context;

    public void OnGet()
    {

        
    }
}