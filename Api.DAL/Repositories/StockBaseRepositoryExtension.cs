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
    public static class StockBaseRepositoryExtension
    {
        private static string includeStr = "";
        public static StockBase ExGetById(this IRepositoryBase<StockBase> repository, string Id)
        {
            var predicate = StockBaseSpecification.Code(Id);
            return repository.GetQueryable(predicate.Predicate, null, includeStr).FirstOrDefault();
        }
        public static DataTableWrapper<StockBase> ExGetDataTableResult(this IRepositoryBase<StockBase> repository, GetStockBaseDataTableQuery query)
        {
            string keyField = new Reflection().GetPropertyName<StockBase>(o => o.code);
            var predicate = StockBaseSpecification.Base;
            
            if (!string.IsNullOrEmpty(query.code))
            {
                predicate = predicate & StockBaseSpecification.Code(query.code);
            }
            if (!string.IsNullOrEmpty(query.name))
            {
                predicate = predicate & StockBaseSpecification.Name(query.name);
            }
            if (query.isfavorite.HasValue)
            {
                predicate = predicate & StockBaseSpecification.IsFavorite(query.isfavorite.Value);
            }
            if (query.isexclude.HasValue)
            {
                predicate = predicate & StockBaseSpecification.IsExclude(query.isexclude.Value);
            }

            (string order, bool desc, int draw, int skip, int take) = query.model.GetDTParameter();
            var recordsTotal = repository.Count();
            var filteredList = repository.GetDbSet().Where(predicate.Predicate);
            var recordsFiltered = filteredList.Count();
            var orderby = filteredList.GetOrderBy(order, desc, keyField);
            var list = take > 0 ? repository.GetList(take, skip, predicate.Predicate, orderby, includeStr) : repository.Get(predicate.Predicate, desc, order, includeStr);
            var stockbasedto = new DataTableWrapper<StockBase> { draw = draw, recordsTotal = recordsTotal, recordsFiltered = recordsFiltered, data = list };
            return stockbasedto;
        }
    }
}
