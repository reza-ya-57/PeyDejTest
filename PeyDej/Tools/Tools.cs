using System.Globalization;

namespace PeyDej.Tools;

public class PeyDejTools
{
    public static string GetCurPersianDate()
    {
        var persianCalendar = new PersianCalendar();
        return persianCalendar.GetYear(DateTime.Now) + "/" +
               persianCalendar.GetMonth(DateTime.Now).ToString("00") + "/" +
               persianCalendar.GetDayOfMonth(DateTime.Now).ToString("00");
    }

    public static DateTime PersianStringToDateTime(string? persianDateStr)
    {
        if (persianDateStr == null)
        {
            return DateTime.Now;
        }

        var persianCulture = new CultureInfo("fa-IR");
        var persianDate = DateTime.ParseExact(persianDateStr, "yyyy/MM/dd", persianCulture);
        var gregorianDate = persianDate.ToUniversalTime();
        return gregorianDate.Date;
    }
}