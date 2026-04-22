using Api.Common.DI;

namespace Api.Common.Events
{
    public class MemoryEventBus : IEventBus
    {
        public void Submit<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            dynamic handlers = DIContainer.Instance.GetAllInstances(handlerType);
            foreach (var handler in handlers)
            {
                handler.Handle((dynamic)@event);
            }
        }
    }
}
