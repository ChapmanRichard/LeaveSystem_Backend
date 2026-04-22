using Api.Common.Commands;
using Api.Contract.Commands;
using Api.Contract.ServiceInterfaces;

namespace Api.BLL.CommandHandlers
{
    public class NewsCommandHandler : ICommandHandler<CreateNewsCommand, int>
        , ICommandHandler<DeleteNewsCommand, int>
        , ICommandHandler<EditNewsCommand, int>
    //more command code
    {
        private readonly INewsService newsService;
        public NewsCommandHandler(INewsService _newsService)
        {
            this.newsService = _newsService;
        }
        public int Handle(CreateNewsCommand cmd)
        {
            var id = this.newsService.CreateNews(cmd);
            return id;
        }
        public int Handle(DeleteNewsCommand cmd)
        {
            var id = this.newsService.DeleteNews(cmd);
            return id;
        }
        public int Handle(EditNewsCommand cmd)
        {
            var id = this.newsService.EditNews(cmd);
            return id;
        }
        #region Auto Comman Code
        #endregion
    }
}
