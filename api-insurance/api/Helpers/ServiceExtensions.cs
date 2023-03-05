using System;
using Microsoft.EntityFrameworkCore;

namespace api.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureDBContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<InsuranceDBContext>(options => options!.UseSqlite(connectionString));
        }
    }
}

