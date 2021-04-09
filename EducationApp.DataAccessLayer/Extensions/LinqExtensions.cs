using EducationApp.Shared.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EducationApp.DataAccessLayer.Extensions
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource>(
       this IQueryable<TSource> query, string propertyName, bool isAscending)
        {
            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "entity");
            MemberExpression property = Expression.Property(arg, propertyName);
            if (propertyInfo is null)
            {
                propertyInfo = entityType.GetProperty(Constants.DEFAULTSORT);
                property = Expression.Property(arg, Constants.DEFAULTSORT);
            }
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });
            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(method => method.Name == (isAscending ? "OrderBy" : "OrderByDescending") && method.IsGenericMethodDefinition)
                 .Where(method =>
                 {
                     var parameters = method.GetParameters().ToList();

                     return parameters.Count is Constants.ORDERBYPARAMCOUNT;
                 }).Single();
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
        

    }
}
