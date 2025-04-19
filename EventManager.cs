public class EventManager : IObservable
{
    private Dictionary<Event, EventListener> listeners;
    public EventManager()
    {
        listeners = new Dictionary<Event, EventListener>();
    }
    public void Notify(Event eventType, IObservable publisher)
    {
        if (listeners.TryGetValue(eventType, out EventListener? subscriber) && subscriber != null)
        {
            subscriber.Update(publisher);
        }

    }
    public void Notify(Event eventType, object? args = null){
        if (listeners.TryGetValue(eventType, out EventListener? subscriber) && subscriber != null && args != null){
            if(subscriber is MenuHoverListener menuHoverListener && args is (Menu menu, int hover)){
                menuHoverListener.Update((menu, hover));
            }
            if (subscriber is MenuSelectedListener menuSelectedListener && args is Menu menu1)
            {
                menuSelectedListener.Update(menu1);
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