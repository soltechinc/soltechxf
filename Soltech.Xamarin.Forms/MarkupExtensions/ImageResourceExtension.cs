using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SolTech.Forms
{
    /// <summary>
    /// Provides a mechanism to get an image from an embedded resource instead of a file path.
    /// </summary>
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Source))
                return null;

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source);

            return imageSource;
        }
    }
}
