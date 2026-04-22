using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Contract.Model
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        public DateTime NewsDate { get; set; }

        public string? Content { get; set; }
        public bool Status { get; set; }

    }
}
