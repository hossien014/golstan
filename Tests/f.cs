using Xunit;
using gol_razor._GolManager;
using gol_razor.Models;
using System.Security.Cryptography.X509Certificates;

public class FTest
{
    [Theory]
    [InlineData(DayOfWeek.Monday, "دوشنبه")]
    [InlineData(DayOfWeek.Tuesday, "سه شنبه")]
    [InlineData(DayOfWeek.Wednesday, "چهارشنبه")]
    [InlineData(DayOfWeek.Thursday, "پنجشنبه")]
    [InlineData(DayOfWeek.Friday, "جمعه")]
    [InlineData(DayOfWeek.Saturday, "شنبه")]
    [InlineData(DayOfWeek.Sunday, "یکشنبه")]
    public void EnToFarsiDay_Success(DayOfWeek day, string expected)
    {
        var shamsi = new Shamsi();
        var actual = shamsi.EnToFarsDay(day);
        Assert.Equal(expected, actual);

    }
    [Fact]
    public void ConvertToShamsi_Pdate_Success()
    {
        var shamsi = new Shamsi();

        var gerigory = new DateTime(2024, 7, 21);
        var exoected = new Pdate
        {
            Year = 1403,
            Month = 4,
            day = 31,
            DayName = "یکشنبه",
            MonthName = "تیر",
        };
        var actual = shamsi.ConvertToShamsi_Pdate(gerigory);

        Assert.Equal(exoected.Year, actual.Year);
        Assert.Equal(exoected.Month, actual.Month);
        Assert.Equal(exoected.day, actual.day);
    }
    /// <summary>
    ///  this test is for checking the first and last day of the month and its sample the 
    ///  1403/4 month on persian calendar 
    /// </summary>
    [Fact]
    public void GetFirst_Last_month_Success()
    {
        var shamsi = new Shamsi();
        DateTime FakeNow = new DateTime(2024, 6, 28);

        int daysCountExpected = 31;

        List<DateTime> firstAndLastExpected = new List<DateTime>
        {
         new DateTime(2024, 6, 21), // first day 
         new DateTime(2024, 7, 21) // last day 
         };

        List<DateTime> firstAndLastActual = shamsi.GetFirst_Last_month(FakeNow, out int days_in_MounthActual);

        Assert.Equal(firstAndLastExpected, firstAndLastActual);
    }
}