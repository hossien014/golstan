using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gol_razor.Models;

public class Staff
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    // [Display(Name ="نام")]
    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; }

    public int WardId { get; set; }
    [ForeignKey("WardId")]
    public Ward Ward { get; set; }

    public string RoleId { get; set; }
    [ForeignKey("RoleId")]
    public IdentityRole Role { get; set; }
}

public class Shift
{
    public int Id { get; set; }

    public int StaffId { get; set; }
    public Staff Staff { get; set; }

    [DataType(DataType.Date)]
    // [Column(TypeName = "Date")]
    // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Required]
    public DateOnly Date { get; set; }

    public int WardId { get; set; }
    [ForeignKey("WardId")]
    public Ward Ward { get; set; }

    [Required]
    [Column(TypeName = "varchar(2)")]
    public string ShiftName { get; set; }
}
//USE ASP IDENTITY ROLE INSTEAD OF CUSTOM ROLE
// [Index(nameof(RoleName), IsUnique = true)]
// public class Role
// {
//     public int Id { get; set; }

//     [Required]
//     [Column(TypeName = "varchar(30)")]
//     public string RoleName { get; set; }
// }
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



public class ShiftData
{
    public string WardName { get; set; }
    public Dictionary<string, List<string>> Shifts { get; set; }
    public DateTime Date { get; set; }
}

public class Pdate
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int day { get; set; }
    public string MonthName{get;set;}
    public string DayName{get;set;}
    
    public DateTime GetDateTime()
    {
        var pc = new PersianCalendar();
        var date_ = new DateTime(Year, Month, day, pc);
        return date_;

    }

    public override string ToString()
    {
        var m = Month < 10 ? "0" + Month : Month.ToString();
        var d = day < 10 ? "0" + day : day.ToString();
        return $"{Year}-{m}-{d}";
    }
    public Pdate ToPdate(string date)
    {

        var d = date.Split("-");
        var pc = new PersianCalendar();
        var date_ = new Pdate
        {
            Year = int.Parse(d[0]),
            Month = int.Parse(d[1]),
            day = int.Parse(d[2])
        };
        return date_;
    }
    public Pdate ChangeDay(Pdate pdate, int _day)
    {
        pdate.day = _day;
        return pdate;
    }

    public string ChangeDay(string pdate, int _day)
    {
        var d = ToPdate(pdate);
        d.day = _day;
        return d.ToString();
    }
}