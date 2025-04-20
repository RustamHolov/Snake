public class EventManager 
{
    private Dictionary<Event, EventListener> listeners;
    public EventManager()
    {
        listeners = new Dictionary<Event, EventListener>();
    }

    public void Notify(Event eventType, object? args = null){
        if (listeners.TryGetValue(eventType, out EventListener? subscriber) && subscriber != null){
            if(args != null){
                subscriber.Update(args);
            }else{
                subscriber.Update();
            }
        }
    }
    public void Subscribe(Event eventType, EventListener subscriber)
    {
        listeners.Add(eventType, subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        listeners.Remove(eventType);
    }
}