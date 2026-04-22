using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Api.Common.Linq
{
    public static class IQueryableOrderByExtension
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty,
                          bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
        public static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderBy<T>(this IQueryable<T> source, string order, bool desc, string keyField)
        {
            Type type = typeof(T);
            var proertyInfos = type.GetProperties();
            //var AppSort = !proertyInfos.Select(o => o.Name).Contains(order);
            //if (AppSort) return null;
            var columnName = order;
            var isChild = order.Contains(".");
            if (isChild)
            {
                columnName = order.Split('.')[0];
            }
            List<string> orderColumn = new List<string>();
            List<bool> orderDir = new List<bool>() { desc };
            foreach (var proertyInfo in proertyInfos)
            {
                if (columnName.ToUpper() == proertyInfo.Name.ToUpper())
                {
                    if (isChild)
                    {
                        orderColumn.Add(order);
                    }
                    else
                    {
                        orderColumn.Add(proertyInfo.Name);
                    }

                    return GetOrderBy<T>(orderColumn, orderDir);
                }
            }
            orderColumn.Add(keyField);
            return GetOrderBy<T>(orderColumn, orderDir);
        }

        public static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderByMultipleColumns<T>(this IQueryable<T> source, List<string> orders, List<bool> descs, string keyField)
        {
            Type type = typeof(T);
            var proertyInfos = type.GetProperties();
            //var AppSort = !proertyInfos.Select(o => o.Name).Contains(order);
            //if (AppSort) return null;
            List<string> orderColumn = new List<string>();
            List<bool> orderDir = new List<bool>();
            for (int i = 0; i < orders.Count; i++)
            {
                var columnName = orders[i];
                var isChild = orders[i].Contains(".");
                if (isChild)
                {
                    columnName = orders[i].Split('.')[0];
                }
                foreach (var proertyInfo in proertyInfos)
                {
                    if (columnName.ToUpper() == proertyInfo.Name.ToUpper())
                    {
                        if (isChild)
                        {
                            orderColumn.Add(orders[i]);
                            orderDir.Add(descs[i]);
                        }
                        else
                        {
                            orderColumn.Add(proertyInfo.Name);
                            orderDir.Add(descs[i]);
                        }
                    }
                }
            }
            if (orderColumn.Count > 0)
            {
                return GetOrderBy<T>(orderColumn, orderDir);
            }
            orderColumn.Add(keyField);
            orderDir.Add(descs[0]);
            return GetOrderBy<T>(orderColumn, orderDir);
        }
        /// <summary>
        /// 动态转换为Linq排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderList">[aaa,bbb,ccc],[asc,asc,desc]</param>
        /// <returns></returns>
        public static Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderBy<T>(List<string> orderColumn, List<bool> orderDir)
        {
            string ascKey = "OrderBy";
            string descKey = "OrderByDescending";

            Type typeQueryable = typeof(IQueryable<T>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "jk");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);

            for (int i = 0; i < orderColumn.Count; i++)
            {
                string columnName = orderColumn[i];
                bool dirKey = orderDir[i];

                IQueryable<T> query = new List<T>().AsQueryable<T>();
                Type type = typeof(T);
                ParameterExpression arg = Expression.Parameter(type, "uf");
                Expression expr = arg;

                if (columnName.Contains("."))
                {
                    // support to be sorted on child fields. 
                    String[] childProperties = columnName.Split('.');
                    System.Reflection.PropertyInfo property = typeof(T).GetProperty(childProperties[0]);
                    MemberExpression propertyAccess = Expression.MakeMemberAccess(arg, property);

                    for (int j = 1; j < childProperties.Length; j++)
                    {
                        Type t = property.PropertyType;
                        if (!t.IsGenericType)
                        {
                            property = t.GetProperty(childProperties[j]);
                        }
                        else
                        {
                            property = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                        }
                        type = property.PropertyType;
                        expr = Expression.MakeMemberAccess(propertyAccess, property);
                        //propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    }
                    //property = type.GetProperty(propertyName);
                    //propertyAccess = Expression.MakeMemberAccess(parameter, property);
                }
                else
                {
                    PropertyInfo pi = type.GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }

                LambdaExpression lambda = Expression.Lambda(expr, arg);
                //string methodName = dirKey ? ascKey : descKey;
                string methodName = dirKey ? descKey : ascKey;
                MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(T), type }, outerExpression.Body, Expression.Quote(lambda));

                outerExpression = Expression.Lambda(resultExp, argQueryable);

                ascKey = "ThenBy";
                descKey = "ThenByDescending";
            }
            return (Func<IQueryable<T>, IOrderedQueryable<T>>)outerExpression.Compile();
        }
    }
}
