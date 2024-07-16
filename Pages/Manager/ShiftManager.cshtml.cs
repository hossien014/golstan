
using System.Globalization;
using gol_razor._GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Common;
public class ShiftManagerModel(GolManager golManager) : PageModel
{
    public Dictionary<string, string> dayname = new Dictionary<string, string>
    {
      {"Saturday","شنبه" },
    {"Sunday","یکشنبه"},
     {"Monday","دوشنبه"}   ,
    { "Tuesday","سه شنبه"},
    {"Wednesday","چهارشنبه"},
    {"Thursday","پنجشنبه"},
    {"Friday","جمعه"}
    };

    [TempData]
    public string message { get; set; }
    public List<DateTime> DaysOfMonth { get; set; }

    public Ward Ward { get; set; }
    public List<Staff> Staffs { get; set; }
    public List<Ward> Wards { get; set; }

    public Dictionary<int, List<Shift>> IdAndShifts { get; set; } = new Dictionary<int, List<Shift>>();
    public class SqJMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Name { get; set; }
        public int DayCount { get; set; }
        public char d1 { get; set; }
        public char d2 { get; set; }
        public char d3 { get; set; }
        public char d4 { get; set; }
        public char d5 { get; set; }
        public char d6 { get; set; }
        public char d7 { get; set; }
        public char d8 { get; set; }
        public char d9 { get; set; }
        public char d10 { get; set; }
        public char d11 { get; set; }
        public char d12 { get; set; }
        public char d13 { get; set; }
        public char d14 { get; set; }
        public char d15 { get; set; }
        public char d16 { get; set; }
        public char d17 { get; set; }
        public char d18 { get; set; }
        public char d19 { get; set; }
        public char d20 { get; set; }
        public char d21 { get; set; }
        public char d22 { get; set; }
        public char d23 { get; set; }
        public char d24 { get; set; }
        public char d25 { get; set; }
        public char d26 { get; set; }
        public char d27 { get; set; }
        public char d28 { get; set; }
        public char d29 { get; set; }
        public char d30 { get; set; }
        public char d31 { get; set; }


    }

    [BindProperty]
    public SqJMonth input { get; set; }

    public async Task<IActionResult> OnGetAsync(string ward, int Year, int Month)
    {
        if (string.IsNullOrEmpty(ward))
        {
            message = "بخش وارد نشده است ";
            Ward = null;
            Wards = await golManager.GetWards();
            return Page();
        }
        System.Console.WriteLine($"year {Year}");
        if (Year == 0 && Month == 0)
        {
            var pc = new PersianCalendar();

            var today = DateTime.Today;
            Year = pc.GetYear(today);
            Month  =  pc.GetMonth(today);
        }
        try
        {

            Ward = await golManager.GetWardWithStaff(ward);
            Staffs = Ward.Staffs.ToList();

            for (int i = 0; i < Staffs.Count; i++)
            {
                var id = Staffs[i].Id;
                var shifts = await golManager.GetShiftsInMonth(id, Year, Month);
                IdAndShifts[id] = shifts;
            }

        }
        catch (GolManagerException ex)
        {
            return StatusCode(ex.StatusCode, ex.Message);
        }
        var s = new Shamsi();
        DaysOfMonth = s.GetDaysOftheMonth(Year, Month);
        return Page();

    }

}