public class Settings : IObservable{
    private EventManager _events;
    public EventManager Events {get => _events;}
    private  int _gameSize = (int)GameSizes.Normal;
    public  int GameSize 
    {
        get => _gameSize;
        set {
            if(_gameSize != value){
                _gameSize = value;
                Notify(Event.Size, value);
            }
        }
    }
    public int PixelSize{get; set;} = (int)PixelSizes.Normal;
    public int Speed {get; set;} = (int)Speeds.Normal;
    public Settings(EventManager eventManager){
        _events = eventManager;
    }
    public void Notify(Event eventType, object? args = null)
    {
        _events.Notify(eventType, args);
    }

    public void Subscribe(Event eventType, EventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }
} 