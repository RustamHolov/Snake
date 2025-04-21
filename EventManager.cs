public class EventManager 
{
    private Dictionary<Event, List<EventListener>> _listeners;
    public EventManager()
    {
        _listeners = new Dictionary<Event, List<EventListener>>();
    }

    public void Notify(Event eventType, object? args = null){
        if (_listeners.ContainsKey(eventType))
        {
            foreach (var listener in _listeners[eventType])
            {
                listener.Update(args);
            }
        }
    }
    public void Subscribe(Event eventType, EventListener subscriber)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _listeners[eventType] = new List<EventListener>();
        }
        _listeners[eventType].Add(subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        if (_listeners.ContainsKey(eventType))
        {
            _listeners[eventType].Remove(subscriber);
        }
    }
    public void ClearAllSubscriptions()
    {
        _listeners.Clear();
    }
}