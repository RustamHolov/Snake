public class EventManager
{
    private Dictionary<string, IObserver> listeners;
    public EventManager(){
        listeners = new Dictionary<string, IObserver>();
    }
    public void Notify(string eventType, IObservable publisher)
    {
        if(listeners.TryGetValue(eventType, out IObserver? subscriber) && subscriber != null){
            subscriber.Update(publisher);
        }
    }

    public void Subscribe(string eventType, IObserver subscriber)
    {
        listeners.Add(eventType, subscriber);
    }

    public void Unscribe(string eventType, IObserver subscriber)
    {
        listeners.Remove(eventType);
    }
}