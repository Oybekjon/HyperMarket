using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyperMarket.Data;
using HyperMarket.Data.SqlServer;
using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using HyperMarket.Web.ActionHelpers;

namespace HyperMarket.Web.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MainContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<DbContext, MainContext>();
            services.AddScoped<RepositoryContextBase, SqlServerRepositoryContext>();
            services.AddScoped<AuthenticationHelper>();
        }
    }
}
