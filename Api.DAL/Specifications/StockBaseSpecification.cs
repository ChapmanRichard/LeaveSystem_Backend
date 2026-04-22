using Api.Common.Specifications;
using Api.Contract.Model;

namespace Api.DAL.Specifications
{
    public class StockBaseSpecification
    {
        public static Specification<StockBase> Base => new Specification<StockBase>(x => !string.IsNullOrEmpty(x.code));
        public static Specification<StockBase> Code(string query) => new Specification<StockBase>(x => x.code == query);
        public static Specification<StockBase> Name(string query) => new Specification<StockBase>(x => x.name.Contains(query));
        public static Specification<StockBase> IsFavorite(bool query) => new Specification<StockBase>(x => x.isfavorite == query);
        public static Specification<StockBase> IsExclude(bool query) => new Specification<StockBase>(x => x.isexclude == query);
    }
}
