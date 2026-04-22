using Api.Common.DataTable;
using Api.Contract.Commands;
using Api.Contract.DTO;
using Api.Contract.Queries;

namespace Api.Contract.ServiceInterfaces
{
    /// <summary>
    ///  
    /// </summary>
    public interface INewsService
    {
        #region 
        NewsDTO GetNewsById(int Id);
        DataTableWrapper<NewsDTO> GetDataTableRequest(GetNewsDataTableQuery query);
        #region Auto Query Code
        #endregion

        int CreateNews(CreateNewsCommand cmd);
        int EditNews(EditNewsCommand cmd);
        int DeleteNews(DeleteNewsCommand cmd);
        #region Auto Comman Code
        #endregion

        #endregion
    }
}
