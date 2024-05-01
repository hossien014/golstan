using gol_razor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace gol_razor.GolManager;


public class GolManager(GolestanContext context, RoleManager<IdentityRole> roleManager)
{


    public async Task<IEnumerable<Ward>> GetWards()
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


    #endregion
}