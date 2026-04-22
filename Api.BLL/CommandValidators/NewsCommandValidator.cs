using Api.Common.Vaildators;
using Api.Contract.Commands;
using Api.Contract.Queries;
using Api.DAL;
using Api.DAL.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Api.BLL.CommandValidators
{
    public class NewsCommandValidator : IValidator<DeleteNewsCommand>,
        IValidator<CreateNewsCommand>,
        IValidator<EditNewsCommand>
    {

        private readonly UnitOfWorkLeaveSystem unitOfWorkStock;

        public NewsCommandValidator(UnitOfWorkLeaveSystem unitOfWorkStock)
        {
            this.unitOfWorkStock = unitOfWorkStock;
        }
        public void ValidateObject(DeleteNewsCommand instance)
        {

        }
        public void ValidateObject(CreateNewsCommand instance)
        {
            
        }

        public void ValidateObject(EditNewsCommand instance)
        {
            
        }
    }
}
