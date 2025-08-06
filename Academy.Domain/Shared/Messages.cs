namespace Academy.Domain.Shared;

public static class SuccessMessages
{
    public const string SuccessfullyDone = "با موفقیت انجام شد !";
    
    #region Common

    public const string UpdateSuccessfullyDone = "ویرایش با موفقیت انجام شد.";

    public const string InsertSuccessfullyDone = "افزودن با موفقیت انجام شد.";

    public const string SavedChangesSuccessfully = "تغییرات با موفقیت اعمال شد";

    public const string DeleteSuccess = "عملیات حذف با موفقیت انجام شد";

    public const string RecoverSuccess = "عملیات بازگردانی با موفقیت انجام شد";

    #endregion
    
    #region User

    public const string LoginSuccessfullyDone = "ورود به حساب کاربری با موفقیت انجام شد";
    public const string RegisterSuccessfullyDone = "ثبت نام شما با موفقیت انجام شد،";
    public const string OtpCodeSentSuccessfully = "ارسال کد تایید با موفقیت انجام شد";
    public const string LogoutSuccessfullyDone = "با موفقیت از حساب کاربری خود خارج شدید";
    public const string UpdateAvatarSuccessfullyDone = "تصویر پروفایل با موفقیت تغییر کرد";  
    public const string DeleteAvatarSuccessfullyDone = "تصویر پروفایل با موفقیت حذف شد";  
    public const string UserIsActiveSuccessfully = "حساب کاربری شما با موفقیت فعال شد";
    public const string OtpCodeSentSuccessfullyToEmail = "کد تأیید به ایمیل شما ارسال شد";
    public const string OtpCodeSentSuccessfullyToMobile = "کد تأیید به شماره همراه شما ارسال شد";
    public const string PasswordChangedSuccessfully = "کلمه عبور با موفقیت تغییر کرد.";


    #endregion

    #region Ticket

    public const string TicketInsertSuccessfullyDone = "ارسال تیکت با موفقیت انجام شد";

    #endregion
}


public static class ErrorMessages
{

    public const string MinRangeError = "{0} باید بزرگتر از {1} باشد";

    public const string MaxRangeError = "{0} باید کوچکتر از {2} باشد";

    public const string RangeError = "{0} باید بین عدد {1} و عدد {2} باشد";

    public const string MinRangeErrorForPrice = "{0} باید بزگتر از {1} ریال باشد";

    public const string RangeErrorForPrice = "{0} باید بین {1} ریال و {2} ریال باشد";

    public const string MaxRangeErrorForPrice = "{0} باید کوچکتر از {2} ریال باشد";

    public const string MaxLengthError = "تعداد کاراکتر مجاز {1} عدد می باشد";

    public const string NullValue = "لطفا فیلدهای فرم را به درستی کامل نمایید";

    public const string NotFoundError = "موردی یافت نشد";
    
    public const string ItemNotFoundError = "{0} یافت نشد";

    public const string OperationFailedError = "عملیات شکست خورد";

    public const string MinLengthError = "کاراکترهای {0} نمیتواند کمتر از {1} باشد";

    public const string RequiredError = "لطفا {0} را وارد کنید";

    public const string RegexIncorrectFormat = "{0} را با فرمت درست وارد کنید";

    public const string DuplicatedError = "{0} قبلا انتخاب شده است";

    public const string DuplicatedNewsletterEmailError = "شما قبلا در عضو خبرنامه شده اید";

    public const string DuplicatedDeletedError = "{0}  در لیست حذف شده ها تکراری است";

    public const string FileSizeError = "سایز فایل نمیتواند بیشتر از {0} مگابایت باشد";

    public const string PasswordRequiredUpperCaseError = "رمز عبور نیازمند حروف بزرگ می باشد";

    public const string PasswordRequiredLowerCaseError = "رمز عبور نیازمند حروف کوچک می باشد";

    public const string PasswordRequiredError = "لطفا کلمه عبور خود را وارد نمایید";

    public const string PasswordMinLengthError = "کلمه عبور نمی تواند کمتر از {0} کاراکتر باشد";

    public const string BadRequestError = "درخواست شما نامعتبر است";

    public const string UserBannedError = "حساب کاربری شما به علت {0} مسدود شده است";

    public const string NullConfirmCode = "کد تایید را وارد کنید";

    public const string AmountNotValidError = "مبلغ وارد شده صحیح نمی باشد";
    
    public const string NotEnoughBalance = "موجودی کیف پول کافی نمی باشد";

    public const string AlreadyExistError = "{0} از قبل موجود است";

    public const string ReCaptchaValidateError = "کپتچا تایید نشد";

    public const string BannerInUseError = "این بنر قبلا استفاده شده است";

    public const string SelectAvatarError = "لطفا آواتار را انتخاب کنید";

    public const string NotAuthenticatedError = "برای انجام عملیات، ابتدا وارد حساب کاربری خود شوید";

    public const string DuplicateRequest = "شما قبلا درخواست خود را ثبت کرده اید";

    public const string ModelNotValidError = "اطلاعات وارد شده صحیح نمی باشد";

    public const string SomethingWentWrong = "مشکلی پیش آمده است";
    
    public const string SelectError = "لطفا {0} را انتخاب کنید";

    public const string InvalidShabaNumberError = "شماره شبا وارد شده صحیح نمی باشد";

    public const string InvalidInformationError = "اطلاعات وارد شده صحیح نمی باشد";

    public const string EmailSendError = "ارسال ایمیل با شکست مواجه شد";

    public const string MobileNotValidError = "شماره موبایل وارد شده صحیح نمی باشد";
        
    public const string EmailNotValidError = "ایمیل وارد شده صحیح نمی باشد";

    public const string StartDateBiggerThanEndDateError = "تاریخ شروع نمی تواند بزرگ تر از تاریخ پایان باشد";

    public const string DateConvertError = "فرمت تاریخ وارد شده صحیح نمی باشد";
    
    public const string NotValid = "{0} وارد شده معتبر نمی باشد";
    
    #region User

    public const string ConflictError = "{0} وارد شده تکراری می باشد";

    public const string UserNotFoundError = "کاربری با مشخصات وارد شده یافت نشد";

    public const string UserNotActiveError = "حساب کاربری شما غیرفعال می باشد";

    public const string ExpireConfirmCodeError = "کد تایید وارد شده منقضی شده است";

    public const string InvalidConfirmationCode = "کد تایید وارد شده نامعتبر می باشد";

    public const string PasswordNotCorrect = "رمز عبور وارد شده صحیح نمی باشد";
    
    public const string CurrentPasswordNotCorrect = "رمز عبور فعلی وارد شده صحیح نمی باشد";

    public const string PasswordCompareError = "تکرار رمز عبور با رمز عبور وارد شده مطابقت ندارد";

    public const string UserIsActive = "حساب کاربری شما فعال می باشد";
    
    public const string RequiredUserFullName = "لطفا قبل از {0}، اطلاعات پروفایل خود را تکمیل نمایید.";

    public const string ActiveCodeExpireDateTime = "برای ارسال مجدد کد تایید باید از ارسال قبلی حداقل 90 ثانیه گذشته باشد";
    
    public const string ConflictActiveUserError = "این شماره موبایل بصورت فعال در لیست کاربران موجود است و امکان {0} وجود ندارد.";
    
    public const string ConflictUserChangeError = "کاربر با این مشخصات در لیست کاربران موجود است و امکان {0} وجود ندارد.";
    
    public const string RequiredMobileOrEmail = "وارد کردن ایمیل و یا شماره تلفن الزامیست.";
    
    public const string NotValidMobileOrEmail = "لطفاً ایمیل یا شماره همراه معتبری وارد کنید.";
    
    public const string NotValidMobile = "لطفاً شماره همراه معتبری وارد کنید.";
    
    public const string NotValidEmail = "لطفاً ایمیل معتبری وارد کنید.";
    
    public const string NotValidOtpCode = "کد فعال‌سازی نامعتبر یا منقضی شده است.";
    
    public const string UserDeletedError = "کاربر حذف شده است.";
    
    #region UserPosition

    public const string PriorityExist = "این اولویت نمایش برای کاربر دیگر ثبت شده است";

    public const string AlreadyAdded = "این کاربر از قبل سمتی براش مشخص شده است";

    #endregion

    #region CourseCategory

    public const string ConflictPriorityError = "این اولویت نمایش موجود است و امکان {0} وجود ندارد.";

    #endregion

    #endregion
    
    #region Google Login

    public const string GoogleAuthBadRequestError = "مشکلی در فرایند ورود با حساب کاربری گوگل پیش آمده است";

    #endregion
    
    #region Sms

    public const string SmsDidNotSendError = "ارسال پیامک با خطا مواجه شد";
    
    public const string EmailDidNotSendError = "ارسال ایمیل با خطا مواجه شد";

    #endregion
    
    #region Files

    public const string FileFormatError = "فرمت فایل آپلود شده معتبر نمی باشد";
    
    public const string FileArchiveFormatError = "لطفا فایل با پسوند zip یا rar بارگزاری نمایید.";

    public const string ImageIsRequired = "وارد کردن تصویر الزامی میباشد";

    public const string FileNotFound = "فایل یافت نشد";

    #endregion
    
    #region Ticket

    public const string TicketIsClosed = "این تیکت بسته شده است";

    #endregion
}