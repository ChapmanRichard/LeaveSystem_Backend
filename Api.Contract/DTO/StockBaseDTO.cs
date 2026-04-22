using System;

namespace Api.Contract.DTO
{
    public class StockBaseDTO
    {
        public string code { get; set; }
        public string name { get; set; }
        public bool isfavorite { get; set; }
        public bool isexclude { get; set; }
    }
}
