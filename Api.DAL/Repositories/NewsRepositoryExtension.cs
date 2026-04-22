using Api.Common.DataTable;
using Api.Common.Linq;
using Api.Common.Mappers;
using Api.Common.Repositories;
using Api.Contract.Model;
using Api.Contract.Queries;
using Api.DAL.Specifications;
using System.Linq;

namespace Api.DAL.Repositories
{
    public static class NewsRepositoryExtension
    {
        private static string includeStr = "";
        public static News? ExGetById(this IRepositoryBase<News> repository, int Id)
        {
            var predicate = NewsSpecification.Id(Id);
            return repository.GetQueryable(predicate.Predicate, null, includeStr).FirstOrDefault();
        }
        public static DataTableWrapper<News> ExGetDataTableResult(this IRepositoryBase<News> repository, GetNewsDataTableQuery query)
        {
            string keyField = new Reflection().GetPropertyName<News>(o => o.NewsId);
            var predicate = NewsSpecification.Base;
            //To Revise
            if (!string.IsNullOrEmpty(query.Content))
            {
                predicate = predicate & NewsSpecification.Content(query.Content);
            }
            if (query.NewsDate != null)
            {
                predicate = predicate & NewsSpecification.NewsDate(query.NewsDate.Value);
            }
            if (query.Status != null)
            {
                predicate = predicate & NewsSpecification.Status(query.Status.Value);
            }
            (string order, bool desc, int draw, int skip, int take) = query.model.GetDTParameter();
            var recordsTotal = repository.Count();
            var filteredList = repository.GetDbSet().Where(predicate.Predicate);
            var recordsFiltered = filteredList.Count();
            var orderby = filteredList.GetOrderBy(order, desc, keyField);
            var list = take > 0 ? repository.GetList(take, skip, predicate.Predicate, orderby, includeStr) : repository.Get(predicate.Predicate, desc, order, includeStr);
            var Newsdto = new DataTableWrapper<News> { draw = draw, recordsTotal = recordsTotal, recordsFiltered = recordsFiltered, data = list };
            return Newsdto;
        }
        #region Auto Query Code
        #endregion
    }
}
