using System.Collections.Specialized;


public class View : IObservable
{
    private Menu _mainMenu;
    private Menu _settingsMenu;
    private Menu _gameOverMenu;
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

        _gameOverMenu = new Menu(new OrderedDictionary<string, Action>(){
            {"New game", NewGame},
            {"Save score", Save},
            {"Main Menu", Start},
            {"Exit", Exit}
        });


    }
    public void DisplayField(Field field)
    {
        Console.SetCursorPosition(0, 2);
        Console.WriteLine(field.Render());
    }
    public void Save() { 
        Notify(Event.Save);
    }
    public void NewGame()
    {
        Console.Clear();
        Notify(Event.NewGame);
    }
    public void Continue()
    {
        _mainMenu.Remove("Continue");
        Notify(Event.Continue);
    }
    public void OnPause()
    {
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
    public void SetSpeed()
    {
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

        Console.SetCursorPosition(0, _settings.GameSize + 8);
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
    public void DisplaRecord(string name, int record)
    {
        int scoreLineLength = name.Length + 6 + $"{record}".Length;
        Console.SetCursorPosition(_settings.GameSize * 2 - scoreLineLength, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Top♔ - {name}:{record}");
        Console.ResetColor();
    }
    private void DrawBox(int x, int y, int width, int height)
    {
        Console.SetCursorPosition(x, y);
        Console.WriteLine("┌" + new string('─', width - 2) + "┐");

        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.WriteLine("│" + new string(' ', width - 2) + "│");
        }

        Console.SetCursorPosition(x, y + height - 1);
        Console.WriteLine("└" + new string('─', width - 2) + "┘");
    }

    private void DisplayCenteredText(string[] lines, int boxStartX, int boxStartY, int boxWidth, int boxHeight)
    {
        int contentStartY = boxStartY + (boxHeight - lines.Length) / 2;

        for (int i = 0; i < lines.Length; i++)
        {
            int contentStartX = boxStartX + (boxWidth - lines[i].Length) / 2;
            Console.SetCursorPosition(contentStartX, contentStartY + i);
            Console.WriteLine(lines[i]);
        }
    }

    private string[] BuildGameOverMessage(int score)
    {
        return
        [
        "  Game Over  ",
        "",
        $"Your score: {score}"
    ];
    }
    private string[] BuildEnterNewName(int score){
        return 
        [
            "Your name:",
            "",
            "",
            $"Score: {score}"
        ];
    }
    public string DisplaySaveWindow(int score){
        DisplayBackground();
        const int WindowHeight = 7;
        const int WindowWidth = 30;
        int consoleWidth = _settings.GameSize * 2;
        int consoleHeight = _settings.GameSize;

        // Get box top-left corner
        (int boxStartX, int boxStartY) = CalculateCenteredPosition(consoleWidth, consoleHeight, WindowWidth, WindowHeight);
        DrawBox(boxStartX, boxStartY, WindowWidth, WindowHeight);

        var lines = BuildEnterNewName(score);
        DisplayCenteredText(lines, boxStartX, boxStartY, WindowWidth, WindowHeight);

        // Find coordinates for text input (after "Your name:")
        //string label = "Your name:";
        int labelLineIndex = 1; // first line
        int labelStartX = boxStartX + 1;
        int inputX = labelStartX + (WindowWidth - lines.Length) / 4 ;
        int inputY = boxStartY + ((WindowHeight - lines.Length) / 2) + labelLineIndex;
        string name = _input.ReadNameInput(inputX, inputY);
        return name;
    }
    public void DisplayBoxWithCenteredMessage(string message){
        DisplayBackground();

        const int WindowHeight = 7;
        const int WindowWidth = 30;
        int consoleWidth = _settings.GameSize * 2;
        int consoleHeight = _settings.GameSize;

        (int boxStartX, int boxStartY) = CalculateCenteredPosition(consoleWidth, consoleHeight, WindowWidth, WindowHeight);

        DrawBox(boxStartX, boxStartY, WindowWidth, WindowHeight);
        DisplayCenteredText([message], boxStartX, boxStartY, WindowWidth, WindowHeight);
        DisplayHorizontalMenu(_gameOverMenu);
        _input.ReadHorisontalMenuOption(_gameOverMenu);
    }
    public void DisplayGameOver(int score)
    {
        DisplayBackground();

        const int WindowHeight = 7;
        const int WindowWidth = 30;
        int consoleWidth = _settings.GameSize * 2;
        int consoleHeight = _settings.GameSize;

        (int boxStartX, int boxStartY) = CalculateCenteredPosition(consoleWidth, consoleHeight, WindowWidth, WindowHeight);

        DrawBox(boxStartX, boxStartY, WindowWidth, WindowHeight);
        DisplayCenteredText(BuildGameOverMessage(score), boxStartX, boxStartY, WindowWidth, WindowHeight);
        DisplayHorizontalMenu(_gameOverMenu);
        _input.ReadHorisontalMenuOption(_gameOverMenu);
    }
    public (int startX, int startY) CalculateCenteredPosition(int containerWidth, int containerHeight, int contentWidth, int contentHeight)
    {
        int startX = containerWidth > contentWidth ? (containerWidth - contentWidth) / 2 + 1 : 0;
        int startY = containerHeight > contentHeight ? (containerHeight - contentHeight) / 2 + 2 : 0;
        return (startX, startY);
    }

    public void DisplayHorizontalMenu(Menu menu)
    {
        _settings.GameState = GameState.Menu;

        var optionKeys = menu.Options.Keys.ToList();
        if (optionKeys.Count == 0) return;

        int totalOptionsLength = optionKeys.Sum(key => key.Length);
        int gameHeight = _settings.GameSize;
        int gameWidth = _settings.GameSize * 2;
        int startY = gameHeight + 5;

        int spacing = Math.Max(1, (gameWidth - totalOptionsLength) / optionKeys.Count);
        int startX = 1;

        // Clear the line before drawing
        Console.SetCursorPosition(0, startY);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(startX, startY);

        for (int i = 0; i < optionKeys.Count; i++)
        {
            string prefix = i == menu.Selected ? ">" : " ";
            string optionText = optionKeys[i];

            if (i == menu.Selected) Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"{prefix}{optionText}{new string(' ', spacing)}");

            if (i == menu.Selected) Console.ResetColor();
        }

    }
    public void DisplayMenu(Menu menu)
    {
        _settings.GameState = GameState.Menu;
        DisplayBackground();

        var options = menu.Options.Keys.ToList();
        if (options.Count == 0) return;

        int longestOption = options.Max(key => key.Length);
        int menuHeight = options.Count;

        int gameHeight = _settings.GameSize + 2;
        int gameWidth = _settings.GameSize * 2;

        int menuWidth = longestOption + 2; // space for "> "
        int startY = Math.Max(0, (gameHeight - menuHeight) / 2);
        int startX = Math.Max(0, (gameWidth - menuWidth) / 2);

        for (int i = 0; i < options.Count; i++)
        {
            string prefix = i == menu.Selected ? ">" : " ";
            string optionText = options[i];

            Console.SetCursorPosition(startX, startY + i);

            if (i == menu.Selected)
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"{prefix}{optionText}");

            if (i == menu.Selected)
                Console.ResetColor();
        }
    }
    public void DisplayBackground()
    {
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