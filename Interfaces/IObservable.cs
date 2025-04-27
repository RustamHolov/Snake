public interface IObservable
{
    void Subscribe(Event eventType, IEventListener subscriber);
    void Unscribe(Event eventType, IEventListener subscriber);
    void Notify(Event eventType, object? args = null);
}