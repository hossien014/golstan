using gol_razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson();


var a = builder.Configuration.GetConnectionString("sqlite");
builder.Services.AddDbContext<GolestanContext>
(option => option.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Minimal API Endpoints
app.MapControllers();

app.MapRazorPages();

app.Run();
