using HyperMarket.Web.ActionHelpers;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyperMarket.Data;
using HyperMarket.Data.SqlServer;
using HyperMarket.DomainObjects;
using Microsoft.EntityFrameworkCore;
using HyperMarket.Queries;
using HyperMarket.Queries.Decorators;

namespace HyperMarket.Web.DependencyInjection
{
    public static class QueryRegistration
    {
        public static void RegisterQueries(this IServiceCollection services)
        {
            var assembly = Assembly.Load("HyperMarket.Queries");
            var interfaceType = typeof(IQueryHandler<,>);
            var bllType = typeof(BusinessLogicQueryHandler<,>);
            var types = assembly.GetTypes().Where(x => x.InheritsOrImplements(bllType) && x != bllType).ToList();

            foreach (var type in types)
            {
                services.AddScoped(type);
                var interfaces = type.GetInterfaces();
                var targetInt = interfaces.FirstOrDefault(x => x.Name == "IQueryHandler`2");
                var args = targetInt.GetGenericArguments();
                var queryInterface = typeof(IQueryHandler<,>).MakeGenericType(args);
                var loggerType = typeof(LoggingDecorator<,>).MakeGenericType(args);
                services.AddScoped(queryInterface, x =>
                {
                    var implementationInst = x.GetService(type);
                    return Activator.CreateInstance(loggerType, implementationInst);
                });
            }

            services.AddScoped<QueryManager>();
        }
    }
}
