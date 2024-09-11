using System;
using System.Linq.Expressions;
using System.Reflection;
using ApplicationCore.Aggregates;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.DataAccess.Filter;

namespace ApplicationCore.Extentions
{
    public static class FilterExtension
    {
        public static List<T> Filter<T>(this List<T> data, T dataSearch) where T : Filter
        {
            return data.Where(item => SetConditionsUsingExpressionTree(dataSearch).All(strategy => strategy(item))).ToList();
        }

        public static void Print<T>(this List<T> data) where T : Filter
        {
            if (data.Count() == 0)
                Console.WriteLine($" {typeof(T).Name} is null");
            data.ToList().ForEach(i => {
                foreach (PropertyInfo propertyInfo in i.GetType().GetProperties())
                {
                    var propertyValue = propertyInfo.GetValue(i)?.ToString() ?? "null";
                    Console.Write($"{propertyInfo.Name}: {propertyValue}, ");
                }
                Console.WriteLine();
            });
        }

        private static List<Func<T, bool>> setConditionsByUsingReFlection<T>(T searchData) where T : Filter
        {
            List<Func<T, bool>> _filteringStrategies = new List<Func<T, bool>>();
            // Get all properties of searchData type

            foreach (PropertyInfo propertyInfo in searchData.GetType().GetProperties())
            {
                _filteringStrategies.Add(item =>
                {
                    // Get the value of the property from both searchData and item using reflection
                    var searchValue = propertyInfo.GetValue(searchData);
                    var itemValue = propertyInfo.GetValue(item);
                    if (searchValue == null && itemValue == null)
                        return true;
                    // Check if both values are equal
                    return searchValue != null && searchValue.Equals(itemValue);
                });
            }
            return _filteringStrategies;
        }

        private static List<Func<T, bool>> SetConditionsUsingExpressionTree<T>(T searchData) where T : Filter
        {
            var filteringStrategies = new List<Func<T, bool>>();

            var searchProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in searchProperties)
            {
                var searchValue = propertyInfo.GetValue(searchData);

                if (searchValue is null)
                {
                    // Build a lambda expression to compare each property dynamically
                    var parameter = Expression.Parameter(typeof(T), "item");

                    var propertyAccess = Expression.Property(parameter, propertyInfo);
                    var constant = Expression.Constant(searchValue);
                    var equalityCheck = Expression.Equal(propertyAccess, constant);

                    var lambda = Expression.Lambda<Func<T, bool>>(equalityCheck, parameter).Compile();

                    filteringStrategies.Add(lambda);
                }
            }
            return filteringStrategies;
        }
    }

}

