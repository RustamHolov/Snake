using System.Text;


public class View : IObservable
{
    private Menu _mainMenu;
    private Input _input;
    private EventManager _events;
    public EventManager Events { get => _events; set { _events = value; } }

    public View(EventManager events, Input input)
    {
        _mainMenu = new Menu(new Dictionary<string, Action>(){
            { "New game", NewGame},
            { "Records", Records},
            { "Settings", Settings},
            { "Exit", Exit}});
        _events = events;
        _input = input;
    }
    public void DisplayField(Field field)
    {
        Console.SetCursorPosition(0, 2);
        Console.WriteLine(field.Render());
    }
    public void NewGame()
    {
        Console.Clear();
        Notify(Event.NewGame, this);
    }
    public void Records() { }
    public void Settings()
    {
        Menu settingMenu = new Menu(new Dictionary<string, Action>(){
            {"Game size", SetGameSize},
            {"Back", Start}
        });
        DisplayMenu(settingMenu);
        while (true)
        {
            _input.ReadMenuOption(settingMenu);
        }
    }
    public void SetGameSize()
    {
        // DisplayMenu(new Dictionary<string, Action>(){
        //     {"Normal", () =>  Notify(GameSize.Normal)},
        //     {"Medium", () => Notify(GameSize.Medium)},
        //     {"Big", () => Notify(GameSize.Big)},
        //     {"Back", Settings}
        // });
    }
    public void Exit()
    {
        Environment.Exit(0);
    }
    public void InvokeAction(Menu menu)
    {
        menu.GetSelectedAction().Invoke();
    }
    public void DisplaySnakeInfo(SnakeModel snake)
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Score: {snake.FoodEated}");
        Console.ResetColor();
    }
    public void DisplayMenu(Menu menu)
    {
        Console.Clear();
        for (int i = 0; i < menu.Options.Keys.Count; i++)
        {
            if (i == menu.Selected)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($">{menu.Options.Keys.ElementAt(i)}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"{menu.Options.Keys.ElementAt(i)}");
            }
        }
    }
    public void Start()
    {
        DisplayMenu(_mainMenu);
        while (true)
        {
            _input.ReadMenuOption(_mainMenu);
        }
    }

    public void Subscribe(Event eventType, EventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }

    public void Notify(Event eventType, IObservable publisher)
    {
        _events.Notify(eventType, publisher);
    }

}