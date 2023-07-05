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
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static async Task<IQueryable<T>> WithDataAuthAsync<T, TKey>(this IQueryable<T> query, string subjectId, string accessAttributeCode, Expression<Func<T, TKey>> filterProperty
            , CancellationToken cancellationToken = default, GrantType grantType = GrantType.ForUser, string? localLookupValue = null)
            where T : class
            where TKey : struct
        {
            if (_serviceProvider == null)
            {
                throw new Exception("Please add EfExtensions.Initialize to application initialization!");
            }

            using var scope = _serviceProvider.CreateScope();
            var coreService = scope.ServiceProvider.GetRequiredService<ICoreService>();

            // Get granted data
            var grantedData = await coreService.GetDataPermissions<TKey>(subjectId, accessAttributeCode, grantType, localLookupValue, cancellationToken);

            // Create lambda expression for query condition
            var propertyInfo = filterProperty.GetPropertyAccess();
            var expressionParam = Expression.Parameter(typeof(T), "x");
            var method = grantedData.GetType().GetMethod("Contains");
            var expressionCall = Expression.Call(Expression.Constant(grantedData), method!, Expression.Property(expressionParam, propertyInfo));
            var lamda = Expression.Lambda<Func<T, bool>>(expressionCall, expressionParam);
            return query.Where(lamda);
        }
    }
}
