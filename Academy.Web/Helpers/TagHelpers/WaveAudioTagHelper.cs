using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Academy.Web.Helpers.TagHelpers;

[HtmlTargetElement("wave-audio") ]
public class WaveAudioTagHelper : TagHelper
{
    private const string AudioSourceAttributeName = "source";
    
    [HtmlAttributeName(AudioSourceAttributeName)]
    public string AudioSource { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if(string.IsNullOrEmpty(AudioSource))
            throw new ArgumentNullException(nameof(AudioSource) , "The source audio clip cannot be null or empty.");
        var path = Directory.GetCurrentDirectory() + "/wwwroot" + AudioSource;
        if(!File.Exists(path))
            throw new FileNotFoundException("The source audio clip cannot be found.", AudioSource);
        
        output.TagName = "div";
        output.AddClass("waveAudio" , HtmlEncoder.Default);
        output.AddClass("d-flex" , HtmlEncoder.Default);
        output.AddClass("my-2" , HtmlEncoder.Default);
        output.AddClass("align-items-center" , HtmlEncoder.Default);
        
        output.Attributes.SetAttribute("data-src", AudioSource);
    }
}