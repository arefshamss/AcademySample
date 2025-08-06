namespace Academy.Domain.Shared;

public static class SmsMessages
{
    public const string AccountOtpMessage = "{0}: \n" + "کد احراز هویت شما : {1}";

    #region Course Delay Report

    public const string CourseDelayReportSms = "اطلاعیه تاخیر در انتشار دوره آموزش جامع {0}";

    public const string CourseDelayReportEmail =
        "{0} عزیز ، سلام !" +
        "\r\nاطلاعیه تاخیر در انتشار دوره {1}" +
        "\r\nمدرس گرامی ، این ایمیل به دلیل تاخیر در انتشار جلسات دوره {1} برای شما ارسال شده ." +
        "\r\nدر صورتی که این دوره تا پایان روز جاری بروز رسانی نشود ، این دوره از سایت حذف خواهد شد و شما دیگر نمیتوانید دوره در حال برگزاری داشته باشید\r\n" +
        "ممنون از همکاری شما";

    #endregion

    #region Forum Question Delay Report

    public const string ForumQuestionDelayReportSms = "اطلاعیه تاخیر در پاسخ به سوال {0} در {1}";

    public const string ForumQuestionDelayReportEmail = 
        "{0} عزیز ، سلام !" +
        "\r\nاطلاعیه تاخیر در پاسخ به سوال {1} در {2}" +
        "\r\nمدرس گرامی ، این ایمیل به دلیل تاخیر در پاسخ به سوال دوره {1} در {2} برای شما ارسال شده ." +
        "\r\nمشاهده سوال\r\nلطفا در اسرع وقت به پاسخ دادن به این سوال اقدام فرمایید\r\nممنون از همکاری شما";

    #endregion
}