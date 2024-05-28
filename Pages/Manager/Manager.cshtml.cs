using FastExcel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ManagerModel : PageModel
{


    public IActionResult OnGet()
    {
        System.Console.WriteLine("hi");

        using (FastExcel.FastExcel fe = new FastExcel.FastExcel(new FileInfo("e.xlsx")))
        {
            Worksheet sheet = fe.Read(1);
            var a = sheet.Rows.First().Cells.FirstOrDefault()?.Value;
            var rows = sheet.Rows.ToList();
            var row_1 = rows[1].Cells.ToList();
            var row_2 = rows[2].Cells.ToList();
        

            System.Console.WriteLine();
        }

        // using (StreamReader sr = new StreamReader("test.txt"))
        // {
        //     var a = sr.ReadToEnd();
        //     System.Console.WriteLine(a);
        // }
        return Page();
    }
}

