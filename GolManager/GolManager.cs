using System.Globalization;
using gol_razor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using PersianDate.Standard;

namespace gol_razor._GolManager;


public class GolManager(GolestanContext context, RoleManager<IdentityRole> roleManager, ILogger<GolManager> logger)
{


    public async Task<List<Ward>> GetWards()
    {
        return await context.Wards.ToListAsync();
    }

    public async Task<Ward> GetWardById(int id)
    {
        var WardInDB = await context.Wards.FirstOrDefaultAsync(x => x.Id == id);

        if (WardInDB == null)
        {
            throw new GolManagerException($"No ward with Id :{id}", 404);
        }
        return WardInDB;
    }

    public async Task<Ward> DeleteWard(int id)
    {
        var wardInDB = context.Wards.FirstOrDefault(x => x.Id == id);
        if (wardInDB == null)
        {
            throw new GolManagerException($"No ward found with Id :{id}", 404);
        }
        context.Wards.Remove(wardInDB);
        await context.SaveChangesAsync();
        return wardInDB;

    }

    public async Task<Ward> CreateWard(Ward ward)
    {

        ward.Name = ward.Name.ToUpper();

        var wardInDB = context.Wards.FirstOrDefault(x => x.Name == ward.Name);
        if (wardInDB != null)
        {
            // return BadRequest($"{ward.Name} already exists");

            //confilct 409
            throw new GolManagerException($"{ward.Name} already exists", 409);
        }

        context.Wards.Add(ward);
        try
        {
            await context.SaveChangesAsync();
            return ward;
        }
        catch (Exception e)
        {
            throw new GolManagerException(e.Message, 500);
        }

    }

    public async Task<Ward> EditWard(int id, Ward ward)
    {
        if (id != ward.Id)
        {
            throw new GolManagerException("Id Mismatch", 400);
        }
        //modifying department with matching id

        ward.Name = ward.Name.ToUpper();
        context.Entry(ward).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync();
            return ward;
            //return Ok(new { Message = $"ward with id : {id} updated successfully", ward })
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (context.Wards.FirstOrDefault(x => x.Id == id) == null)
            {
                // return NotFound($"ward with id {id} not found for update.");
                throw new GolManagerException($"ward with id {id} not found for update.", 404);
            }

            throw new GolManagerException(e.Message, 500);
        }


    }

    public async Task CreateRole(string roleName)

    {

        if (await roleManager.RoleExistsAsync(roleName))
        {
            throw new GolManagerException($"{roleName} already exist", 409);
        }

        var result = await roleManager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {

        }
        else
        {
            throw new GolManagerException($"Failed to create {roleName}.{result.Errors}", 404);
        }

    }

    public async Task<List<IdentityRole>> GetRoles()
    {
        return await roleManager.Roles.ToListAsync();
    }

    public async Task DeleteRole(string id)
    {
        var roleToDelete = await roleManager.FindByIdAsync(id);
        if (roleToDelete == null)
        {
            throw new GolManagerException($"role dose not exist{id} ", 404);

        }

        await roleManager.DeleteAsync(roleToDelete);
    }

    #region staff 
    public async Task CreateStaff(Staff staff)
    {
        try
        {
            var a = await context.Staffs.AddAsync(staff);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new GolManagerException(e.Message, 500);
        }

    }

    public async Task<List<Staff>> GetStaffs()
    {
        var StaffList = await context.Staffs.Include(x => x.Ward).ToListAsync();
        return StaffList;
    }

    public void DeleteStaff(int staffId)
    {
        var staffToDelete = context.Staffs.FirstOrDefault(x => x.Id == staffId);
        if (staffToDelete != null)
        {
            context.Staffs.Remove(staffToDelete);
            context.SaveChanges();
        }
    }

    public async Task<Staff> GetStaff(int id)
    {
        try
        {
            var staff = await context.Staffs.FirstOrDefaultAsync(x => x.Id == id);
            if (staff == null)
            {
                throw new GolManagerException($"No staff found with id :{id}", 404);
            }
            return staff;
        }
        catch (Exception e)
        {
            throw new GolManagerException(e.Message, 500);
        }
    }

    public void EditStaff(Staff staff)
    {
        context.Entry(staff).State = EntityState.Modified;
        try
        {
            context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new GolManagerException(e.Message, 500);
        }

    }

    public async Task<Ward> GetWardWithStaff(string ward)
    {
        if (string.IsNullOrEmpty(ward))
        {
            throw new ArgumentException("Ward name is required");
        }

        ward = ward.ToUpper();

        var result = context.Wards.Include(x => x.Staffs).FirstOrDefault(x => x.Name == ward);
        if (result == null)
        {
            throw new GolManagerException("shit ", 404);
        }
        return result;
    }


    #endregion
    #region shifts 
    public void GetFirstAndLastDayOfGerigory(int year, int month, out DateTime firstDay, out DateTime lastDay)
    {

        firstDay = new DateTime(year, month, 1);
        var pc = new PersianCalendar();
        var dayCount = pc.GetDaysInMonth(pc.GetYear(firstDay), pc.GetDayOfMonth(firstDay));
        lastDay = firstDay.AddDays(dayCount - 1);

        logger.LogInformation("starting to add shifts first day of the month : {firstDay} and last day of the month{lastDay}"
        , firstDay, lastDay);

    }
    public void AddShifts(ShiftData save_list, bool _overrid = false)
    {
        var shiftNameList = new string[] { "m", "n", "o", "e", "M", "N", "O", "E" };

        var pc = new PersianCalendar();
        var day = 0;
        var firstDay = save_list.Date;
        var tmpDay = save_list.Date;
        var dayCount = pc.GetDaysInMonth(pc.GetYear(tmpDay), pc.GetDayOfMonth(tmpDay));
        var lastDay = tmpDay.AddDays(dayCount - 1);
        logger.LogInformation("starting to add shifts first day of the month : {firstDay} and last day of the month{lastDay}"
        , firstDay, lastDay);

        foreach (var shift in save_list.Shifts)
        {
            var name = shift.Key;
            var staffId = shift.Key.Split('/')[0];
            var WardId = shift.Key.Split('/')[1];
            logger.LogInformation("start to add shifts of staff with staff id : {StaffId} adm  ward id : {WardId}", staffId, WardId);
            day = 0;
            tmpDay = firstDay;
            foreach (var staffShift in shift.Value)
            {

                day += 1;
                if (tmpDay > lastDay)
                {
                    logger.LogError("the current : {currentDay} day is bigger that the last day of the month : {lastDay}"
                    , tmpDay, lastDay);
                    throw new GolManagerException("date is biger that shamsi month ", 404);
                }
                var currentShift = new Shift
                {
                    StaffId = int.Parse(staffId),
                    WardId = int.Parse(WardId),
                    Date = DateOnly.FromDateTime(tmpDay),
                    ShiftName = shiftNameList.Contains(staffShift) ? staffShift : "unknown"
                };
                tmpDay = tmpDay.AddDays(1);
                var shiftInDb = context.Shifts.FirstOrDefault(x => x.Date == currentShift.Date && x.StaffId == currentShift.StaffId);
                if (shiftInDb != null)
                {
                    logger.LogWarning("Shift already exists in the database. shift Id ={Id}", shiftInDb.Id);
                    if (_overrid == false) { continue; }
                    shiftInDb.ShiftName = currentShift.ShiftName;
                    logger.LogInformation("shift updated in the database queue shift Id ={Id}", shiftInDb.Id);

                }
                else
                {
                    context.Shifts.Add(currentShift);
                    logger.LogInformation("shift added to the database queue shift Id ={Id}", currentShift.Id);
                }
            }
            try
            {
                context.SaveChanges();
                logger.LogInformation("changes for this user successfully added to database . staff id : {StaffId} and  ward id : {WardId} added successfully in the date :{date}", staffId, WardId, firstDay);
            }
            catch (Exception e)
            {
                logger.LogError("Error in adding shifts of staff with staff id : {StaffId} adm  ward id : {WardId} in the date :{date} Error : {error}", staffId, WardId, firstDay, e.Message);
                throw new GolManagerException(e.Message, 500);
            }
        }
    }
    public async Task<List<Shift>> GetShiftsInMonth(int _staffId, int s_year, int s_month)
    {
        // figure out first and last day of the month 
        var pc = new PersianCalendar();
        // its give us date in gregorian calendar 
        DateOnly firstDay = new DateOnly(s_year, s_month, 1, pc);
        var days = pc.GetDaysInMonth(s_year, s_month);
        DateOnly lastDay = firstDay.AddDays(days - 1);
        logger.LogInformation("starting to get shifts first day of the month : {firstDay} and last day of the month{lastDay}"
        , firstDay, lastDay);
        try
        {
            List<Shift> shifts = await context.Shifts.Where(x => x.StaffId == _staffId && x.Date >= firstDay && x.Date <= lastDay)
            .OrderBy(x => x.Date).ToListAsync();
            logger.LogInformation("{shiftCount} shifts collected", shifts.Count);
            return shifts;
        }
        catch (Exception e)
        {
            logger.LogError("Error in getting shifts of staff with staff id : {StaffId} adm  ward id : {WardId} in the date :{date} Error : {error}", _staffId, s_year, s_month, e.Message);
            throw new GolManagerException(e.Message, 500);
        }
    }

    #endregion
}