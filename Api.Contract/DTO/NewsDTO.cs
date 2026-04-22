using System;

namespace Api.Contract.DTO
{
    public class NewsDTO
    {

        public int NewsId { get; set; }

        public DateTime NewsDate { get; set; }

        public string? Content { get; set; }
        public bool Status { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public string? CreatedPostBy { get; set; }

        public string? UpdatedPostBy { get; set; }

        public string? CreatedRoleBy { get; set; }

        public string? UpdatedRoleBy { get; set; }

    }
}
