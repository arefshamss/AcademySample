using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Academy.Application.Extensions;

public static class Common
{
    public static string GetEnumName(this Enum myEnum)
    {
        var enumDisplayName = myEnum.GetType()
            .GetMember(myEnum.ToString())
            .FirstOrDefault();

        if (enumDisplayName != null)
            return enumDisplayName.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? enumDisplayName.Name;

        return "";
    }


    public static bool IsValidEmail(this string? value)
    {
        if (value == null)
        {
            return true;
        }

        if (!(value is string valueAsString))
        {
            return false;
        }

        // only return true if there is only 1 '@' character
        // and it is neither the first nor the last character
        int index = valueAsString.IndexOf('@');

        return
            index > 0 &&
            index != valueAsString.Length - 1 &&
            index == valueAsString.LastIndexOf('@');
    }


    public static string GetValueAsString(this Enum myEnum)
    {
        var enumDisplayName = myEnum.GetType()
            .GetMember(myEnum.ToString())
            .FirstOrDefault();

        return enumDisplayName != null ? enumDisplayName.Name : "";
    }


    public static string FixMobileNumber(this string mobile)
    {
        return mobile.Replace("098", "0")
            .Replace("+98", "0")
            .Replace("98", "0");
    }

    public static bool EqualText(this string? model, string? text)
    {
        if (string.IsNullOrEmpty(model) || string.IsNullOrEmpty(text))
            return false;

        return model.Trim()
            .ToLower()
            .Contains(text.Trim().ToLower());
    }

    /// <summary>
    /// generates random code based on the given length
    /// </summary>
    /// <param name="length">the length of the code</param>
    /// <returns>a random code based on the given length</returns>
    public static string GenerateRandomNumericCode(int length)
    {
        Random random = new Random();

        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string GetEnglishNumbers(this string s)
        => s.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4")
            .Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");


    public static string ConvertToEnglishNumber(this string persianStr)
    {
        Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
        {
            ['۰'] = '0',
            ['۱'] = '1',
            ['۲'] = '2',
            ['۳'] = '3',
            ['۴'] = '4',
            ['۵'] = '5',
            ['۶'] = '6',
            ['۷'] = '7',
            ['۸'] = '8',
            ['۹'] = '9'
        };
        if (persianStr == null) return persianStr;
        foreach (var item in persianStr.Where(item => !item.IsDigitEnglish()).Where(item => item != '.'))
        {
            try
            {
                persianStr = persianStr.Replace(item, LettersDictionary[item]);
            }
            catch
            {
                continue;
            }
        }

        return persianStr;
    }


    public static string ConvertEnglishToPersianString(this string enStr)
    {
        var lettersDictionary = new Dictionary<char, char>
        {
            ['a'] = 'ش',
            ['b'] = 'ذ',
            ['c'] = 'ز',
            ['C'] = 'ژ',
            ['d'] = 'ی',
            ['e'] = 'ث',
            ['f'] = 'ب',
            ['g'] = 'ل',
            ['h'] = 'ا',
            ['i'] = 'ه',
            ['j'] = 'ت' , 
            ['k'] = 'ن' , 
            ['l'] = 'م' , 
            ['m'] = 'ئ' , 
            ['n'] = 'د' , 
            ['o'] = 'خ' , 
            ['p'] = 'ح' , 
            ['q'] = 'ض' , 
            ['r'] = 'ق' , 
            ['s'] = 'س' , 
            ['t'] = 'ف' , 
            ['u'] = 'ع' , 
            ['v'] = 'ر' , 
            ['w'] = 'ث' , 
            ['x'] = 'ط' , 
            ['y'] = 'غ' , 
            ['z'] = 'ظ' , 
            [','] = 'و' , 
            [';'] = 'ک' , 
            ['\''] = 'گ' , 
            ['\\'] = 'پ' , 
        };
        
        if(string.IsNullOrEmpty(enStr)) return enStr;
        
        
        var result = new StringBuilder(enStr.Length);

        foreach (var c in enStr)
            result.Append(lettersDictionary.TryGetValue(c, out var persianChar) ? persianChar : c);
        
        return result.ToString();
    }
    
    public static string ConvertToPersianNumber(this string persianStr)
    {
        Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
        {
            ['0'] = '۰',
            ['1'] = '۱',
            ['2'] = '۲',
            ['3'] = '۳',
            ['4'] = '۴',
            ['5'] = '۵',
            ['6'] = '۶',
            ['7'] = '۷',
            ['8'] = '۸',
            ['9'] = '۹'
        };
        if (persianStr == null) return persianStr;
        foreach (var item in persianStr.Where(item => !item.IsDigitEnglish()).Where(item => item != '.'))
        {
            persianStr = persianStr.Replace(item, LettersDictionary[item]);
        }

        return persianStr;
    }

    public static bool IsDigitEnglish(this char str)
    {
        List<char> englishDigits = new List<char>()
        {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
        };
        return englishDigits.Contains(str);
    }


    public static bool IsNullOrEmptyOrWhiteSpace(this string? text)
    {
        return string.IsNullOrEmpty(text) && string.IsNullOrWhiteSpace(text);
    }

    public static string ToPrice(this double value)
    {
        return value.ToString("#,0");
    }

    public static string ToPrice(this int value) => value.ToString("#,0");

    public static string GenerateSlug(this string title)
    {
        return title.ToLower().Trim()
            .Replace(" ", "-")
            .Replace(".", "")
            .Replace("--", "-")
            .Replace("_", "-")
            .Replace("-_", "-")
            .Replace(".", "-")
            .Replace("+", "-")
            .Replace("  ", "-")
            .Replace("__", "-")
            .Replace("*", "")
            .Replace(@"\", "-")
            .Replace("/", "")
            .Replace(":", "")
            .Replace("--", "");
    } 

    public static string ToShow(this string? text) =>
        !text.IsNullOrEmptyOrWhiteSpace() ? text : "ثبت نشده";
    
    
    public static string ToJavaScriptDateTimeStandard(this DateTime dateTime) =>
        dateTime.ToString("O");

}