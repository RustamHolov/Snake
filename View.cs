using System.Text;


public class View : IObserver, IObservable
{
    private Dictionary<string, Action> _mainMenu;
    private List<IObserver> _subscribers;
    private EventManager _events;
    public EventManager Events {get => _events; set { _events = value;}}

    public View()
    {
        _mainMenu = new Dictionary<string, Action>
        {
            {"New game", NewGame},
            {"Records", Records},
            {"Settings", Settings},
            {"Exit", Exit}
        };
        _subscribers = new List<IObserver>();
        _events = new EventManager();
    }
    public void DisplayField(Field field){
        Console.SetCursorPosition(0, 2);
        Console.WriteLine(field.Render());
    }
    public void NewGame(){
        Console.Clear();
        Notify(Event.NewGame);
    }
    public void Records(){}
    public void Settings(){
        DisplayMenu(new Dictionary<string, Action>(){
            {"Game size", SetGameSize},
            {"Back", Game}
        });
    }
    public void SetGameSize(){
        DisplayMenu(new Dictionary<string, Action>(){
            {"Normal", () =>  Notify(GameSize.Normal)},
            {"Medium", () => Notify(GameSize.Medium)},
            {"Big", () => Notify(GameSize.Big)},
            {"Back", Settings}
        });
    }
    public void Exit(){
        Environment.Exit(0);
    }
    public void DisplaySnakeInfo(SnakeModel snake){
        Console.SetCursorPosition(0,0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Score: {snake.FoodEated}");
        Console.ResetColor();
    }
    public void HoverMenuElement(Dictionary<string, Action> menu, int hoverOption){
        Console.Clear();
        for (int i = 0; i < menu.Keys.Count; i++)
        {
            if(i == hoverOption){
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($">{menu.Keys.ElementAt(i)}");
                Console.ResetColor();
            }
            else{
                Console.WriteLine($"{menu.Keys.ElementAt(i)}");
            }
        }

    }
    public void DisplayMenu(Dictionary<string, Action> menu){
        int hover = 0;
        Console.Clear();
        for(int i = 0; i < menu.Keys.Count; i++){
            Console.WriteLine($"{menu.Keys.ElementAt(i)}");
        }
        HoverMenuElement(menu, hover);
        while(true){
            if (Console.KeyAvailable)
            {
                 switch(Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter: menu.TryGetValue(menu.Keys.ElementAt(hover), out Action? action); action?.Invoke(); break;
                    case ConsoleKey.W or ConsoleKey.UpArrow : HoverMenuElement(menu, hover > 0 ? --hover : hover = menu.Keys.Count - 1 ); break;
                    case ConsoleKey.S or ConsoleKey.DownArrow : HoverMenuElement(menu, hover < menu.Keys.Count - 1 ? ++hover : hover = 0); break;
                    default : break;
                };
            }
        }
    }
    public void Game(){
        DisplayMenu(_mainMenu);
    }
    public void Update(IObservable publisher)
    {
        if (publisher is Field field)
        {
            DisplayField(field);
        }
        else if (publisher is SnakeModel snake)
        {
            DisplaySnakeInfo(snake);
        }
    }

    public void Subscribe(IObserver subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unscribe(IObserver subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public void Notify()
    {
        foreach(var subscriber in _subscribers){
            subscriber.Update(this);
        }
    }
    public void Notify(Event eventType){
        _events.Notify(eventType, this);
    }
    public void Notify(GameSize size){
        _events.Notify(Event.Size, size);
    }
}