using Api.Common.DataTable;
using Api.Common.Queries;
using Api.Contract.DTO;
using Api.Common.Mappers;

namespace Api.Contract.Queries
{
    public class GetStockBaseDataTableQuery : ISingleQuery<DataTableWrapper<StockBaseDTO>>
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public bool? isfavorite { get; set; }
        public bool? isexclude { get; set; }
        public DTParameterModel model { get; set; }
    }

    public class GetStockBaseByIdQuery : ISingleQuery<StockBaseDTO>
    {
        public string Id { get; set; }
    }
}
