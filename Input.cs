public class Input : IObservable
{

    private EventManager _events;
    public EventManager Events { get => _events; set { _events = value; } }
    public Input(EventManager events)
    {
        _events = events;
    }

    public void ReadMenuOption(Menu menu)
    {
        while (Console.KeyAvailable)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Enter:
                    Notify(Event.MenuSelect, menu);
                    break;
                case ConsoleKey.W or ConsoleKey.UpArrow:
                    Notify(Event.MenuHover, (menu, -1));
                    break;
                case ConsoleKey.S or ConsoleKey.DownArrow:
                    Notify(Event.MenuHover, (menu, 1));
                    break;
            }
        }
    }
    public void ReadSnakeControll(){
        ConsoleKey? latestKey = null;
        while (Console.KeyAvailable)
        {
            latestKey = Console.ReadKey(true).Key;
        }
        if (latestKey.HasValue){
            switch (latestKey)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow: Notify(Event.SnakeTurn, Vector.Up); break;
                case ConsoleKey.S or ConsoleKey.DownArrow: Notify(Event.SnakeTurn, Vector.Down); break;
                case ConsoleKey.A or ConsoleKey.LeftArrow: Notify(Event.SnakeTurn, Vector.Left); break;
                case ConsoleKey.D or ConsoleKey.RightArrow: Notify(Event.SnakeTurn, Vector.Right); break;
                default: break;
            }
        }
    }

    public void Notify(Event eventType, IObservable publisher)
    {
        _events.Notify(eventType, publisher);
    }
    public void Notify(Event eventType, (Menu menu, int hover) args)
    {
        _events.Notify(eventType, args);
    }
    public void Notify(Event eventType, Menu menu)
    {
        _events.Notify(eventType, menu);
    }

    public void Notify(Event eventType, Vector vector){
        _events.Notify(eventType, vector);
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