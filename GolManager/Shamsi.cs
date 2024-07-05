
using System.Globalization;
using gol_razor.Models;

namespace gol_razor._GolManager;
public class Shamsi
{

    public List<DateTime> GetDaysOftheMonth(int year, int month)
    {

        var pc = new PersianCalendar();
        var day_count = pc.GetDaysInMonth(year, month);
        List<DateTime> Days_in_Mounth = new List<DateTime>();
        for (int day = 1; day <= day_count; day++)
        {
            var _day = new DateTime(year, month, day, pc);
            Days_in_Mounth.Add(_day);
        }
        return Days_in_Mounth;
    }


    public string GetMonthName(int month)
    {
        var n = new Dictionary<int, string>
    {
        {1,"فروردین" },
        {2,"اردیبهشت" },
        {3,"خرداد" },
        {4,"تیر" },
        {5,"مرداد" },
        {6,"شهریور" },
        {7,"مهر" },
        {8,"آبان" },
        {9,"آذر" },
        {10,"دی" },
        {11,"بهمن" },
        {12,"اسفند" }
    };

        return n.ContainsKey(month) ? n[month] : string.Empty;
    }
    public DateTime ConvertToShamsi(DateTime date)
    {

        var pc = new PersianCalendar();
        int year = pc.GetYear(date);
        int month = pc.GetMonth(date);
        int day = pc.GetDayOfMonth(date);
        var w = new DateTime(year, month, day, pc);
        var a = new DateOnly(year, month, day);
        return w;

    }
    public string GetShamsiString(DateTime date)
    {
        var pc = new PersianCalendar();
        return $"{pc.GetYear(date)}-{pc.GetMonth(date)}-{pc.GetDayOfMonth(date)}";

    }
    public Pdate ConvertToShamsi_Pdate(DateTime date)
    {
        var pc = new PersianCalendar();
        int y = pc.GetYear(date);
        int m = pc.GetMonth(date);
        int d = pc.GetDayOfMonth(date);

        Pdate p = new Pdate { Year = y, Month = m, day = d };
        return p;
    }

    public List<DateTime> GetFirst_Last_month(DateTime gerigory_Date, out int days_in_Mounth)
    {


        var pc = new PersianCalendar();
        int year_s = pc.GetYear(gerigory_Date);
        int month_s = pc.GetMonth(gerigory_Date);

        DateTime firstDayInShamsi = pc.ToDateTime(year_s, month_s, 1, 1, 1, 1, 1);

        days_in_Mounth = pc.GetDaysInMonth(year_s, month_s);

        DateTime lastDayInShamsi = firstDayInShamsi.AddDays(days_in_Mounth - 1);
        return new List<DateTime> { firstDayInShamsi, lastDayInShamsi };

    }

}