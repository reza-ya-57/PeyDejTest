using System.Collections;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    public static SelectList Week()
    {
        var dictSector = new List<SectorItem>();
        dictSector.Add(new SectorItem(5, "شنبه"));
        dictSector.Add(new SectorItem(6, "یک شنبه"));
        dictSector.Add(new SectorItem(0, "دو شنبه"));
        dictSector.Add(new SectorItem(1, "سه شنبه"));
        dictSector.Add(new SectorItem(2, "جهار شنبه"));
        dictSector.Add(new SectorItem(3, "پنج شنبه"));
        dictSector.Add(new SectorItem(4, "جمعه"));
        return new SelectList(dictSector.ToList(), "Id", "Name");
    }
}