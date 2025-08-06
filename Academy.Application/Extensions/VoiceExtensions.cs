using Microsoft.AspNetCore.Http;

namespace Academy.Application.Extensions;

public static class VoiceExtensions
{
    public static bool IsVoice(this IFormFile postedFile)
    {

        if (Path.GetExtension(postedFile.FileName)?.ToLower() != ".mp3" &&
            Path.GetExtension(postedFile.FileName)?.ToLower() != ".wav")
        {
            return false;
        }
        if (postedFile.ContentType.ToLower() != "audio/wav")
        {
            return false;
        }
        return true;
    }


    public static bool HasLength(this IFormFile postedFile,int length)
    {
        if (postedFile.Length > length)
        {
            return false;
        }

        return true;
    }
}