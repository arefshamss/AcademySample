using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Academy.Application.Extensions;

public static class VideoExtensions
{
    public const int VideoMinimumBytes = 512;

    public static bool IsVideo(this IFormFile postedFile)
    {
        //-------------------------------------------
        //  Check the video mime types
        //-------------------------------------------
        if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".avi"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".wmv"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".mpeg-2"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".mov")
        {
            return false;
        }

        //-------------------------------------------
        //  Check the video extension
        //-------------------------------------------
        if (Path.GetExtension(postedFile.FileName).ToLower() != ".mp4"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".avi"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".wmv"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".mpeg-2"
            && Path.GetExtension(postedFile.FileName).ToLower() != ".mov")
        {
            return false;
        }

        //-------------------------------------------
        //  Attempt to read the file and check the first bytes
        //-------------------------------------------
        try
        {
            if (!postedFile.OpenReadStream().CanRead)
            {
                return false;
            }
            //------------------------------------------
            //check whether the image size exceeding the limit or not
            //------------------------------------------ 
            //if (postedFile.Length < ImageMinimumBytes)
            //{
            //    return false;
            //}

            byte[] buffer = new byte[VideoMinimumBytes];
            postedFile.OpenReadStream().Read(buffer, 0, VideoMinimumBytes);
            string content = System.Text.Encoding.UTF8.GetString(buffer);
            if (Regex.IsMatch(content,
                    @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}