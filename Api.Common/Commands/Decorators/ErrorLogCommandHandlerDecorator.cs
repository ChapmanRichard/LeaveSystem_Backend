using Api.Common.Loggers;
using System;

namespace Api.Common.Commands.Decorators
{
    public class ErrorLogCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> handler;
        private readonly IAppLogger logger;


        public ErrorLogCommandHandlerDecorator(ICommandHandler<TCommand, TResult> handler, IAppLogger logger)
        {
            this.handler = handler;
            this.logger = logger;
        }

        public TResult Handle(TCommand command)
        {
            try
            {
                //var Enable_eMSPSLog = ConfigurationManager.AppSettings["Enable_eMSPS_QueryCommandLog"];
                //if (Enable_eMSPSLog == "True")
                //{
                //    var userId = GetUserId();
                //    var externalUserId = GetExternalUserId();
                //    var commandName = command.GetType().Name;
                //    var parameters = JsonConvert.SerializeObject(command);

                //    var msgtemplate = $"userId: {userId}, externalUserId: {externalUserId} {Environment.NewLine} cmd:{commandName} {Environment.NewLine} parameters: {parameters} {Environment.NewLine}";
                //    logger.Info(msgtemplate);
                //}

                return this.handler.Handle(command);
            }
            catch (Exception e)
            {
                this.logger.Error(e, command.GetType().Name);
                throw;
            }
        }




    }
}
