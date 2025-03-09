using Microsoft.Extensions.DependencyInjection;
using RealEstate.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;

namespace RealEstate.Internal.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register external services with HttpClient
            services.AddScoped<IExturnalTestService, ExturnalTestService>();

            return services;
        }
    }
}
