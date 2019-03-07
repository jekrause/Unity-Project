public interface ISubscriber<T>
{
    void OnEventHandler(T eventData);
}
