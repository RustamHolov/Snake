public class EventManager
{
    private Dictionary<Event, List<IEventListener>> _listeners;
    public EventManager()
    {
        _listeners = new Dictionary<Event, List<IEventListener>>();
    }

    public void Notify(Event eventType, object? args = null)
    {
        if (_listeners.ContainsKey(eventType))
        {
            foreach (var listener in _listeners[eventType])
            {
                listener.Update(args);
            }
        }
    }
    public void Subscribe(Event eventType, IEventListener subscriber)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _listeners[eventType] = new List<IEventListener>();
        }
        _listeners[eventType].Add(subscriber);
    }

    public void Unscribe(Event eventType, IEventListener subscriber)
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