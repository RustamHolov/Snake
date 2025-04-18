public class EventManager
{
    private Dictionary<Event, IObserver> listeners;
    public EventManager(){
        listeners = new Dictionary<Event, IObserver>();
    }
    public void Notify(Event eventType, IObservable publisher)
    {
        if(listeners.TryGetValue(eventType, out IObserver? subscriber) && subscriber != null){
            subscriber.Update(publisher);
        }
        
    }
    public void Notify(Event eventType, GameSize size){
        if (listeners.TryGetValue(eventType, out IObserver? subscriber) && subscriber != null)
        {
            if (subscriber is SetSizeListener sizeListener){
                sizeListener.Update(size);
            }
                
        }
    }

    public void Subscribe(Event eventType, IObserver subscriber)
    {
        listeners.Add(eventType, subscriber);
    }

    public void Unscribe(Event eventType, IObserver subscriber)
    {
        listeners.Remove(eventType);
    }
}