using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace SolTech.Forms
{
    public class EmbeddedResourceLocalizer : ILocalizer
    {
        private readonly IDictionary<String, IDictionary<String, ResourceManager>> _namespaceCache;
        private readonly Assembly _assembly;

        public EmbeddedResourceLocalizer(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            _assembly = assembly;

            _namespaceCache = new Dictionary<String, IDictionary<String, ResourceManager>>();
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            if (String.IsNullOrEmpty(namespaceKey)) throw new ArgumentNullException("namespaceKey");
            if (String.IsNullOrEmpty(typeKey)) throw new ArgumentNullException("typeKey");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            // Have we already loaded this resource class?
            String localizedText = null;

            IDictionary<String, ResourceManager> typeCache;
            ResourceManager resourceManager = null;

            if (!_namespaceCache.TryGetValue(namespaceKey, out typeCache))
            {
                System.Diagnostics.Debug.WriteLine("Adding namespace '{0}' to resource cache.", namespaceKey);
                typeCache = new Dictionary<String, ResourceManager>();
                _namespaceCache[namespaceKey] = typeCache;
            }

            if (!typeCache.TryGetValue(typeKey, out resourceManager))
            {
                System.Diagnostics.Debug.WriteLine("No cached type information found for '{0}.{1}'", namespaceKey, typeKey);
                resourceManager = GetResourceManagerForType(_assembly, namespaceKey, typeKey);
                typeCache[typeKey] = resourceManager;
            }

            if (resourceManager != null)
            {
                localizedText = resourceManager.GetString(name);
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(localizedText), "Expected to have found a resource string with name '" + name + "'");
                if (!String.IsNullOrEmpty(localizedText))
                {
                    if (formatArgs != null && formatArgs.Length > 0)
                    {
                        localizedText = String.Format(System.Globalization.CultureInfo.CurrentUICulture, localizedText, formatArgs);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No resource found for '{0}.{1}':{2}", namespaceKey, typeKey, name);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No type information found for '{0}.{1}'", namespaceKey, typeKey);
            }

            return localizedText;
        }

        private static ResourceManager GetResourceManagerForType(Assembly assembly, String namespaceKey, String typeKey)
        {
            return new ResourceManager(namespaceKey + "." + typeKey, assembly);
        }
    }
}
