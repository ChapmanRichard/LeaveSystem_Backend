namespace Api.Common.Events
{
    public interface IEventBus
    {
        void Submit<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
