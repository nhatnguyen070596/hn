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
        public static IQueryable<TExpected> Where<T, TExpected>(this IQueryable<TExpected> data, T dataSearch)
        where T : Filter where TExpected : class
        {
            var filteringExpression = SetConditions<T,TExpected>(dataSearch);
            return data.Where(filteringExpression);
        }

        // Use expression trees to build filtering conditions for IQueryable (without compiling)
        private static Expression<Func<TExpected, bool>> SetConditions<T, TExpected>(T searchData)
        where T : Filter where TExpected : class
        {
            var parameter = Expression.Parameter(typeof(TExpected),nameof(TExpected));
            Expression? predicateBody = null;

            var searchProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var rootProperties = typeof(TExpected).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in searchProperties)
            {
                var rootProperty = rootProperties
                                   .FirstOrDefault(r => r.Name == propertyInfo.Name && r.GetType() == propertyInfo.GetType());

                if (rootProperty is not null)
                {
                    var searchValue = propertyInfo.GetValue(searchData);

                    if (searchValue is not null && !searchValue.Equals(0))
                    {
                        var propertyAccess = Expression.Property(parameter, rootProperty);

                        //Meta data property
                        Expression left = propertyAccess; 
                        //Search value 
                        Expression right = Expression.Constant(searchValue);

                        var equalityCheck = Expression.Equal(left, right);

                        predicateBody = predicateBody == null ? equalityCheck : Expression.AndAlso(predicateBody, equalityCheck);
                    }
                    // Access the property from the TExpected instance (parameter)
                }
            }

            if (predicateBody == null)
            {
                // Return an expression that always returns true if no conditions are set
                return Expression.Lambda<Func<TExpected, bool>>(Expression.Constant(true), parameter);
            }

            // Create the final predicate lambda expression
            return Expression.Lambda<Func<TExpected, bool>>(predicateBody, parameter);
        }

    }
}

