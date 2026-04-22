using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Common.DataTable
{
    public class DataTableWrapper<T>
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public IList<T> data { get; set; }
    }
    public class DateTableRequest
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<Columnx> columns { get; set; }
        public List<Orderx> order { get; set; }
        public Searchx search { get; set; }

    }
    public class Columnx
    {
        [FromQuery(Name = "[data]")]
        public string data { get; set; }
        [FromQuery(Name = "[name]")]
        public string name { get; set; }
        [FromQuery(Name = "[searchable]")]
        public bool searchable { get; set; }
        [FromQuery(Name = "[orderable]")]
        public bool orderable { get; set; }
        [FromQuery(Name = "[search]")]
        public Searchx search { get; set; }
    }
    public class Searchx
    {
        [FromQuery(Name = "[value]")]
        public string value { get; set; }
        [FromQuery(Name = "[regex]")]
        public bool regex { get; set; }
    }
    public class Orderx
    {
        [FromQuery(Name = "[column]")]
        public int column { get; set; }
        [FromQuery(Name = "[dir]")]
        public string dir { get; set; }
    }
}
