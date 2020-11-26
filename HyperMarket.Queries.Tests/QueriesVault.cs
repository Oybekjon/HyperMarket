using HyperMarket.Data;
using HyperMarket.Data.SqlServer;
using HyperMarket.Errors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace HyperMarket.Queries.Tests
{
    public class QueriesVault
    {
        private static readonly Lazy<QueriesVault> _Lazy =
            new Lazy<QueriesVault>(() => new QueriesVault());
        public static QueriesVault Instance => _Lazy.Value;

        private static readonly Type RepositoryType = typeof(RepositoryContextBase);
        private static readonly Type MockHelperType = typeof(MockHelper);
        private readonly Dictionary<Type, Delegate> QueryMappings;
        private QueriesVault()
        {
            QueryMappings = new Dictionary<Type, Delegate>();

            var assembly = Assembly.Load("HyperMarket.Queries");
            var bllType = typeof(BusinessLogicQueryHandler<,>);
            var types = assembly.GetTypes()
                .Where(x => x.InheritsOrImplements(bllType) && x != bllType).ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                var targetInt = interfaces.FirstOrDefault(x => x.Name == "IQueryHandler`2");
                var args = targetInt.GetGenericArguments();
                var bllConcreteType = bllType.MakeGenericType(args);

                var implConstructor = type.GetConstructors().FirstOrDefault();

                var param = Expression.Parameter(RepositoryType, "x");
                var implExpr = BuildConstructor(param, implConstructor);
                var bodyExpr = Expression.Convert(implExpr, bllConcreteType);
                var lambdaType = typeof(Func<,>).MakeGenericType(RepositoryType, bllConcreteType);

                var methodExpr = Expression.Lambda(lambdaType, bodyExpr, param);
                var method = methodExpr.Compile();

                QueryMappings[bllConcreteType] = method;
            }
        }

        private static Expression BuildConstructor(Expression dbParam, ConstructorInfo ctor)
        {
            var paramList = new List<Expression>();
            var ctorParams = ctor.GetParameters();
            foreach (var param in ctorParams)
            {
                if (param.ParameterType.InheritsOrImplements(RepositoryType))
                {
                    paramList.Add(dbParam);
                }
                else
                {
                    var callEpxr = Expression.Call(MockHelperType, "GetMock", new[] { param.ParameterType });
                    var prop = ObjectProperty(param.ParameterType);
                    var objectProp = Expression.Property(callEpxr, prop);
                    paramList.Add(objectProp);
                }
            }
            return Expression.New(ctor, paramList);
        }

        private static PropertyInfo ObjectProperty(Type mockedType)
        {
            var type = typeof(Mock<>).MakeGenericType(mockedType);
            var propInfo = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == "Object" && x.PropertyType == mockedType);

            return propInfo;
        }

        public BusinessLogicQueryHandler<TQuery, TResult> GetQueryHandler<TQuery, TResult>(RepositoryContextBase context) where TQuery : IQuery<TResult>
        {
            var queryType = typeof(BusinessLogicQueryHandler<TQuery, TResult>);
            if (QueryMappings.ContainsKey(queryType))
            {
                var method = (Func<RepositoryContextBase, BusinessLogicQueryHandler<TQuery, TResult>>)QueryMappings[queryType];
                return method(context);
            }

            throw new NotFoundException($"{queryType.Name} mapping not found");
        }
    }
}
