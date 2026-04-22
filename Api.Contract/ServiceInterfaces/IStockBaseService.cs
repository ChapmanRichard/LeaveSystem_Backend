using Api.Common.DataTable;
using Api.Contract.Commands;
using Api.Contract.DTO;
using Api.Contract.Queries;

namespace Api.Contract.ServiceInterfaces
{
    /// <summary>
    ///  
    /// </summary>
    public interface IStockBaseService
    {
        #region 
        StockBaseDTO GetStockBaseById(string Id);
        DataTableWrapper<StockBaseDTO> GetDataTableRequest(GetStockBaseDataTableQuery query);
        #region Auto Query Code
        #endregion

        string CreateStockBase(CreateStockBaseCommand cmd);
        string EditStockBase(EditStockBaseCommand cmd);
        string DeleteStockBase(DeleteStockBaseCommand cmd);
        #region Auto Comman Code
        #endregion

        #endregion
    }
}
