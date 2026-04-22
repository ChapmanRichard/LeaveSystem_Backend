using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ASL.Common.Mappers
{
    public class DTParameterModel
    {
        [FromQuery(Name = "draw")]
        public int Draw { get; set; }
        [FromQuery(Name = "start")]
        public int Start { get; set; }
        [FromQuery(Name = "length")]
        public int Length { get; set; }
        [FromQuery(Name = "columns")]
        public List<DTColumn> Columns { get; set; }
        [FromQuery(Name = "order")]
        public List<DTOrder> Order { get; set; }
        [FromQuery(Name = "search")]
        public DTSearch Search { get; set; }

    }
    public class DTColumn
    {
        public int Index { get; set; }
        [FromQuery(Name = "[data]")]
        public string Data { get; set; }
        [FromQuery(Name = "[name]")]
        public string Name { get; set; }
        [FromQuery(Name = "[searchable]")]
        public bool Searchable { get; set; }
        [FromQuery(Name = "[orderable]")]
        public bool Orderable { get; set; }
        [FromQuery(Name = "[search]")]
        public DTSearch Search { get; set; }
    }
    public class DTSearch
    {
        [FromQuery(Name = "[value]")]
        public string Value { get; set; }
        [FromQuery(Name = "[regex]")]
        public bool Regex { get; set; }
    }
    public class DTOrder
    {
        [FromQuery(Name = "[column]")]
        public int Column { get; set; }
        [FromQuery(Name = "[dir]")]
        public string Dir { get; set; }
    }
}
