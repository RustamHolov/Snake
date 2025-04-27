public class Settings : IObservable
{
    private EventManager _events;
    private GameState _gameState = GameState.Menu;
    private int _gameSpeed = (int)Speeds.Moderate;
    public EventManager Events { get => _events; }
    public bool SizeEdited = false;
    private int _gameSize = (int)GameSizes.Normal;

    public int GameSize
    {
        get => _gameSize;
        set
        {
            if (_gameSize != value)
            {
                _gameSize = value;
                SizeEdited = true;
                Notify(Event.Size, value);
            }
        }
    }
    public int PixelSize { get; set; } = (int)PixelSizes.Normal;
    public int Speed
    {
        get => _gameSpeed;
        set
        {
            if (_gameSpeed != value)
            {
                _gameSpeed = value;
            }
        }
    }

    public GameState GameState
    {
        get => _gameState;
        set
        {
            if (_gameState != value)
            {
                _gameState = value;
                Notify(Event.State);
            }
        }
    }
    public Settings(EventManager eventManager)
    {
        _events = eventManager;
    }
    public void Notify(Event eventType, object? args = null)
    {
        _events.Notify(eventType, args);
    }

    public void Subscribe(Event eventType, IEventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, IEventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }
}