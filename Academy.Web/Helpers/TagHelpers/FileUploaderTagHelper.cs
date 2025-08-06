using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

/// <summary>
/// an enum for file types that can be uploaded through the chunk-upload tag helper
/// </summary>
public enum UploaderAllowedFileType
{
    Image,
    Audio,
    Video,
    Pdf,
    Compressed,
    Documents,
    Mp4 , 
    Rar , 
    Any,
}

/// <summary>
/// file types extensions for setting the file type limitations based on the <see cref="UploaderAllowedFileType"/> enum.
/// </summary>
public static class FileTypeExtensions
{
    private const string ImageFormatTypes =
        "'.jpg', '.jpeg', '.png', '.gif', '.bmp', '.tiff', '.tif', '.svg', '.webp', '.heic', '.heic' ,  '.psd', '.ico'";

    private const string VideoFormatTypes =
        "'.mp4', '.mov', '.avi', '.mkv', '.wmv', '.flv', '.webm', '.mpg', '.mpeg', '.3gp', '.m4v', '.vob'";

    private const string AudioFormatTypes =
        "'.mp3', '.wav', '.flac', '.aac', '.ogg', '.wma', '.m4a', '.alac', '.aiff', '.aif', '.pcm', '.opus', '.mid', '.midi', '.amr'";

    private const string PdfFormatType = "'.pdf'";

    private const string CompressedFormatTypes =
        "'.zip', '.rar', '.7z', '.tar', '.gz', '.bz2', '.xz', '.iso', '.dmg', '.tgz', '.tar.gz'";

    private const string DocumentsFormatTypes = "'.pdf', '.docx', '.txt'";

    private const string RarFormatType = "'.rar'";
    
    private const string Mp4FormatType = "'.mp4'";

    public static string? GetAllowedFileTypes(this UploaderAllowedFileType allowedFileType)
    {
        return allowedFileType switch
        {
            UploaderAllowedFileType.Image => ImageFormatTypes,
            UploaderAllowedFileType.Audio => AudioFormatTypes,
            UploaderAllowedFileType.Video => VideoFormatTypes,
            UploaderAllowedFileType.Pdf => PdfFormatType,
            UploaderAllowedFileType.Compressed => CompressedFormatTypes,
            UploaderAllowedFileType.Documents => DocumentsFormatTypes,
            UploaderAllowedFileType.Mp4 => Mp4FormatType, 
            UploaderAllowedFileType.Rar => RarFormatType,
            UploaderAllowedFileType.Any => null,
            _ => throw new ArgumentOutOfRangeException(nameof(allowedFileType), allowedFileType, null)
        };
    }
}

/// <summary>
/// tag helper for adding chunk upload ability for uploading files. you should set chunk-upload to true and also you should bind a model expression into it
/// </summary>
[HtmlTargetElement("button", Attributes = ChunkUploadAttributeName)]
public class FileUploaderTagHelper : TagHelper
{
    private const string AllowedFileTypesAttributeName = "allowed-type";
    private const string ChunkUploadAttributeName = "chunk-upload";
    private const string LocationAttributeName = "location";
    private const string FileNameAttributeName = "filename";
    private const string BindFileNameAttributeName = "bind";

    private const string BindedFileInputArgumantNullMessage =
        "you must select an input from the model for this tag helper to work.";


    /// <summary>
    /// the location to upload completed file on there. it takes a relative path from the root of the project with no slash or backslash at the begging 
    /// </summary>
    [HtmlAttributeName(LocationAttributeName)]
    public string? Location { get; set; }

    /// <summary>
    /// the name of the file to be set for upload. if you not fill this option it generates a unique identifier for the file name.
    /// </summary>
    [HtmlAttributeName(FileNameAttributeName)]
    public string FileName { get; set; } = Guid.NewGuid().ToString();

    [HtmlAttributeName(ChunkUploadAttributeName)]
    public bool IsActive { get; set; }

    /// <summary>
    /// set the allowed file types to limit the uploaded file types based on the file type should be uploaded. default value is Any
    /// </summary>
    [HtmlAttributeName(AllowedFileTypesAttributeName)]
    public UploaderAllowedFileType AllowedFileType { get; set; } = UploaderAllowedFileType.Any;

    /// <summary>
    /// takes a model expression for binding the uploaded file name into it after the file uploaded completely.
    /// it,s a required property for this tag helper to work. and you should have an input on your page with the attribute name set to the exact selected name of the model expression here to work properly. 
    /// </summary>
    /// <exception cref="ArgumentNullException">if it,s not set the tag helper throws an <see cref="ArgumentNullException"/></exception>
    [HtmlAttributeName(BindFileNameAttributeName)]
    public ModelExpression BindedFileNameInput { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!IsActive) return;

        if (BindedFileNameInput is null)
            throw new ArgumentNullException(nameof(BindedFileNameInput), BindedFileInputArgumantNullMessage);

        string? fileTypes = AllowedFileType.GetAllowedFileTypes();
        if (!string.IsNullOrEmpty(fileTypes?.Trim()))
            output.Attributes.SetAttribute("data-allowed-types", fileTypes);

        output.Attributes.SetAttribute("data-bind", BindedFileNameInput.Name);


        output.Attributes.SetAttribute("data-chunk-upload", "true");
        if (!string.IsNullOrEmpty(Location))
            output.Attributes.SetAttribute("data-loc", Location);

        output.Attributes.SetAttribute("data-sfm", FileName);
    }
}