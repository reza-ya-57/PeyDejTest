using DNTPersianUtils.Core;

namespace Ccms.Common.Utilities
{

    using System;
    using System.Globalization;

    public static class DateTimeConvertor
    {
        public static string ToMiladiString(this DateTime? date)
        {
            if (date is null) return null;
            PersianCalendar pc = new();
            return $"{pc.GetYear(date.Value)}/{pc.GetMonth(date.Value)}/{pc.GetDayOfMonth(date.Value)} {pc.GetHour(date.Value)}:{pc.GetMinute(date.Value)}:{pc.GetSecond(date.Value)}";
        }
        public static DateTime? ToMiladi(this DateTime? date)
        {
            if (date is null) return null;
            return new PersianCalendar()
                .ToDateTime(
                    date.Value.Year,
                    date.Value.Month,
                    date.Value.Day,
                    date.Value.Hour,
                    date.Value.Minute,
                    date.Value.Second,
                    date.Value.Millisecond);

        }

        public static DateTime? ToMiladi(this string date, int hour = 0, int minute = 0, int second = 0, int millisecond = 1)
        {
            string[] datetime = date.Split('/');
            return new PersianCalendar()
                .ToDateTime(
                int.Parse(datetime[0]),
                int.Parse(datetime[1]),
                int.Parse(datetime[2]),
                hour,
                minute,
                second,
                millisecond);
        }
        private static Tuple<bool, int> ToNumber(this string data)
        {
            bool result = int.TryParse(data, NumberStyles.Number, CultureInfo.InvariantCulture, out var number);
            return new Tuple<bool, int>(result, number);
        }
        private static int? GetDay(string part)
        {
            var day = part.ToNumber();
            if (!day.Item1) return null;
            var pDay = day.Item2;
            if (pDay == 0 || pDay > 31) return null;
            return pDay;
        }

        private static int? GetMonth(string part)
        {
            var month = part.ToNumber();
            if (!month.Item1) return null;
            var pMonth = month.Item2;
            if (pMonth == 0 || pMonth > 12) return null;
            return pMonth;
        }

        private static int? GetYear(string part, int beginningOfCentury)
        {
            var year = part.ToNumber();
            if (!year.Item1) return null;
            var pYear = year.Item2;
            if (part.Length == 2) pYear += beginningOfCentury;
            return pYear;
        }
        /// <summary>
        /// تعیین اعتبار تاریخ شمسی
        /// </summary>
        /// <param name="persianYear">سال شمسی</param>
        /// <param name="persianMonth">ماه شمسی</param>
        /// <param name="persianDay">روز شمسی</param>
        public static bool IsValidPersianDate(this int persianYear, int persianMonth, int persianDay)
        {
            if (persianDay > 31 || persianDay <= 0)
            {
                return false;
            }

            if (persianMonth > 12 || persianMonth <= 0)
            {
                return false;
            }

            if (persianMonth <= 6 && persianDay > 31)
            {
                return false;
            }

            if (persianMonth >= 7 && persianDay > 30)
            {
                return false;
            }

            if (persianMonth == 12)
            {
                var persianCalendar = new PersianCalendar();
                var isLeapYear = persianCalendar.IsLeapYear(persianYear);

                if (isLeapYear && persianDay > 30)
                {
                    return false;
                }

                if (!isLeapYear && persianDay > 29)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// تعیین اعتبار تاریخ و زمان رشته‌ای شمسی
        /// با قالب‌های پشتیبانی شده‌ی ۹۰/۸/۱۴ , 1395/11/3 17:30 , ۱۳۹۰/۸/۱۴ , ۹۰-۸-۱۴ , ۱۳۹۰-۸-۱۴
        /// </summary>
        /// <param name="persianDateTime">تاریخ و زمان شمسی</param>
        /// <param name="throwOnException"></param>
        public static bool IsValidPersianDateTime(this string persianDateTime, bool throwOnException = false)
        {
            try
            {
                var dt = persianDateTime.ToGregorianDateTime();
                return dt.HasValue;
            }
            catch
            {
                if (throwOnException)
                {
                    throw;
                }
                return false;
            }
        }


        public static DateTime? ToGregorianDateTime(this string persianDateTime, bool convertToUtc = false, int beginningOfCentury = 1300)
        {
            if (persianDateTime is null)
            {
                return null;
            }

            persianDateTime = persianDateTime.Trim().ToEnglishNumbers();
            string[] splittedDateTime;
            if (persianDateTime.Contains('T'))
            {
                splittedDateTime = persianDateTime.Split(new[] { 'T' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                splittedDateTime = persianDateTime.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            var rawTime = Array.Find(splittedDateTime, s => s.Contains(':', StringComparison.OrdinalIgnoreCase));
            var rawDate = Array.Find(splittedDateTime, s => !s.Contains(':', StringComparison.OrdinalIgnoreCase));

            var splittedDate = rawDate?.Split('/', '\\', ',', '؍', '.', '-');
            if (splittedDate?.Length != 3)
            {
                return null;
            }

            var day = GetDay(splittedDate[2]);
            if (!day.HasValue)
            {
                return null;
            }

            var month = GetMonth(splittedDate[1]);
            if (!month.HasValue)
            {
                return null;
            }

            var year = GetYear(splittedDate[0], beginningOfCentury);
            if (!year.HasValue)
            {
                return null;
            }

            if (!IsValidPersianDate(year.Value, month.Value, day.Value))
            {
                return null;
            }

            var hour = 0;
            var minute = 0;
            var second = 0;

            if (!string.IsNullOrWhiteSpace(rawTime))
            {
                var splittedTime = rawTime.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                hour = int.Parse(splittedTime[0], CultureInfo.InvariantCulture);
                minute = int.Parse(splittedTime[1], CultureInfo.InvariantCulture);
                if (splittedTime.Length > 2)
                {
                    var lastPart = splittedTime[2].Trim();
                    var formatInfo = PersianCulture.Instance.DateTimeFormat;
                    if (lastPart.Equals(formatInfo.PMDesignator, StringComparison.OrdinalIgnoreCase))
                    {
                        if (hour < 12)
                        {
                            hour += 12;
                        }
                    }
                    else
                    {
                        if (!int.TryParse(lastPart, NumberStyles.Number, CultureInfo.InvariantCulture, out second))
                        {
                            second = 0;
                        }
                    }
                }
            }

            var persianCalendar = new PersianCalendar();
            var dateTime = persianCalendar.ToDateTime(year.Value, month.Value, day.Value, hour, minute, second, 0);
            if (convertToUtc)
            {
                dateTime = dateTime.ToUniversalTime();
            }
            return dateTime;
        }
        public static string ToShamsi(this DateTime date)
        {
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date)}/{pc.GetDayOfMonth(date)} {pc.GetHour(date)}:{pc.GetMinute(date)}:{pc.GetSecond(date)}.{pc.GetMilliseconds(date)}";
        }
        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        /// <exception cref="1400/1/1">فرمت خروجی</exception>
        public static string ToShamsi(this DateTime? date)
        {
            if (date is null) return null;
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date.Value)}/{pc.GetMonth(date.Value)}/{pc.GetDayOfMonth(date.Value)}";
        }
        /// <summary>
        ///     Converts Persian and Arabic digits of a given string to their equivalent English digits.
        /// </summary>
        /// <param name="data">Persian number</param>
        /// <returns></returns>
        public static string ToEnglishNumbers(this string? data)
        {
            if (data is null)
            {
                return string.Empty;
            }

            var dataChars = data.ToCharArray();
            for (var i = 0; i < dataChars.Length; i++)
            {
                switch (dataChars[i])
                {
                    case '\u06F0':
                    case '\u0660':
                        dataChars[i] = '0';
                        break;

                    case '\u06F1':
                    case '\u0661':
                        dataChars[i] = '1';
                        break;

                    case '\u06F2':
                    case '\u0662':
                        dataChars[i] = '2';
                        break;

                    case '\u06F3':
                    case '\u0663':
                        dataChars[i] = '3';
                        break;

                    case '\u06F4':
                    case '\u0664':
                        dataChars[i] = '4';
                        break;

                    case '\u06F5':
                    case '\u0665':
                        dataChars[i] = '5';
                        break;

                    case '\u06F6':
                    case '\u0666':
                        dataChars[i] = '6';
                        break;

                    case '\u06F7':
                    case '\u0667':
                        dataChars[i] = '7';
                        break;

                    case '\u06F8':
                    case '\u0668':
                        dataChars[i] = '8';
                        break;

                    case '\u06F9':
                    case '\u0669':
                        dataChars[i] = '9';
                        break;
                }
            }

            return new string(dataChars);
        }
    }
}
