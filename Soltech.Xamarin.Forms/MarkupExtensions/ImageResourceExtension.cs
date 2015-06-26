using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
        private static readonly Regex _resourceSplitter = new Regex("(?<resourceName>[^;]+)(?:;assembly=(?<assemblyName>.+))?");

        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Source))
            {
                System.Diagnostics.Debug.WriteLine("ImageResourceExtension::ProvideValue - No value supplied for Source.");
                return null;
            }
            System.Diagnostics.Debug.WriteLine("ImageResourceExtension::ProvideValue - Attempting to load resource '{0}'...", Source);

            ImageSource imageSource = null;
            var match = _resourceSplitter.Match(Source);
            if (match.Success)
            {
                var resourceName = match.Groups["resourceName"].Value;
                var assemblyName = match.Groups["assemblyName"].Value;
                Assembly assembly = null;
                if (!String.IsNullOrEmpty(assemblyName))
                {
                    assembly = System.Reflection.Assembly.Load(new AssemblyName(assemblyName));
                }
                else
                {
                    if (serviceProvider == null)
                    {
                        throw new ArgumentNullException("serviceProvider");
                    }
                    IRootObjectProvider rootObjectProvider = serviceProvider.GetService<IRootObjectProvider>();
                    if (rootObjectProvider == null)
                    {
                        throw new ArgumentException("serviceProvider did not provide an IRootObjectProvider");
                    }

                    // Find the root visual element (our view)
                    var visualElement = rootObjectProvider.RootObject as VisualElement;
                    var visualElementType = visualElement.GetType();
                    assembly = visualElementType.GetTypeInfo().Assembly;
                }
                
#if DEBUG
                if (assembly != null && !assembly.GetManifestResourceNames().Contains(resourceName))
                {
                    System.Diagnostics.Debug.WriteLine("ImageResourceExtension::ProvideValue - No resource found for stream name '{0}'.", Source);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ImageResourceExtension::ProvideValue - Found resource '{0}'.", Source);
                }
#endif
                // Do your translation lookup here, using whatever method you require
                imageSource = ImageSource.FromStream(() => assembly.GetManifestResourceStream(resourceName));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("ImageResourceExtension::ProvideValue - Unable to parse source string '{0}'.", Source);
            }

            return imageSource;
        }
    }
}
