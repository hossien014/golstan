using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class DepartmentModel(DbContext context) : PageModel
{
    private readonly DbContext Context = context;

    public void OnGet()
    {

        
    }
}