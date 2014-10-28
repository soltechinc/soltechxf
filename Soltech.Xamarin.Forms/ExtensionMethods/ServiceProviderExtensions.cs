using System;

namespace SolTech.Forms
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            T service = serviceProvider.GetService(typeof(T)) as T;
            return service;
        }
    }
}
