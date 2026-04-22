using Api.Common.Vaildators;
using Api.Contract.Commands;
using Api.DAL;

namespace Api.BLL.CommandValidators
{
    public class StockBaseCommandValidator : IValidator<DeleteStockBaseCommand>,
        IValidator<CreateStockBaseCommand>,
        IValidator<EditStockBaseCommand>
    {
        private readonly UnitOfWorkLeaveSystem unitOfWorkStock;

        public StockBaseCommandValidator(UnitOfWorkLeaveSystem unitOfWorkStock)
        {
            this.unitOfWorkStock = unitOfWorkStock;
        }

        public void ValidateObject(DeleteStockBaseCommand instance)
        {
            // Validation logic for deleting a StockBase
        }

        public void ValidateObject(CreateStockBaseCommand instance)
        {
            // Validation logic for creating a StockBase
        }

        public void ValidateObject(EditStockBaseCommand instance)
        {
            // Validation logic for editing a StockBase
        }
    }
}
