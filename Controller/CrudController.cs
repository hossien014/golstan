using gol_razor;
using gol_razor._GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/wards")]
// [Authorize(Policy ="AdminPolicy",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
public class CrudController : ControllerBase
{
    private readonly GolestanContext Context;
    private readonly GolManager golManager;
    public CrudController(GolestanContext context, GolManager _golmananger)
    {
        Context = context;
        golManager = _golmananger;
    }

    /// <summary>
    /// Gets a list of all staff members.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Ward>>> GetWards()
    {
        return Ok(golManager.GetWards().Result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ward>> GetWardByID(int id)
    {
        try
        {
            return await golManager.GetWardById(id);
        }
        catch (GolManagerException e)
        {
            return StatusCode(e.StatusCode, e.Message);
            // return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var wardInDB = await golManager.DeleteWard(id);
            return Ok($"ward with id : {id} and name :{wardInDB.Name} deleted successfully");
        }
        catch (GolManagerException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }


    }
    [HttpPost]
    public async Task<IActionResult> Post(Ward ward)
    {
        try
        {
            var a = await golManager.CreateWard(ward);

            return Created("wards", ward);
        }
        catch (GolManagerException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }


    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, Ward ward)
    {
 
        try
        {
            var result = await golManager.EditWard(id, ward);
            return Ok(new { Message = $"ward with id :{id} updated successfully", result });
        }
        catch (GolManagerException e)
        {
            return StatusCode(e.StatusCode, e.Message);
        }

    }
}