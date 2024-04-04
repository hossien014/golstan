using gol_razor;
using gol_razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/department")]
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

    public async Task<ActionResult<IEnumerable<Department>>> Get()
    {
        return await Context.Departments.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> Get(int id)
    {
        var departmentInDB = await Context.Departments.FirstOrDefaultAsync(x => x.Id == id);

        if (departmentInDB == null)
        {
            return NotFound($"No department found with Id :{id}");
        }

        return departmentInDB;
    }



    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var departmentInDB = Context.Departments.FirstOrDefault(x => x.Id == id);
        if (departmentInDB == null)
        {
            return NotFound($"No department found with Id :{id}");
        }
        Context.Departments.Remove(departmentInDB);
        await Context.SaveChangesAsync();
        return Ok($"Department with id : {id} and name :{departmentInDB.Name} deleted successfully");

    }
    [HttpPost]
    public async Task<IActionResult> Post(Department department)
    {

        department.Name = department.Name.ToUpper();

        var departmentInDB = Context.Departments.FirstOrDefault(x => x.Name == department.Name);
        if (departmentInDB != null)
        {
            return BadRequest($"{department.Name} already exists");
        }

        Context.Departments.Add(department);
        try
        {

            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }

        return CreatedAtAction(nameof(Get), department);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Department department)
    {
        if (id != department.Id)
        {
            return BadRequest("Id Mismatch");
        }
        department.Name = department.Name.ToUpper();
        //modifying department with matching id
        Context.Entry(department).State = EntityState.Modified;
        try
        {
            await Context.SaveChangesAsync();
            return Ok(new { Message = $"Department with id : {id} updated successfully", department });
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (Context.Departments.FirstOrDefault(x => x.Id == id) == null)
            {
                return NotFound($"Department with id {id} not found for update.");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
}