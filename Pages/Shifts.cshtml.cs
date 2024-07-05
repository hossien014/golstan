using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using gol_razor;
using gol_razor._GolManager;
using Microsoft.EntityFrameworkCore;
using gol_razor.Models;
public class ShiftModel(GolestanContext context, ILogger<ShiftModel> logger, GolManager golManager) : PageModel
{

    public int WardID { get; set; }
    public List<DateTime> First_Last_Day { get; set; } = new List<DateTime>();
    public Dictionary<DateOnly, Dictionary<string, List<string>>> sd { get; set; } = new();
    public List<Shift> month_shifts { get; set; }
    public int DaysCount { get; set; }
    public IActionResult OnGet(string ward_name)

    {
        //find id :
        if (string.IsNullOrEmpty(ward_name))
        {
            logger.LogError("no ward name");
            return NotFound("please enter ward name");
        }
        ward_name = ward_name.ToUpper();
        var ward_in_db = context.Wards.FirstOrDefault(x => x.Name == ward_name);
        if (ward_in_db == null)
        {
            logger.LogError("no ward name : {0}", ward_name);
            return NotFound($"ward not found : {ward_name}");
        }
        logger.LogInformation("ward name : {0} have id of: {1} ", ward_name, ward_in_db.Id);
        WardID = ward_in_db.Id;

        var s = new Shamsi();
        First_Last_Day = s.GetFirst_Last_month(DateTime.Now, out int _DaysCount);
        DaysCount = _DaysCount;
        System.Console.WriteLine(DaysCount);
        var first_day = DateOnly.FromDateTime(First_Last_Day[0]);
        var last_day = DateOnly.FromDateTime(First_Last_Day[1]);

        month_shifts = context.Shifts.Include(x => x.Staff).Where(x => x.WardId == WardID && x.Date >= first_day && x.Date <= last_day)
       .OrderBy(x => x.StaffId).ToList();
        // tosdo : sort by staffs 

        DateOnly date_t;
        string shift_name_t;
        string staff_name_t;
        for (int i = 0; i < month_shifts.Count; i++)
        {
            date_t = month_shifts[i].Date;
            shift_name_t = month_shifts[i].ShiftName;
            staff_name_t = month_shifts[i].Staff.FirstName +" "+month_shifts[i].Staff.LastName;


            if (!sd.ContainsKey(date_t))
            {
                //we have key but no value
                //we have to create new dictionary with key of string and value of liste of string
                var tmp = new Dictionary<string, List<string>>
                {
                    { shift_name_t, new List<string>() { staff_name_t } }
                };
                sd.Add(date_t, tmp);

            }
            else
            {
                if (!sd[date_t].ContainsKey(shift_name_t))
                {
                    // wa have to make new dictonary with key of string and value of liste of string
                    var t = new Dictionary<string, List<string>>
                    {
                        {shift_name_t,new List<string> { staff_name_t } }
                    };

                    sd[date_t].Add(shift_name_t, new List<string>() { staff_name_t });
                }

                else
                {
                    if (sd[date_t][shift_name_t] == null)
                    {
                        sd[date_t][shift_name_t] = new List<string>();
                    }
                    sd[date_t][shift_name_t].Add(staff_name_t);
                }
            }

        }

        return Page();

    }
}