using Api.Common.DataTable;
using Api.Common.Queries;
using Api.Contract.DTO;
using Api.Contract.Queries;
using Api.Contract.ServiceInterfaces;

namespace Api.BLL.QueryHandlers
{
    public class StockBaseQueryHandler : ISingleQueryHandler<GetStockBaseDataTableQuery, DataTableWrapper<StockBaseDTO>>,
        ISingleQueryHandler<GetStockBaseByIdQuery, StockBaseDTO>
    {
        private readonly IStockBaseService stockBaseService;

        public StockBaseQueryHandler(IStockBaseService _stockBaseService)
        {
            this.stockBaseService = _stockBaseService;
        }

        public DataTableWrapper<StockBaseDTO> Handle(GetStockBaseDataTableQuery query)
        {
            return this.stockBaseService.GetDataTableRequest(query);
        }

        public StockBaseDTO Handle(GetStockBaseByIdQuery query)
        {
            return this.stockBaseService.GetStockBaseById(query.Id);
        }
    }
}
