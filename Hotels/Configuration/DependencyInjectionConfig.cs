using Hotels.Domain.Interfaces;
using Hotels.Domain.Models;
using Hotels.Domain.Services;
using Hotels.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Configuration
{
    public static class DependencyInjectionConfig
    {

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IHotelQueryRepository, HotelQueryRepository>();
            services.AddScoped<ICommandRepository<Hotel>, HotelCommandRepository>();
            services.AddScoped<IHotelService, HotelService>();

            return services;
        }
    }
}
