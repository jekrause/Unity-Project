public interface IEventAggregator
{
    void Publish<T>(T EventData);
    void Register<T>(ISubscriber<T> eventHandler);
    void Unregister<T>(ISubscriber<T> eventHandler);

}
