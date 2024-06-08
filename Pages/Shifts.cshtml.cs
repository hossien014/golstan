using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using gol_razor;
using gol_razor._GolManager;
public class ShiftModel(GolestanContext context, ILogger<ShiftModel> logger, GolManager golManager) : PageModel
{

    public int WardID { get; set; }
    public List<DateTime> First_Last_Day { get; set; } = new List<DateTime>();
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
        First_Last_Day = s.GetFirst_Last_month(DateTime.Now);
        var first_day = DateOnly.FromDateTime(First_Last_Day[0]);
        var last_day = DateOnly.FromDateTime(First_Last_Day[1]);
        var all_ward_shift_in_month = context.Shifts.Where(x => x.WardId == WardID && x.Date >= first_day && x.Date <= last_day).ToList();
        // tosdo : sort by staffs 
        //       


        return Page();

    }
}