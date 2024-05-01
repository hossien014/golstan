using gol_razor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gol_razor;

public class GolestanContext : IdentityDbContext<IdentityUser>
{
    public GolestanContext(DbContextOptions<GolestanContext> options) : base(options)
    {

    }

    // public DbSet<Role> Roles { get; set; }
    public DbSet<Ward> Wards { get; set; } //MAKE IT WARD   
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Shift> Shifts { get; set; }


}