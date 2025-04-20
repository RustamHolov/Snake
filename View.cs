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
        Notify(Event.Continue);
    }
    public void OnPause(){
        var extendetMainMenu = _mainMenu;
        extendetMainMenu.Options.Insert(0, "Continue", Continue);
        DisplayMenu(extendetMainMenu);
    }
    public void Records() { }
    public void EditSettings()
    {
        DisplayMenu(_settingsMenu);
        while (true)
        {
            _input.ReadMenuOption(_settingsMenu);
        }
    }
    public void SetGameSize()
    {
        var _gameSizeMenu = new Menu(new OrderedDictionary<string, Action>{
            {"Normal", () => {_settings.GameSize = (int)GameSizes.Normal; EditSettings();}},
            {"Medium", () => {_settings.GameSize = (int)GameSizes.Medium; EditSettings();}},
            {"Big", () => {_settings.GameSize = (int)GameSizes.Big; EditSettings();}},
        });
        DisplayMenu(_gameSizeMenu);
        while (true){
            _input.ReadMenuOption(_gameSizeMenu);
        }
    }
    public void SetSpeed(){
        var _gameSpeedMenu = new Menu(new OrderedDictionary<string, Action>{
            {"Slow", () => {_settings.Speed = (int)Speeds.Slow; EditSettings();}},
            {"Normal", () => {_settings.Speed = (int)Speeds.Normal; EditSettings();}},
            {"Moderate", () => {_settings.Speed = (int)Speeds.Moderate; EditSettings();}},
            {"Fast", () => {_settings.Speed = (int)Speeds.Fast; EditSettings();}},
        });
        DisplayMenu(_gameSpeedMenu);
        while (true)
        {
            _input.ReadMenuOption(_gameSpeedMenu);
        }
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
    public void DisplayGameOver(int score){
        DisplayBackground();
        int windowHeight = 5; // Increased height to accommodate "Game Over" and score
        int windowWidth = 15; // Increased width for better readability
        int consoleWidth = _settings.GameSize * 2;
        int consoleHeight = _settings.GameSize;

        // Calculate the starting position to center the entire box
        int startX = Math.Max(0, (consoleWidth - windowWidth) / 2);
        int startY = Math.Max(0, (consoleHeight - windowHeight/2) / 2);

        // Draw the box
        Console.SetCursorPosition(startX, startY);
        Console.WriteLine("┌" + new string('─', windowWidth - 2) + "┐"); // Upper line

        // Draw the middle lines
        for (int i = 1; i < windowHeight - 1; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            Console.WriteLine("│" + new string(' ', windowWidth - 2) + "│");
        }

        Console.SetCursorPosition(startX, startY + windowHeight - 1);
        Console.WriteLine("└" + new string('─', windowWidth - 2) + "┘"); // Lower line

        // Calculate the position to center the "Game Over" text
        string gameOverText = "Game Over";
        int gameOverTextX = startX + (windowWidth - gameOverText.Length) / 2;
        int gameOverTextY = startY + 1; // Position it one row below the top

        Console.SetCursorPosition(gameOverTextX, gameOverTextY);
        Console.WriteLine(gameOverText);

        // Calculate the position to center the score
        string scoreText = $"Score: {score}";
        int scoreTextX = startX + (windowWidth - scoreText.Length) / 2;
        int scoreTextY = startY + 3; // Position it one row below "Game Over"

        Console.SetCursorPosition(scoreTextX, scoreTextY);
        Console.WriteLine(scoreText);
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

    public void Notify(Event eventType, object? args = null)
    {
        _events.Notify(eventType, args);
    }

}