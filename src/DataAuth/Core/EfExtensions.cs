using DataAuth.Base;
using DataAuth.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Core
{
    public static class EfExtensions
    {
        private static IServiceProvider? _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static async Task<IQueryable<T>> WithDataAuthAsync<T, TKey>(
            this IQueryable<T> query,
            string subjectId,
            string accessAttributeCode,
            Expression<Func<T, TKey>> filterProperty,
            CancellationToken cancellationToken = default,
            GrantType grantType = GrantType.ForUser,
            string? localLookupValue = null,
            string functionCode = FunctionCode.All
        )
            where T : class
        {
            if (_serviceProvider == null)
            {
                throw new Exception(
                    "Please add EfExtensions.Initialize to application startup, after dependency injection!"
                );
            }

            using var scope = _serviceProvider.CreateScope();
            var coreService = scope.ServiceProvider.GetRequiredService<ICoreService>();

            // Get granted data
            var permissionResult = await coreService.GetDataPermissions<TKey>(
                subjectId,
                accessAttributeCode,
                grantType,
                localLookupValue,
                functionCode,
                cancellationToken
            );
            var grantedData = permissionResult.GrantedValues;

            if (grantedData != null && grantedData.Any())
            {
                // Create lambda expression for query condition
                var lambda = CreateContainsLambdaExpression(filterProperty, grantedData);
                return query.Where(lambda);
            }
            // If don't have any permission then return empty query
            return Enumerable.Empty<T>().AsQueryable();
        }

        static Expression<Func<T, bool>> CreateContainsLambdaExpression<T, TKey>(
            Expression<Func<T, TKey>> filterProperty,
            IEnumerable<TKey> grantedData
        )
        {
            var methods = typeof(Enumerable)
                .GetMethods()
                .Where(x => x.Name == nameof(Enumerable.Contains))
                .ToList();
            // Get Contains method having 2 parameters
            // public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value)
            var method = methods.First(m =>
            {
                if (!m.IsGenericMethod || m.GetParameters().Length != 2)
                {
                    return false;
                }
                return true;
            });
            var genericMethod = method.MakeGenericMethod(new Type[] { typeof(TKey) });
            var memberExpression = (MemberExpression)filterProperty.Body;
            var expressionCall = Expression.Call(
                null,
                genericMethod!,
                Expression.Constant(grantedData),
                memberExpression
            );
            var expressionParam = filterProperty.Parameters[0];
            var lambda = Expression.Lambda<Func<T, bool>>(expressionCall, expressionParam);
            return lambda;
        }
    }
}
