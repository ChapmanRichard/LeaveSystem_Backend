using Api.Common.Commands;
using System;

namespace Api.Contract.Commands
{
    public class CreateStockBaseCommand : ICommand<string>
    {
        public string code { get; set; }
        public string name { get; set; }
        public bool isfavorite { get; set; }
        public bool isexclude { get; set; }
    }

    public class EditStockBaseCommand : ICommand<string>
    {
        public string code { get; set; }
        public string name { get; set; }
        public bool isfavorite { get; set; }
        public bool isexclude { get; set; }
    }

    public class DeleteStockBaseCommand : ICommand<string>
    {
        public string Id { get; set; }
    }
}
