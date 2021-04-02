using EducationApp.Shared.Constants;
using EducationApp.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Extensions
{
    public static class LinqExtensions
    {
        public static IOrderedQueryable<TSource> OrderBy<TSource>(
       this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "entity");
            MemberExpression property;
            if (propertyInfo is null)
            {
                propertyInfo = entityType.GetProperty(Constants.DEFAULTSORT);
                property = Expression.Property(arg, Constants.DEFAULTSORT);
            }
            else
            {
                property = Expression.Property(arg, propertyName);
            }
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(method => method.Name == "OrderBy" && method.IsGenericMethodDefinition)
                 .Where(method =>
                 {
                     var parameters = method.GetParameters().ToList();         
             return parameters.Count == 2;
         }).Single();
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(
       this IQueryable<TSource> query, string propertyName)
        {
            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);
            ParameterExpression arg = Expression.Parameter(entityType, "entity");
            MemberExpression property;
            if (propertyInfo is null)
            {
                propertyInfo = entityType.GetProperty(Constants.DEFAULTSORT);
                property = Expression.Property(arg, Constants.DEFAULTSORT);
            }
            else
            {
                property = Expression.Property(arg, propertyName);
            }
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(Queryable);
            var method = enumarableType.GetMethods()
                 .Where(method => method.Name == "OrderByDescending" && method.IsGenericMethodDefinition)
                 .Where(method =>
                 {
                     var parameters = method.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();
            MethodInfo genericMethod = method
                 .MakeGenericMethod(entityType, propertyInfo.PropertyType);

            var newQuery = (IOrderedQueryable<TSource>)genericMethod
                 .Invoke(genericMethod, new object[] { query, selector });
            return newQuery;
        }
    }
}
