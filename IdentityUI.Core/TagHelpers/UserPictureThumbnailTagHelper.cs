using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityUI.Core.TagHelpers
{
    public class UserPictureThumbnailTagHelper : TagHelper
    {
        public string? PictureUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if (string.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/UserPictures/defaultUserPicture.png");
                output.Attributes.SetAttribute("width", "300px");
                output.Attributes.SetAttribute("height", "300px");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/UserPictures/{PictureUrl}");
                output.Attributes.SetAttribute("width", "300px");
                output.Attributes.SetAttribute("height", "300px");
            }
            //base.Process(context, output);
        }
    }
}
