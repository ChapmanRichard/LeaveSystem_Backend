using Api.Common.Commands;
using Api.Common.DI;
using Api.Contract.Commands;
using Api.Contract.Constants;
using Api.Contract.DTO;
using Api.Contract.Queries;
using Api.Contract.ServiceInterfaces;
using Api.DAL;
using System;
using System.Linq;

namespace Api.BLL.CommandDecorators
{
    public class ActionNameProvider
    {
        public string ActionName { get; set; } = "";

        public ActionNameProvider()
        {
        }

    }

    public class ActionNameCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {

        private ActionNameProvider actionNameProvider;
        private UnitOfWorkLeaveSystem unitOfWork;

        private readonly ICommandHandler<TCommand, TResult> handler;
        public ActionNameCommandHandlerDecorator(ICommandHandler<TCommand, TResult> handler, ActionNameProvider actionNameProvider, UnitOfWorkLeaveSystem unitOfWork)
        {
            this.handler = handler;
            this.actionNameProvider = actionNameProvider;
            this.unitOfWork = unitOfWork;
        }

        public TResult Handle(TCommand cmd)
        {
            var result = this.handler.Handle(cmd);
            return result;
        }

       
    }
}
