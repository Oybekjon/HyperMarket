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
using System.Linq.Expressions;

namespace HyperMarket.Web.DependencyInjection
{
    public static class QueryRegistration
    {
        private static readonly Type ServiceProviderType = typeof(IServiceProvider);
        private static readonly MethodInfo GetServiceMethod = typeof(IServiceProvider).GetMethod("GetService");

        private static void RegisterQuery(IServiceCollection services, Type type)
        {
            var interfaces = type.GetInterfaces();
            var targetInt = interfaces.FirstOrDefault(x => x.Name == "IQueryHandler`2");
            var args = targetInt.GetGenericArguments();
            var queryInterface = typeof(IQueryHandler<,>).MakeGenericType(args);
            var loggerType = typeof(LoggingDecorator<,>).MakeGenericType(args);
            var loggerConstructor = loggerType.GetConstructors().First();
            var implConstructor = type.GetConstructors().First();
            var param = Expression.Parameter(ServiceProviderType, "x");

            var implExpr = BuildConstructor(param, implConstructor, null);
            var implConverted = Expression.Convert(implExpr, queryInterface);

            var loggerImpl = BuildConstructor(param, loggerConstructor, implConverted);
            var loggerConverted = Expression.Convert(loggerImpl, queryInterface);

            var body = Expression.Convert(loggerConverted, typeof(object));
            var methodExpr = Expression.Lambda<Func<IServiceProvider, object>>(body, param);
            var method = methodExpr.Compile();
            services.AddScoped(queryInterface, method);
        }

        private static Expression BuildConstructor(Expression serviceParam, ConstructorInfo ctor, Expression queryInstanceExpression)
        {
            var paramList = new List<Expression>();
            var ctorParams = ctor.GetParameters();
            foreach (var param in ctorParams)
            {
                if (param.ParameterType.InheritsOrImplements(typeof(IQueryHandler<,>)) && queryInstanceExpression != null)
                {
                    paramList.Add(queryInstanceExpression);
                }
                else
                {
                    var callEpxr = Expression.Call(serviceParam, GetServiceMethod, Expression.Constant(param.ParameterType));
                    var converted = Expression.Convert(callEpxr, param.ParameterType);
                    paramList.Add(converted);
                }
            }
            return Expression.New(ctor, paramList);
        }

        public static void RegisterQueries(this IServiceCollection services)
        {
            var assembly = Assembly.Load("HyperMarket.Queries");
            var interfaceType = typeof(IQueryHandler<,>);
            var bllType = typeof(BusinessLogicQueryHandler<,>);
            var types = assembly.GetTypes().Where(x => x.InheritsOrImplements(bllType) && x != bllType).ToList();

            foreach (var type in types)
            {
                RegisterQuery(services, type);
            }

            services.AddScoped<QueryManager>();
        }
    }
}
