using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ManagerModel : PageModel
{


    public IActionResult OnGet()
    {

        // using (StreamReader sr = new StreamReader("test.txt"))
        // {
        //     var a = sr.ReadToEnd();
        //     System.Console.WriteLine(a);
        // }
        return Page();
    }
}

