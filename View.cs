using System.Collections.Specialized;


public class View : IObservable
{
    private Menu _mainMenu;
    private Menu _settingsMenu;
    private Settings _settings;
    private Input _input;
    private EventManager _events;
    private Field _backgroundField;
    public EventManager Events { get => _events; set { _events = value; } }

    public View(EventManager events, Input input, Field field, Settings settings)
    {
        _events = events;
        _input = input;
        _backgroundField = field;
        _settings = settings;

        _mainMenu = new Menu(new OrderedDictionary<string, Action>(){
            { "New game", NewGame},
            { "Records", Records},
            { "Settings", EditSettings},
            { "Exit", Exit}});
        _settingsMenu = new Menu(new OrderedDictionary<string, Action>(){
            {"Game size", SetGameSize},
            {"Speed", SetSpeed},
            {"Back", Start}
        });


    }
    public void DisplayField(Field field)
    {
        Console.SetCursorPosition(0, 2);
        Console.WriteLine(field.Render());
    }
    public void NewGame()
    {
        Console.Clear();
        Notify(Event.NewGame);
    }
    public void Continue(){
        _mainMenu.Options.Remove("Continue");
        Notify(Event.Continue);
    }
    public void OnPause(){
        _mainMenu.Options.Insert(0, "Continue", Continue);
        DisplayMenu(_mainMenu);
        _input.ReadMenuOption(_mainMenu);
    }
    public void Records() { }
    public void EditSettings()
    {
        DisplayMenu(_settingsMenu);
         _input.ReadMenuOption(_settingsMenu);
    }
    public void SetGameSize()
    {
        var _gameSizeMenu = new Menu(new OrderedDictionary<string, Action>{
            {"NORMAL", () => {_settings.GameSize = (int)GameSizes.Normal; EditSettings();}},
            {"MEDIUM", () => {_settings.GameSize = (int)GameSizes.Medium; EditSettings();}},
            {"BIG", () => {_settings.GameSize = (int)GameSizes.Big; EditSettings();}},
            {"Back", EditSettings},
        });
        DisplayMenu(_gameSizeMenu);
        _input.ReadMenuOption(_gameSizeMenu);
    }
    public void SetSpeed(){
        var _gameSpeedMenu = new Menu(new OrderedDictionary<string, Action>{
            {"SLOW", () => {_settings.Speed = (int)Speeds.Slow; EditSettings();}},
            {"NORMAL", () => {_settings.Speed = (int)Speeds.Normal; EditSettings();}},
            {"MODERATE", () => {_settings.Speed = (int)Speeds.Moderate; EditSettings();}},
            {"FAST", () => {_settings.Speed = (int)Speeds.Fast; EditSettings();}},
            {"Back", EditSettings},
        });
        DisplayMenu(_gameSpeedMenu);
        _input.ReadMenuOption(_gameSpeedMenu);
    }
    public void Exit()
    {
        Console.SetCursorPosition(0, _settings.GameSize + 5);
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
    public void DisplayGameOver(int score)
    {
        DisplayBackground();
        int windowHeight = 7;
        int windowWidth = 17;
        int consoleWidth = _settings.GameSize * 2;
        int consoleHeight = _settings.GameSize;

        // Calculate the starting position to center the entire box
        (int boxStartX, int boxStartY) = CalculateCenteredPosition(consoleWidth, consoleHeight, windowWidth, windowHeight);
        string gameOverText = $"  Game Over  \nYour score: {score}";
        DisplayBoxWithCenteredContent(windowHeight, windowWidth, boxStartX, boxStartY, gameOverText);
    }
    public (int startX, int startY) CalculateCenteredPosition(int containerWidth, int containerHeight, int contentWidth, int contentHeight)
    {
        int startX = containerWidth > contentWidth ? (containerWidth - contentWidth) / 2 +1: 0;
        int startY = containerHeight > contentHeight ? (containerHeight - contentHeight) / 2 +2: 0;
        return (startX, startY);
    }
    public void DisplayBox(int height, int width, int startX, int startY)
    {
        Console.SetCursorPosition(startX, startY);
        Console.WriteLine("┌" + new string('─', width - 2) + "┐"); // Upper line

        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            Console.WriteLine("│" + new string(' ', width - 2) + "│");
        }

        Console.SetCursorPosition(startX, startY + height - 1);
        Console.WriteLine("└" + new string('─', width - 2) + "┘"); // Lower line
    }
    public void DisplayBoxWithCenteredContent(int height, int width, int startX, int startY, string content)
    {
        DisplayBox(height, width, startX, startY); // Draw the box

        string[] lines = content.Split('\n');
        int contentHeight = lines.Length;

        int contentStartY = startY + (height - contentHeight) / 2;

        for (int i = 0; i < contentHeight; i++)
        {
            int contentStartX = startX + (width - lines[i].Length) / 2;
            Console.SetCursorPosition(contentStartX, contentStartY + i);
            Console.WriteLine(lines[i]);
        }
    }
    public void DisplayMenu(Menu menu)
    {
        DisplayBackground();

        var options = menu.Options.Keys.ToList();
        if (options.Count == 0)
        {
            return; // Nothing to display
        }

        var longestOption = options.OrderByDescending(key => key.Length).FirstOrDefault()?.Length ?? 0;
        int totalMenuLines = options.Count;

        // Calculate vertical center
        int gameHeight = _settings.GameSize;
        int menuHeight = totalMenuLines; // Each option takes one line
        int startY = Math.Max(0, (gameHeight - menuHeight/2) / 2);

        // Calculate horizontal center
        int gameWidth = _settings.GameSize * 2;
        int menuWidth = longestOption + 2; // Add 2 for the "> " or " " prefix
        int startX = Math.Max(0, (gameWidth - menuWidth) / 2);
        for (int i = 0; i < menu.Options.Keys.Count; i++)
        {
            Console.SetCursorPosition(startX, startY+i);
            if (i == menu.Selected)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($">{menu.Options.Keys.ElementAt(i)}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($" {menu.Options.Keys.ElementAt(i)}");
            }
        }
    }
    public void DisplayBackground(){
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        DisplayField(_backgroundField);
        Console.ResetColor();
    }
    public void Start()
    {
        DisplayMenu(_mainMenu);
        _input.ReadMenuOption(_mainMenu);
    }

    public void Subscribe(Event eventType, EventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }

    public void Notify(Event eventType, object? args = null)
    {
        _events.Notify(eventType, args);
    }

}