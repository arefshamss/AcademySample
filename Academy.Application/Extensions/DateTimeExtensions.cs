using System.Globalization;
using Academy.Application.Statics;
using Humanizer;

namespace Academy.Application.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this string ts)
        {
            var splitedDateTime = ts.Split(" ");
            var spliteDate = splitedDateTime[0].Split('/');

            int h = 0, m = 0;
            if (splitedDateTime.Length > 1)
            {
                var spliteTime = splitedDateTime[1].Split(':');
                h = Convert.ToInt32(spliteTime[0]);
                m = Convert.ToInt32(spliteTime[1]);
            }

            try
            {
                int year = int.Parse(spliteDate[0]);
                int month = int.Parse(spliteDate[1]);
                int day = int.Parse(spliteDate[2]);
                DateTime currentDate = new DateTime(year, month, day, h, m, 0);
                return currentDate;
            }
            catch (Exception e)
            {
                return new DateTime();
            }
        }

        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            return persianCalendar.GetYear(value) + "/" +
                   persianCalendar.GetMonth(value).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(value).ToString("00");
        }
        
        public static string ToShamsi(this DateTime? value)
        {
            if (value == null)
                return string.Empty;

            PersianCalendar persianCalendar = new PersianCalendar();

            return persianCalendar.GetYear(value.Value) + "/" +
                   persianCalendar.GetMonth(value.Value).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(value.Value).ToString("00");
        }


        public static string ToShamsi(this DateTime value, ShamsiDateType dateType = ShamsiDateType.Default)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            var monthNames = new string[]
            {
                "فروردین", "اردیبهشت", "خرداد", "تیر",
                "مرداد", "شهریور", "مهر", "آبان",
                "آذر", "دی", "بهمن", "اسفند"
            };
            switch (dateType)
            {
                case ShamsiDateType.Default:
                    return persianCalendar.GetYear(value) + "/" +
                           persianCalendar.GetMonth(value).ToString("00") + "/" +
                           persianCalendar.GetDayOfMonth(value).ToString("00");
                case ShamsiDateType.ShamsiYear:
                    return persianCalendar.GetYear(value).ToString("00");
                case ShamsiDateType.NumericDayNameMonth:
                    return persianCalendar.GetDayOfMonth(value).ToString("00") + " " +
                           monthNames[persianCalendar.GetMonth(value) - 1];
                case ShamsiDateType.DayOfWeekName:
                    return persianCalendar.GetDayOfWeek(value).GetPersianNameOfDay();
                default:
                    return persianCalendar.GetYear(value) + "/" +
                           persianCalendar.GetMonth(value).ToString("00") + "/" +
                           persianCalendar.GetDayOfMonth(value).ToString("00");
            }
        }

        public static string ToShamsiDateTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " - " + pc.GetSecond(value).ToString("00") + " : " +
                   pc.GetMinute(value).ToString("00") + " : " + pc.GetHour(value).ToString("00");
        }

        public static string ToShamsi(this DateOnly value)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            DateTime dateTime = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);

            return persianCalendar.GetYear(dateTime) + "/" +
                   persianCalendar.GetMonth(dateTime).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(dateTime).ToString("00");
        }

        private static string GetPersianNameOfDay(this DayOfWeek day)
        {
            var result = "";
            switch (day)
            {
                case DayOfWeek.Friday:
                    result = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    result = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    result = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    result = "یکشنبه";
                    break;
                case DayOfWeek.Thursday:
                    result = "پنجشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    result = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    result = "چهارشنبه";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }

        public static DateTime ToMiladi(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new PersianCalendar());
        }

        public static DateTime GetDateNow()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }

        public static string GetHourAndMinutesFormat(this DateTime time)
        {
            return time.ToString("HH:mm");
        }

        public static DateTime? ParseUserDateToUTC(this string dateTime)
        {
            var userCulture = Thread.CurrentThread.CurrentCulture;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            var successfullyParsed = DateTime.TryParse(dateTime, out DateTime parsedDateTime);

            if (successfullyParsed)
            {
                return TimeZoneInfo.ConvertTimeToUtc(parsedDateTime, userTimeZone);
            }
            else
            {
                return null;
            }
        }


        public static string ToStringShamsiDate(this DateTime dt)
        {
            PersianCalendar PC = new PersianCalendar();
            int intYear = PC.GetYear(dt);
            int intMonth = PC.GetMonth(dt);
            int intDayOfMonth = PC.GetDayOfMonth(dt);
            DayOfWeek enDayOfWeek = PC.GetDayOfWeek(dt);
            string strMonthName = "";
            string strDayName = "";

            switch (intMonth)
            {
                case 1:
                    strMonthName = "فروردین";
                    break;
                case 2:
                    strMonthName = "اردیبهشت";
                    break;
                case 3:
                    strMonthName = "خرداد";
                    break;
                case 4:
                    strMonthName = "تیر";
                    break;
                case 5:
                    strMonthName = "مرداد";
                    break;
                case 6:
                    strMonthName = "شهریور";
                    break;
                case 7:
                    strMonthName = "مهر";
                    break;
                case 8:
                    strMonthName = "آبان";
                    break;
                case 9:
                    strMonthName = "آذر";
                    break;
                case 10:
                    strMonthName = "دی";
                    break;
                case 11:
                    strMonthName = "بهمن";
                    break;
                case 12:
                    strMonthName = "اسفند";
                    break;
                default:
                    strMonthName = "";
                    break;
            }

            switch (enDayOfWeek)
            {
                case DayOfWeek.Friday:
                    strDayName = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    strDayName = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    strDayName = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    strDayName = "یکشنبه";
                    break;
                case DayOfWeek.Thursday:
                    strDayName = "پنجشنبه";
                    break;
                case DayOfWeek.Tuesday:
                    strDayName = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    strDayName = "چهارشنبه";
                    break;
                default:
                    strDayName = "";
                    break;
            }

            return (string.Format("{0} {1} {2} {3}", strDayName, intDayOfMonth, strMonthName, intYear));
        }

        public static string ToShortTime(this TimeSpan ts)
        {
            return ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00");
        }

        public static DateTime ToMiladiDateTime(this string ts)
        {
            string[] dateSplit = ts.Split("/");
            int date = Convert.ToInt32(dateSplit[0].GetEnglishNumbers());
            int month = Convert.ToInt32(dateSplit[1].GetEnglishNumbers());
            int day = Convert.ToInt32(dateSplit[2].GetEnglishNumbers());
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = new DateTime(date, month, day, pc);
            return dt;
        }

        public static DateOnly ToMiladiDateOnly(this string persianDate)
        {
            string[] dateSplit = persianDate.Split("/");
            int date = Convert.ToInt32(dateSplit[0].GetEnglishNumbers());
            int month = Convert.ToInt32(dateSplit[1].GetEnglishNumbers());
            int day = Convert.ToInt32(dateSplit[2].GetEnglishNumbers());
            PersianCalendar pc = new PersianCalendar();
            DateOnly dt = new DateOnly(date, month, day, pc);
            return dt;
        }

        public static string ToPersianHourMinute(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            DateTime persianTime = persianCalendar.ToDateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
            return persianTime.ToString("HH:mm");
        }

        public static string ToUserShortDateTime(this DateTime dateUtc)
        {
            var userCulture = Thread.CurrentThread.CurrentCulture;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, userTimeZone)
                .ToString("yyyy/MM/dd HH:mm", userCulture.DateTimeFormat);
        }


        public static string ToUserDateTime(this DateTime dateTime)
        {
            var userCulture = CultureInfo.CurrentCulture;
            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, userTimeZone)
                    .ToString("yyyy/MM/dd - HH:mm", userCulture.DateTimeFormat);
            }
            catch
            {
                return TimeZoneInfo.ConvertTime(dateTime, userTimeZone)
                    .ToString("yyyy/MM/dd - HH:mm", userCulture.DateTimeFormat);
            }
        }

        public static string ToUserDate(this DateTime? dateTime)
        {
            if (dateTime is null) return string.Empty;
            var userCulture = CultureInfo.CurrentCulture;
            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime.Value, userTimeZone)
                    .ToString("yyyy/MM/dd", userCulture.DateTimeFormat);
            }
            catch
            {
                return TimeZoneInfo.ConvertTime(dateTime.Value, userTimeZone)
                    .ToString("yyyy/MM/dd", userCulture.DateTimeFormat);
            }
        }

        public static string ToUserDate(this DateTime dateTime)
        {
            var userCulture = CultureInfo.CurrentCulture;
            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, userTimeZone)
                    .ToString("yyyy/MM/dd", userCulture.DateTimeFormat);
            }
            catch
            {
                return TimeZoneInfo.ConvertTime(dateTime, userTimeZone)
                    .ToString("yyyy/MM/dd", userCulture.DateTimeFormat);
            }
        }


        public static string ToUserLongDateTime(this DateTime dateUtc)
        {
            var userCulture = Thread.CurrentThread.CurrentCulture;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, userTimeZone)
                .ToString("yyyy/MM/dd HH:mm:ss", userCulture.DateTimeFormat);
        }

        public static string ToUserLongDateTimeWithoutSecond(this DateTime dateUtc)
        {
            var userCulture = Thread.CurrentThread.CurrentCulture;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, userTimeZone)
                .ToString("yyyy/MM/dd HH:mm", userCulture.DateTimeFormat);
        }

        public static string ToUserTimeHourMinute(this DateTime dateUtc)
        {
            var userCulture = Thread.CurrentThread.CurrentCulture;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, userTimeZone)
                .ToString("HH:mm", userCulture.DateTimeFormat);
        }

        public static string ToUserShortDateTime(this DateTime? dateUtc)
        {
            if (dateUtc is null) return string.Empty;

            var userCulture = Thread.CurrentThread.CurrentCulture;

            var userTimeZone =
                TimeZoneInfo.FindSystemTimeZoneById(
                    SystemTimeZones.GetTimeZoneStandardNameByCultureName(userCulture.Name));

            return TimeZoneInfo.ConvertTimeFromUtc(dateUtc.Value, userTimeZone)
                .ToString("yyyy/MM/dd HH:mm", userCulture.DateTimeFormat);
        }

        public static int ToShamsiIntDate(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            string year = pc.GetYear(date).ToString("0000");
            string month = pc.GetMonth(date).ToString("00");
            string day = pc.GetDayOfMonth(date).ToString("00");
            return Convert.ToInt32(year + month + day);
        }

        public static int GetDifferenceInDays(this DateTime startDate, DateTime endDate)
            => (startDate - endDate).Days;

        public static int GetDifferenceInDays(this DateOnly startDate, DateOnly endDate)
            => (startDate.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromMicroseconds(1))) -
                endDate.ToDateTime(TimeOnly.FromTimeSpan(TimeSpan.FromMicroseconds(1)))).Days;

        public static DateTime ToDateTimeFromJavaScriptDateTime(this string dateTime)
        {
            if (DateTime.TryParse(dateTime, null, DateTimeStyles.RoundtripKind, out DateTime finallyDateTime)) 
                return finallyDateTime;
            return DateTime.MinValue;
        }
        
        public static string ToHumanize(this DateTime dateTime) =>
            dateTime.Humanize(false  , DateTime.Now , CultureInfo.GetCultureInfo("fa-IR"));
    }


    public enum ShamsiDateType
    {
        Default,
        ShamsiYear,
        NumericDayNameMonth,
        DayOfWeekName
    }
}