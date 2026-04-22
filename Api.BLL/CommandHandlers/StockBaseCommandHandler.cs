using Api.Common.Commands;
using Api.Contract.Commands;
using Api.Contract.ServiceInterfaces;

namespace Api.BLL.CommandHandlers
{
    public class StockBaseCommandHandler : ICommandHandler<CreateStockBaseCommand, string>,
        ICommandHandler<DeleteStockBaseCommand, string>,
        ICommandHandler<EditStockBaseCommand, string>
    {
        private readonly IStockBaseService stockBaseService;
        public StockBaseCommandHandler(IStockBaseService _stockBaseService)
        {
            this.stockBaseService = _stockBaseService;
        }
        public string Handle(CreateStockBaseCommand cmd)
        {
            var id = this.stockBaseService.CreateStockBase(cmd);
            return id;
        }
        public string Handle(DeleteStockBaseCommand cmd)
        {
            var id = this.stockBaseService.DeleteStockBase(cmd);
            return id;
        }
        public string Handle(EditStockBaseCommand cmd)
        {
            var id = this.stockBaseService.EditStockBase(cmd);
            return id;
        }
    }
}
