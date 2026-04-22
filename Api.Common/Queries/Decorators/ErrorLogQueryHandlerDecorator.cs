using Api.Common.Loggers;
using System;
using System.Collections.Generic;

namespace Api.Common.Queries.Decorators
{
    public class ErrorLogQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> decorated;
        private readonly IAppLogger logger;
        public ErrorLogQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated, IAppLogger logger)
        {
            this.decorated = decorated;
            this.logger = logger;
        }

        public IList<TResult> Handle(TQuery query)
        {
            try
            {
                //var Enable_eMSPSLog = ConfigurationManager.AppSettings["Enable_eMSPS_QueryCommandLog"];
                //if(Enable_eMSPSLog == "True")
                //{
                //    var userId = GetUserId();
                //    var externalUserId = GetExternalUserId();
                //    var queryName = query.GetType().Name;
                //    var parameters = JsonConvert.SerializeObject(query);

                //    var msgtemplate = $"userId: {userId}, externalUserId: {externalUserId} {Environment.NewLine} query:{queryName} {Environment.NewLine} parameters: {parameters} {Environment.NewLine}";
                //    logger.Info(msgtemplate);
                //}

                return this.decorated.Handle(query);
            }
            catch (Exception e)
            {
                logger.Error(e, query.GetType().Name);
                throw;
            }



        }


    }

    public class ErrorLogSingleQueryHandlerDecorator<TQuery, TResult> : ISingleQueryHandler<TQuery, TResult> where TQuery : ISingleQuery<TResult>
    {
        private readonly ISingleQueryHandler<TQuery, TResult> decorated;
        private readonly IAppLogger logger;
        public ErrorLogSingleQueryHandlerDecorator(ISingleQueryHandler<TQuery, TResult> decorated, IAppLogger logger)
        {
            this.decorated = decorated;
            this.logger = logger;
        }

        public TResult Handle(TQuery query)
        {
            try
            {
                //var Enable_eMSPSLog = ConfigurationManager.AppSettings["Enable_eMSPS_QueryCommandLog"];
                //if (Enable_eMSPSLog == "True")
                //{
                //    var userId = GetUserId();
                //    var externalUserId = GetExternalUserId();
                //    var queryName = query.GetType().Name;
                //    var parameters = JsonConvert.SerializeObject(query);

                //    var msgtemplate = $"userId: {userId}, externalUserId: {externalUserId} {Environment.NewLine} query:{queryName} {Environment.NewLine} parameters: {parameters} {Environment.NewLine}";
                //    logger.Info(msgtemplate);
                //}
                return this.decorated.Handle(query);
            }
            catch (Exception e)
            {
                logger.Error(e, query.GetType().Name);
                throw;
            }

        }


    }

}
