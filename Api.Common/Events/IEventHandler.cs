namespace Api.Common.Events
{
    public interface IEventHandler<TEvent>
    {
        void Handle(TEvent @event);
    }
}
