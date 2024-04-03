To add a minimal API to your ASP.NET Razor Pages application, you can follow these steps:

1. **Add the `Microsoft.AspNetCore.Mvc.NewtonsoftJson` package**: This is required to enable JSON serialization using Newtonsoft.Json.

    ```bash
    dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
    ```

2. **Update `Program.cs` to include minimal API endpoints**: Modify your `Program.cs` file to include minimal API endpoints alongside Razor Pages.

    ```csharp
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorPages();

    // Add NewtonsoftJson
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

    // Minimal API endpoints
    app.MapControllers();

    app.MapRazorPages();

    app.Run();
    ```

3. **Create Minimal API Controllers**: Create controllers that derive from `ControllerBase` instead of `Controller` and define your API endpoints using attributes like `[HttpGet]`, `[HttpPost]`, etc.

    For example:

    ```csharp
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new string[] { "value1", "value2" });
        }

        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            // Do something with the posted value
            return Ok();
        }
    }
    ```

    You can create more controllers and endpoints as per your requirements.

With these steps, you've integrated minimal APIs alongside your Razor Pages application. You can define lightweight APIs using minimal API syntax, and they will be accessible alongside your Razor Pages endpoints.

