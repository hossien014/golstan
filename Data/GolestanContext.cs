using gol_razor.Models;
using Microsoft.EntityFrameworkCore;

namespace gol_razor;

public class GolestanContext : DbContext
{
    public GolestanContext(DbContextOptions<GolestanContext> options) : base(options)
    {

    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Staff> Staffs { get; set; }
    public DbSet<Shift> Shifts { get; set; }


}