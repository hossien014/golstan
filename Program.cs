using gol_razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("gol", new OpenApiInfo { Title = "Golestan API", Version = "v1" });
});

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

app.UseSwagger();

app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/gol/swagger.json", "gol_razor V1");
});
app.Run();
