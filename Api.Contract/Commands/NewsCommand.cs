using Api.Common.Commands;
using System;

namespace Api.Contract.Commands
{
    public class CreateNewsCommand : ICommand<int>
    {

        public DateTime NewsDate { get; set; }

        public string? Content { get; set; }
        public bool Status { get; set; }

    }
    public class EditNewsCommand : ICommand<int>
    {
        public int NewsId { get; set; }

        public DateTime NewsDate { get; set; }

        public string? Content { get; set; }
        public bool Status { get; set; }

    }
    public class DeleteNewsCommand : ICommand<int>
    {
        //NewsId
        public int Id { get; set; }
    }
    #region Auto Comman Code
    #endregion
}
