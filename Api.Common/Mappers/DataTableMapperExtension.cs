using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Api.Common.Mappers
{
    public static class DataTableMapperExtension
    {

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        /// 
        public static string ToDataTableJsonString<T>(this IList<T> list, DTParameterModel model, string error = null)
        {
            int draw = model.Draw;
            int start = model.Start;
            int length = model.Length;

            if (!string.IsNullOrEmpty(error))
            {
                var error_data = new { draw, recordsTotal = 0, recordsFiltered = 0, data = "", error };
                return JsonConvert.SerializeObject(error_data);
            }



            var orderByColumnIndex = model.Order.FirstOrDefault()?.Column;
            var orderBy = model.Columns.ToList().FirstOrDefault(d => d.Index == orderByColumnIndex)?.Data ?? "";
            var orderType = model.Order.FirstOrDefault()?.Dir;
            var predicate = CreateAllColumnsSearchPredicate<T>(model);

            var total = list.Count;
            var qlist = list.AsQueryable();
            if (predicate != null)
            {
                qlist = qlist.Where(predicate);
            }
            var recordsFiltered = qlist.Count();
            if (orderBy != "")
            {
                if (orderType == "asc")
                {
                    list = qlist.OrderBy(orderBy).Skip(start).Take(length).ToList();
                }
                else
                {
                    list = qlist.OrderByDescending(orderBy).Skip(start).Take(length).ToList();
                }
            }
            else
            {
                list = qlist.Skip(start).Take(length).ToList();
            }


            var data = new { draw, recordsTotal = total, recordsFiltered, data = list };
            return JsonConvert.SerializeObject(data);

        }

        public static List<T> FilterDataTable<T>(this List<T> list, DTParameterModel model, out int recordsFiltered, string error = null)
        {
            int draw = model.Draw;
            int start = model.Start;
            int length = model.Length;

            var orderByColumnIndex = model.Order.FirstOrDefault()?.Column;
            var orderBy = model.Columns.ToList().FirstOrDefault(d => d.Index == orderByColumnIndex)?.Data ?? "";
            var orderType = model.Order.FirstOrDefault()?.Dir;

            var predicate = CreateAllColumnsSearchPredicate<T>(model);



            var total = list.Count;
            var qlist = list.AsQueryable();
            if (predicate != null)
            {
                qlist = qlist.Where(predicate);
            }
            recordsFiltered = qlist.Count();
            if (orderBy != "")
            {
                if (orderType == "asc")
                {
                    list = qlist.OrderBy(orderBy).Skip(start).Take(length).ToList();
                }
                else
                {
                    list = qlist.OrderByDescending(orderBy).Skip(start).Take(length).ToList();
                }
            }
            else
            {
                list = qlist.Skip(start).Take(length).ToList();
            }

            return list;
        }

        private static Expression<Func<T, bool>> CreatePredicate<T>(DTColumn column)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "X");
            Expression property = Expression.Property(parameter, column.Name);
            Expression target = Expression.Constant(column.Search.Value);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            // Expression equalsMethod = Expression.Call(property, "Equals", null, target);
            var equalsMethod = Expression.Call(property, method, target);
            Expression<Func<T, bool>> lambda =
               Expression.Lambda<Func<T, bool>>(equalsMethod, parameter);

            return lambda;
        }

        public static Expression<Func<T, bool>> CreateAllColumnsSearchPredicate<T>(DTParameterModel model)
        {
            Expression<Func<T, bool>> exp = d => true;
            foreach (var column in model.Columns)
            {
                if (!string.IsNullOrEmpty(column.Search.Value))
                {
                    var temp = CreatePredicate<T>(column);
                    var paramExpr = Expression.Parameter(typeof(T), "X");
                    var exprBody = Expression.And(exp.Body, temp.Body);
                    exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
                    var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
                    exp = finalExpr;
                }
            }
            return exp;
        }

        public static (string, bool, int, int, int) GetDTParameter(this DTParameterModel dTParameterModel)
        {
            string order = "";
            var desc = true;

            if (dTParameterModel.Order != null)
            {
                var orderByColumnIndex = dTParameterModel.Order.FirstOrDefault()?.Column;
                if (!string.IsNullOrEmpty(dTParameterModel.Order.ToList().FirstOrDefault()?.ColumnName))
                {
                    order = dTParameterModel.Order.ToList().FirstOrDefault()?.ColumnName;
                }
                else
                {
                    order = dTParameterModel.Columns.ToList().FirstOrDefault(d => d.Index == orderByColumnIndex)?.Data ?? "";
                }

                desc = dTParameterModel.Order.FirstOrDefault()?.Dir == "desc" ? true : false;
            }
            int draw = dTParameterModel.Draw;
            int skip = dTParameterModel.Start;
            int take = dTParameterModel.Length;
            return (order, desc, draw, skip, take);
        }

        public static (List<string>, List<bool>, int, int, int) GetDTParameterForMultipleColumns(this DTParameterModel dTParameterModel)
        {
            List<string> orders = new List<string>();
            List<bool> descs = new List<bool>();
            var desc = true;

            if (dTParameterModel.Order != null)
            {
                foreach (var ord in dTParameterModel.Order)
                {
                    var orderByColumnIndex = ord.Column;
                    string order = dTParameterModel.Columns.ToList().FirstOrDefault(d => d.Index == orderByColumnIndex)?.Data ?? "";
                    orders.Add(order);
                    desc = ord.Dir == "desc" ? true : false;
                    descs.Add(desc);
                }
            }
            int draw = dTParameterModel.Draw;
            int skip = dTParameterModel.Start;
            int take = dTParameterModel.Length;
            return (orders, descs, draw, skip, take);
        }

    }
    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(_parameter);
        }

        public ParameterReplacer(ParameterExpression parameter)
        {
            _parameter = parameter;
        }
    }

    public interface IDataTableMapper<T>
    {
        string Map(List<T> list, int draw, int start, int length, string orderBy, string orderType, Expression<Func<T, bool>> predicate, string error);
        Expression<Func<T, bool>> CreateAllColumnsSearchPredicate(DTParameterModel model);
    }
    public class DataTableMapper<T> : IDataTableMapper<T>
    {
        public string Map(List<T> list, int draw, int start, int length, string orderBy, string orderType, Expression<Func<T, bool>> predicate, string error = null)
        {

            if (!string.IsNullOrEmpty(error))
            {
                var error_data = new { draw, recordsTotal = 0, recordsFiltered = 0, data = "", error };
                return JsonConvert.SerializeObject(error_data);
            }

            var total = list.Count;
            var qlist = list.AsQueryable();
            if (predicate != null)
            {
                qlist = qlist.Where(predicate);
            }
            var recordsFiltered = qlist.Count();
            if (orderBy != "")
            {
                if (orderType == "asc")
                {
                    list = qlist.OrderBy(orderBy).Skip(start).Take(length).ToList();
                }
                else
                {
                    list = qlist.OrderByDescending(orderBy).Skip(start).Take(length).ToList();
                }
            }
            else
            {
                list = qlist.Skip(start).Take(length).ToList();
            }


            var data = new { draw, recordsTotal = total, recordsFiltered, data = list };
            return JsonConvert.SerializeObject(data);



        }

        //private Expression<Func<T, bool>> CreatePredicate(DTColumn column)
        //{


        //    ParameterExpression parameter = Expression.Parameter(typeof(T), "X");
        //    Expression property = Expression.Property(parameter, column.Name);
        //    Expression target = Expression.Constant(column.Search.Value);
        //    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        //    // Expression equalsMethod = Expression.Call(property, "Equals", null, target);
        //    var equalsMethod = Expression.Call(property, method, target);
        //    Expression<Func<T, bool>> lambda =
        //       Expression.Lambda<Func<T, bool>>(equalsMethod, parameter);

        //    return lambda;
        //}

        public Expression<Func<T, bool>> CreateAllColumnsSearchPredicate(DTParameterModel model)
        {

            return DataTableMapperExtension.CreateAllColumnsSearchPredicate<T>(model);

            //Expression<Func<T, bool>> exp = d => true;
            //foreach (var column in model.Columns)
            //{
            //    if (!string.IsNullOrEmpty(column.Search.Value))
            //    {
            //        var temp = CreatePredicate(column);
            //        var paramExpr = Expression.Parameter(typeof(T), "X");
            //        var exprBody = Expression.And(exp.Body, temp.Body);
            //        exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            //        var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            //        exp = finalExpr;
            //    }
            //}
            //return exp;
        }
    }

    public static class OrderedQueryableExtension
    {


        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }
        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
