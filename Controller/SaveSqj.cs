using Microsoft.AspNetCore.Mvc;
using static ShiftManagerModel;
using Newtonsoft.Json;
using gol_razor.Models;
using gol_razor._GolManager;
using System.Globalization;
namespace gol_razor.Controllers;
public class SaveSqj(GolManager golManager) : ControllerBase
{

    [Route("api/save")]
    [HttpPost]
    public IActionResult Save([FromBody] ShiftData save_list)
    {
        try
        {
            golManager.AddShifts(save_list, true);

        }
        catch (GolManagerException e)
        {

            return BadRequest(e.Message);
        }
        return Ok("Shift saved successfully");
    }

    [HttpPost]
    [Route("api/getshifts")]
    public async Task<IActionResult> GetShiftsInMonth(int year, int month, int staff_id)
    {
        var pc = new PersianCalendar();
        var d = new DateTime(1403, 2, 1, pc);
        try
        {
            var shifts = await golManager.GetShiftsInMonth(staff_id, year, month);
            return Ok(shifts);
        }
        catch (GolManagerException e)
        {
            return BadRequest(e.Message);
        }
    }
}
