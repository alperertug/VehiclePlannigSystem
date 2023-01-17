using Business.Abstract;
using Business.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Business.Extensions
{
    public static class BusinessLayerExtensions
    {
        public static IServiceCollection LoadBusinessLayerExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IVehicleService, VehicleManager>();
            services.AddAutoMapper(assembly);

            return services;
        }
    }
}
