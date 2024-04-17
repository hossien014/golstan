using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace gol_razor.Models;

public class Staff
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; }

    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public Ward Department { get; set; }

    public int RoleId { get; set; }
    [ForeignKey("RoleId")]
    public Role Role { get; set; }
}

public class Shift
{
    public int Id { get; set; }

    public int StaffId { get; set; }
    public Staff Staff { get; set; }

    [DataType(DataType.Date)]
    [Required]
    public DateTime Date { get; set; }

    public int DepartmentId { get; set; }
    [ForeignKey("DepartmentId")]
    public Ward Department { get; set; }

    [Required]
    [Column(TypeName = "varchar(2)")]
    public string ShiftName { get; set; }
}

[Index(nameof(RoleName), IsUnique = true)]
public class Role
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(30)")]
    public string RoleName { get; set; }
}
[Index(nameof(Name), IsUnique = true)]
public class Ward
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(30)")]
    public string Name { get; set; }

    // Navigation property
    public ICollection<Staff>? Staffs { get; set; }
}
