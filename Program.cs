using System.Text;
using gol_razor;
using gol_razor.GolManager;
using gol_razor.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//for swagger documetation
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("gol", new OpenApiInfo { Title = "Golestan API", Version = "v1" });
});

builder.Services.AddScoped<GolManager>();
builder.Services.AddDbContext<GolestanContext>
(option => option.UseSqlite(builder.Configuration.GetConnectionString("sqlite")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(
    options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
}

)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<GolestanContext>();
// به صورت نرمال از کوکی استفاده میکند ولی میتوان جی د تی هم اشافه کرد
builder.Services.AddAuthentication(options =>
{
    //اگر از اینها استفاده کنیم به صورت پیشفرض از  جی دابلیو تی استفاد می کند ه
    // options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };

});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "dentity/Account/Loging";
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Adjust expiration as needed
    options.LoginPath = "/Identity/Account/loging"; // Specify the login page route
    options.AccessDeniedPath = "/AccessDenied"; // Specify the access denied page route
    options.SlidingExpiration = true;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

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
app.UseAuthentication();
app.UseAuthorization();

//Minimal API Endpoints
app.MapControllers();
// make get endpoint that get a number and make it squer and return it 
app.MapGet("/square/{number}", (long number) => { return number * number; });
// .RequireAuthorization();

app.MapRazorPages();

app.UseSwagger();

app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/gol/swagger.json", "gol_razor V1");
});

using (var scope = app.Services.CreateScope())
{
    var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var UserManeger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    if (!await RoleManager.RoleExistsAsync("Admin"))
    {
        await RoleManager.CreateAsync(new IdentityRole("Admin"));
    }

    var email = "admin@admin.com";
    var password = "1234567";
    if (await UserManeger.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };
        var a = await UserManeger.CreateAsync(user, password);

        await UserManeger.AddToRoleAsync(user, "Admin");
    }
}
app.UseStatusCodePagesWithRedirects("/errors/{0}");
app.Run();
