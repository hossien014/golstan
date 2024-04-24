using gol_razor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using gol_razor;
using Microsoft.AspNetCore.Mvc;
using gol_razor.GolManager;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

public class WardModel(GolestanContext context, GolManager golManager) : PageModel
{
    private readonly GolestanContext Context = context;
    public List<Ward> wards { get; set; }
    [TempData]
    public string massage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        wards = await Context.Wards.ToListAsync();
        return Page();
    }
    //delete ward
    public async Task<IActionResult> OnPostDelete(int id)
    {
        try
        {
            var ward = await golManager.DeleteWard(id);
            massage = $"war with id of {ward.Id} and name of {ward.Name} successfully deleted";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            massage = e.Message;
            return RedirectToPage();
        }

    }


    public class inputModel
    {
        public string Name { get; set; }
    }

    [BindProperty]
    public Ward W { get; set; }
    public async Task<IActionResult> OnPostPutAsync(int id)
    {
        try
        {
            W.Id = Convert.ToInt32(Request.Form["Id"]);
            await golManager.EditWard(id, W);
            massage = "بخش با موفقیت ویرایش شد";
            return RedirectToPage();
        }
        catch (Exception e)
        {
            massage = e.Message;
            return RedirectToPage();
        }

    }
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await golManager.CreateWard(W);
            massage  = "بخش با موفقیت ساخته شد";
            return RedirectToPage();

        }
        catch (Exception e)
        {
            massage = e.Message;
            return RedirectToPage();
        }
    }
}