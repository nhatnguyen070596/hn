using System;
using ApplicationCore.Aggregates;
using ApplicationCore.Interfaces.DataAccess.Filter;
using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;

namespace ApplicationCore.Extentions
{
	public static class FilterAggregateExtentions
	{
        public static IQueryable<TExpected> Where<T, TExpected>(this IQueryable<TExpected> data, T filter)
        where T : Filter where TExpected : class
        {
            Expression<Func<TExpected, bool>> filteringExpression = SetConditions<T,TExpected>(filter);
            return data.Where(filteringExpression);
        }

        // Use expression trees to build filtering conditions for IQueryable (without compiling)
        private static Expression<Func<TExpected, bool>> SetConditions<T, TExpected>(T filter)
        where T : Filter where TExpected : class
        {
            var parameter = Expression.Parameter(typeof(TExpected),nameof(TExpected));
            Expression? predicateBody = null;

            var filterProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var metaDataProperties = typeof(TExpected).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in filterProperties)
            {
                 var rootProperty =  metaDataProperties
                                   .FirstOrDefault(r => r.Name == propertyInfo.Name && r.GetType() == propertyInfo.GetType());

                if (rootProperty is not null)
                {
                    var filterValue = propertyInfo.GetValue(filter);

                    if (filterValue is not null)
                    {
                        var propertyAccess = Expression.Property(parameter, rootProperty);
                        //Meta data property
                        Expression left = propertyAccess;
                        //Search value 
                        Expression right = Expression.Constant(filterValue);

                        var equalityCheck = Expression.Equal(left, right);

                        predicateBody = predicateBody == null ? equalityCheck : Expression.AndAlso(predicateBody, equalityCheck);
                    }
                    // Access the property from the TExpected instance (parameter)
                }
            }

            if (predicateBody == null)
                // Return an expression that always returns true if no conditions are set
                return Expression.Lambda<Func<TExpected, bool>>(Expression.Constant(true), parameter);

            // Create the final predicate lambda expression
            return Expression.Lambda<Func<TExpected, bool>>(predicateBody, parameter);
        }

    }
}

