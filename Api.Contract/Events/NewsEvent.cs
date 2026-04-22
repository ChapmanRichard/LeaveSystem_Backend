using Api.Common.Events;

namespace Api.Contract.Events
{
    public class NewsEvent : IEvent
    {
        public int NewsId { get; set; }
        public DateTime NewsDate { get; set; }

        public string? Content { get; set; }
        public bool Status { get; set; }
    }
}
