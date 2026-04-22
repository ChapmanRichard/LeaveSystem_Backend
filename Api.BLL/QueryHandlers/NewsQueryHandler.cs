using Api.Common.DataTable;
using Api.Common.Queries;
using Api.Contract.DTO;
using Api.Contract.Queries;
using Api.Contract.ServiceInterfaces;

namespace Api.BLL.QueryHandlers
{
    public class NewsQueryHandler : ISingleQueryHandler<GetNewsDataTableQuery, DataTableWrapper<NewsDTO>>
       , ISingleQueryHandler<GetNewsByIdQuery, NewsDTO>
    //more query code
    {
        private readonly INewsService newsService;
        public NewsQueryHandler(INewsService _newsService)
        {
            this.newsService = _newsService;
        }

        public DataTableWrapper<NewsDTO> Handle(GetNewsDataTableQuery query)
        {
            return this.newsService.GetDataTableRequest(query);
        }
        public NewsDTO Handle(GetNewsByIdQuery query)
        {
            return this.newsService.GetNewsById(query.Id);
        }
        #region Auto Query Code
        #endregion
    }
}
