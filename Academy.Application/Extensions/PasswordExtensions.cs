using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Academy.Domain.Shared;

namespace Academy.Application.Extensions;

public static class PasswordExtensions
{
    public static string EncodePasswordMd5(string password) //Encrypt using MD5    
    {
        Byte[] originalBytes;
        Byte[] encodedBytes;
        MD5 md5;
        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
        md5 = new MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(password);
        encodedBytes = md5.ComputeHash(originalBytes);
        //Convert encoded bytes back to a 'readable' string    
        return BitConverter.ToString(encodedBytes);
    }
    
    public static string EncodePasswordSHA512(this string pass)
    {
        Byte[] originalBytes;
        Byte[] encodedBytes;
        var provider = new SHA512CryptoServiceProvider();

        originalBytes = ASCIIEncoding.Default.GetBytes(pass);
        encodedBytes = provider.ComputeHash(originalBytes);

        //Convert encoded bytes back to a 'readable' string   
        return Convert.ToBase64String(encodedBytes);
    }

    public static Result PasswordIsValid(this string Password, int minLength = 8, bool requiredUpperCase = true, bool requiredLowerCase = true)
    {
        if (string.IsNullOrWhiteSpace(Password))
        {
            return Result.Failure(ErrorMessages.PasswordRequiredError);
        }

        if (Password.Length < minLength)
        {
            return Result.Failure(string.Format(ErrorMessages.PasswordMinLengthError , minLength));
        }

        if (requiredUpperCase && !Regex.Match(Password, "^(?=.*[A-Z]).+$").Success)
        {
            return Result.Failure(ErrorMessages.PasswordRequiredUpperCaseError );
        }

        if (requiredLowerCase && !Regex.Match(Password, "^(?=.*[a-z]).+$").Success)
        {
            return Result.Failure(ErrorMessages.PasswordRequiredLowerCaseError);
        }

        return Result.Success();
    }
}