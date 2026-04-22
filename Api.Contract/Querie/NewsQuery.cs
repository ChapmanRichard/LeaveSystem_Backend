using Api.Common.DataTable;
using Api.Common.Mappers;
using Api.Common.Queries;
using Api.Contract.DTO;
using System;

namespace Api.Contract.Queries
{
    public class GetNewsDataTableQuery : ISingleQuery<DataTableWrapper<NewsDTO>>
    {

        public int NewsId { get; set; }

        public DateTime? NewsDate { get; set; }

        public string? Content { get; set; }
        public bool? Status { get; set; }

        public DTParameterModel? model { get; set; }

    }
    public class GetNewsByIdQuery : ISingleQuery<NewsDTO>
    {
        //NewsId
        public int Id { get; set; }
    }
    #region Auto Query Code
    #endregion
}
