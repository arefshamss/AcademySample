using Academy.Application.Convertors;
using Academy.Application.Statics;
using Academy.Domain.Shared;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Academy.Application.Extensions;

public static class FileExtensions
{
    public static async Task<byte[]> GetBytes(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
    
    #region Image

    public static async Task<Result<string>> AddImageToServer(
        this IFormFile image,
        string originalPath,
        int? width = default,
        int? height = default,
        string? thumbPath = null,
        string? suggestedFileName = null,
        string? deleteFileName = null,
        bool checkImageFormat = true)
    {
        if (string.IsNullOrEmpty(suggestedFileName))
            suggestedFileName = Guid.NewGuid().ToString();

        suggestedFileName = suggestedFileName.ToLower()
            .Trim()
            .Replace(" ", "_")
            .Replace("/", "_")
            .Replace("%", "_")
            .Replace("?", "_")
            .Replace("#", "_")
            .Replace("+", "plus")
            .Replace(":", "")
            .Replace("\\", "_");

        while (File.Exists(originalPath + suggestedFileName + Path.GetExtension(image.FileName)))
            suggestedFileName += new Random().Next(1, 10).ToString();

        suggestedFileName += Path.GetExtension(image.FileName);

        if (checkImageFormat)
        {
            if (image == null || !image.IsImage())
                return Result.Failure<string>(ErrorMessages.FileFormatError);
        }

        originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;
        thumbPath = Directory.GetCurrentDirectory() + "/wwwroot" + thumbPath;

        if (!Directory.Exists(originalPath))
            Directory.CreateDirectory(originalPath);

        if ((!string.IsNullOrEmpty(deleteFileName)) && deleteFileName != SiteTools.DefaultImageName)
        {
            if (File.Exists(originalPath + deleteFileName))
                File.Delete(originalPath + deleteFileName);

            if (!string.IsNullOrEmpty(thumbPath))
            {
                if (File.Exists(thumbPath + deleteFileName))
                    File.Delete(thumbPath + deleteFileName);
            }
        }

        string finalPath = originalPath + suggestedFileName;

        await using (var stream = new FileStream(finalPath, FileMode.Create))
            if (!Directory.Exists(finalPath))
                await image.CopyToAsync(stream);


        if (string.IsNullOrEmpty(thumbPath)) return suggestedFileName;

        if (!Directory.Exists(thumbPath))
            Directory.CreateDirectory(thumbPath);

        ImageOptimizer resizer = new();

        if (width != null || height != null)
            resizer.ImageResizer(originalPath + suggestedFileName, thumbPath + suggestedFileName, width, height);

        return suggestedFileName;
    }


    public static void DeleteImage(this string imageName, string originalPath, string? thumbPath)
    {
        if ((string.IsNullOrEmpty(imageName)) || imageName == SiteTools.DefaultImageName) return;
        originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;
        thumbPath = Directory.GetCurrentDirectory() + "/wwwroot" + thumbPath;

        if (File.Exists(originalPath + imageName))
            File.Delete(originalPath + imageName);

        if (string.IsNullOrEmpty(thumbPath)) return;
        if (File.Exists(thumbPath + imageName))
            File.Delete(thumbPath + imageName);
    }

    public static void DeleteImage(this string imageName, string originalPath)
    {
        if ((string.IsNullOrEmpty(imageName)) || imageName == SiteTools.DefaultImageName) return;
        originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;

        if (File.Exists(originalPath + imageName))
            File.Delete(originalPath + imageName);
    }

    public static async Task<StreamContent> ConvertIFormFileToStreamContent(this IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            // Copy the contents of the IFormFile to the MemoryStream
            await file.CopyToAsync(memoryStream);

            // Reset the position of the MemoryStream to the beginning
            memoryStream.Position = 0;

            // Create a StreamContent object from the MemoryStream
            StreamContent streamContent = new StreamContent(memoryStream);

            // Optionally, set the content type of the StreamContent based on the IFormFile
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            return streamContent;
        }
    }

    #endregion

    #region File

    public static async Task<Result<string>> AddFilesToServer(this IFormFile file, string fileName, string originalPath,
        string deleteFileName = null, bool checkFileExtension = true, string customFolderPath = null)
    {
        if (file != null)
        {
            if (file.IsFile(checkFileExtension))
            {
                if (!string.IsNullOrEmpty(customFolderPath))
                {
                    if (!Directory.Exists(customFolderPath))
                        Directory.CreateDirectory(customFolderPath);

                    originalPath = Path.Combine(Directory.GetCurrentDirectory() + customFolderPath + originalPath);
                }
                else
                {
                    originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;
                }

                if (!Directory.Exists(originalPath))
                    Directory.CreateDirectory(originalPath);

                if (!string.IsNullOrEmpty(deleteFileName))
                {
                    if (!string.IsNullOrEmpty(customFolderPath))
                    {
                        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), customFolderPath, originalPath, deleteFileName)))
                            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), customFolderPath, originalPath, deleteFileName));
                    }
                    else
                    {
                        if (File.Exists(originalPath + deleteFileName))
                            File.Delete(originalPath + deleteFileName);
                    }
                }

                string finalPath = originalPath + fileName;

                await using var stream = new FileStream(finalPath, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;   
            }
            
            return Result.Failure<string>(ErrorMessages.FileFormatError);
        }

        return Result.Failure<string>(ErrorMessages.FileNotFound);
    }
    
    
        public static async Task<Result<string>> AddArchiveFilesToServer(this IFormFile file, string fileName, string originalPath,
        string deleteFileName = null, bool checkFileExtension = true, string customFolderPath = null)
    {
        if (file != null)
        {
            if (file.IsArchiveFile(checkFileExtension))
            {
                if (!string.IsNullOrEmpty(customFolderPath))
                {
                    if (!Directory.Exists(customFolderPath))
                        Directory.CreateDirectory(customFolderPath);

                    originalPath = Path.Combine(Directory.GetCurrentDirectory() + customFolderPath + originalPath);
                }
                else
                {
                    originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;
                }

                if (!Directory.Exists(originalPath))
                    Directory.CreateDirectory(originalPath);

                if (!string.IsNullOrEmpty(deleteFileName))
                {
                    if (!string.IsNullOrEmpty(customFolderPath))
                    {
                        if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), customFolderPath, originalPath, deleteFileName)))
                            File.Delete(Path.Combine(Directory.GetCurrentDirectory(), customFolderPath, originalPath, deleteFileName));
                    }
                    else
                    {
                        if (File.Exists(originalPath + deleteFileName))
                            File.Delete(originalPath + deleteFileName);
                    }
                }

                string finalPath = originalPath + fileName;

                await using var stream = new FileStream(finalPath, FileMode.Create);
                await file.CopyToAsync(stream);

                return fileName;   
            }
            
            return Result.Failure<string>(ErrorMessages.FileArchiveFormatError);
        }

        return Result.Failure<string>(ErrorMessages.FileNotFound);
    }

    public static void DeleteFile(this string fileName, string originalPath)
    {
        if (string.IsNullOrEmpty(fileName)) return;
        originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;

        if (File.Exists(originalPath + fileName))
        {
            File.Delete(originalPath + fileName);
        }
    }

    public static async Task<Result<string>> AddFilesToServerWithNoFormatChecker(this IFormFile file, string fileName,
        string originalPath)
    {
        originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;

        if (!Directory.Exists(originalPath))
            Directory.CreateDirectory(originalPath);

        string finalPath = originalPath + fileName;

        await using var stream = new FileStream(finalPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }

    public static FileStream GetFileStream(this string path)
    {
        if (!File.Exists(path))
            return null;

        FileStream stream = File.OpenRead(path);

        return stream;
    }

    #endregion

    #region Video

    public static async Task AddVideoToServer(this IFormFile video, string fileName, string originalPath,
        string deleteFileName = null, bool checkFileExtension = true)
    {
        if (video != null && video.IsVideo())
        {
            originalPath = Directory.GetCurrentDirectory() + "/wwwroot" + originalPath;

            if (!Directory.Exists(originalPath))
                Directory.CreateDirectory(originalPath);

            if (!string.IsNullOrEmpty(deleteFileName))
            {
                if (File.Exists(originalPath + deleteFileName))
                {
                    File.Delete(originalPath + deleteFileName);
                }
            }

            if (!Directory.Exists(originalPath))
                Directory.CreateDirectory(originalPath);

            string finalPath = originalPath + fileName;

            await using var stream = new FileStream(finalPath, FileMode.Create);
            await video.CopyToAsync(stream);
        }
    }

    #endregion

    public static bool IsFile(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".rar" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".zip" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".pdf" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".txt" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".doc" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".docx" ||
               postedFile.IsImage();
    }
    
    public static bool IsArchiveFile(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".rar" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".zip";
    }

    public static bool IsRarFile(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".rar";
    }

    public static bool IsAudio(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".mp3" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".webm";
    }

    public static bool IsPdf(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".pdf";
    }

    public static bool IsImage(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".jpg" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".jpeg" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".png" ||
               Path.GetExtension(postedFile.FileName)?.ToLower() == ".webp";
    }

    public static bool IsSvg(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".svg";
    }

    public static bool IsIcon(this IFormFile postedFile, bool checkFileExtension = true)
    {
        if (!checkFileExtension) return true;
        return Path.GetExtension(postedFile.FileName)?.ToLower() == ".ico";
    }


    #region ConvertToImgToIco

    public static byte[] ConvertPngToIco(byte[] pngImageBytes, string outputPath = null, int width = 32, int height = 32)
    {
        using (var msPng = new MemoryStream(pngImageBytes))
        {
            // Load the image using ImageSharp
            var image = Image.Load(msPng);

            // Resize the image to the required size
            var resizedImage = image.Clone(x => x.Resize(width, height));

            // Save the resized image as a PNG
            using (var msPngOutput = new MemoryStream())
            {
                resizedImage.Save(msPngOutput, new SixLabors.ImageSharp.Formats.Png.PngEncoder());

                // Optionally save the PNG file to disk
                if (!string.IsNullOrEmpty(outputPath))
                {
                    File.WriteAllBytes(outputPath, msPngOutput.ToArray());
                }

                // Return the PNG byte array (to be converted to ICO later)
                return msPngOutput.ToArray();
            }
        }
    }

    #endregion
}