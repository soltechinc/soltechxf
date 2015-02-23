using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SolTech.Forms.MarkupExtensions
{
    [ContentProperty("ID")]
    public class LocalizeExtension : IMarkupExtension
    {
        public String ResourceNamespace { get; set; }
        public String ResourceName { get; set; }
        public String ID { get; set; }

        public ILocalizer _localizer;

        public static String DefaultResourceNamespace { get; set; }

        public LocalizeExtension()
        {
        }

        public LocalizeExtension(ILocalizer localizer)
        {
            if (localizer == null) throw new ArgumentNullException("localizer");
            _localizer = localizer;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
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

            // If the XAML contains an explicit resource namespace and/or name,
            // we will use those.
            var resourceNamespace = ResourceNamespace;
            var resourceName = ResourceName;

            // If no resource namespace was provided, we'll use whatever was
            // supplied as a default.
            if (String.IsNullOrEmpty(resourceNamespace))
                resourceNamespace = DefaultResourceNamespace;

            // If no resource name was provided, we'll use the name of the 
            // class that backs the XAML view.
            if (String.IsNullOrEmpty(resourceName))
                resourceName = visualElementType.Name;

            // Use the class factory to get an instance of an ILocalizer
            var localizer = GetLocalizer(visualElementType.GetTypeInfo().Assembly);

            // Combine the name and resource namespace to get the localized string
            String localizationResult = _localizer.GetText(resourceNamespace, resourceName, ID);

            if (String.IsNullOrEmpty(localizationResult)) localizationResult = "ResourceNotFound";
            
            return localizationResult;
        }

        /// <summary>
        /// A class factory for localizers.
        /// </summary>
        /// <param name="assembly">
        /// The <see cref="Assembly"/> to use for resource resolution.
        /// </param>
        /// <returns>An instance of <see cref="ILocalizer"/></returns>
        /// <remarks>
        /// If an <see cref="ILocalizer"/> was provided through the constructor,
        /// that instance will be used.
        /// </remarks>
        private ILocalizer GetLocalizer(Assembly assembly)
        {
            if (_localizer == null)
                _localizer = new EmbeddedResourceLocalizer(assembly);
            return _localizer;
        }
    }
}