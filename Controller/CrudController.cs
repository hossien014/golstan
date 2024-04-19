using gol_razor;
using gol_razor.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/wards")]
[Authorize(Policy ="AdminPolicy",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
public class CrudController : ControllerBase
{
    private readonly GolestanContext Context;

    public CrudController(GolestanContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Gets a list of all staff members.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Ward>>> GetWards()
    {
        return await Context.Wards.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Ward>> GetWardByID(int id)
    {
        var WardInDB = await Context.Wards.FirstOrDefaultAsync(x => x.Id == id);

        if (WardInDB == null)
        {
            return NotFound($"No ward found with Id :{id}");
        }

        return WardInDB;
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var wardInDB = Context.Wards.FirstOrDefault(x => x.Id == id);
        if (wardInDB == null)
        {
            return NotFound($"No ward found with Id :{id}");
        }
        Context.Wards.Remove(wardInDB);
        await Context.SaveChangesAsync();
        return Ok($"ward with id : {id} and name :{wardInDB.Name} deleted successfully");

    }
    [HttpPost]
    public async Task<IActionResult> Post(Ward ward)
    {

        ward.Name = ward.Name.ToUpper();

        var wardInDB = Context.Wards.FirstOrDefault(x => x.Name == ward.Name);
        if (wardInDB != null)
        {
            return BadRequest($"{ward.Name} already exists");
        }

        Context.Wards.Add(ward);
        try
        {

            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }

        return CreatedAtAction(nameof(GetWardByID), ward);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Ward ward)
    {
        if (id != ward.Id)
        {
            return BadRequest("Id Mismatch");
        }
        ward.Name = ward.Name.ToUpper();
        //modifying department with matching id
        Context.Entry(ward).State = EntityState.Modified;
        try
        {
            await Context.SaveChangesAsync();
            return Ok(new { Message = $"ward with id : {id} updated successfully", ward });
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (Context.Wards.FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound($"ward with id {id} not found for update.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
}