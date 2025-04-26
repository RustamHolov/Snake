using System.Collections.Specialized;


public class View : IObservable
{
    private readonly Menu _mainMenu;
    private readonly Menu _settingsMenu;
    private readonly Menu _gameOverMenu;
    private readonly Menu _pauseMenu;
    private readonly Settings _settings;
    private readonly Input _input;
    private readonly EventManager _events;
    private readonly Field _backgroundField;
    private int _currentLeaderboardPage = 0;
    public EventManager Events { get => _events; }

    public View(EventManager events, Input input, Field field, Settings settings)
    {
        _events = events;
        _input = input;
        _backgroundField = field;
        _settings = settings;

        _mainMenu = new Menu(new OrderedDictionary<string, Action>(){
            { "New game", NewGame},
            { "Rating", Rating},
            { "Settings", EditSettings},
            { "Exit", Exit}});
        _settingsMenu = new Menu(new OrderedDictionary<string, Action>(){
            { "Game size", SetSize},
            { "Speed", SetSpeed},
            { "Back", Back}
        });
        _gameOverMenu = new Menu(new OrderedDictionary<string, Action>(){
            { "New game", NewGame},
            { "Save score", Save},
            { "Main Menu", Start},
            { "Exit", Exit}
        });
        _pauseMenu = new Menu(new OrderedDictionary<string, Action>(){
            { "Continue", Continue},
            { "New game", NewGame},
            { "Rating", Rating},
            { "Settings", EditSettings},
            { "Main Menu", Start},
            { "Exit", Exit}});
    }

    #region MenuPublicMethods
    public void Start()
    {
        DisplayMenu(_mainMenu);
        _input.ReadMenuOption(_mainMenu);
    }
    public void NewGame()
    {
        Console.Clear();
        Notify(Event.NewGame);
    }
    public void Continue()
    {
        Notify(Event.Continue);
    }
    public void OnPause()
    {
        DisplayMenu(_pauseMenu);
        _input.ReadMenuOption(_pauseMenu);
    }
    public void Rating()
    {
        Notify(Event.Rating);
    }
    public void Exit()
    {
        Console.SetCursorPosition(0, 0);
        Environment.Exit(0);
    }
    public void InvokeAction(Menu menu)
    {
        menu.GetSelectedAction().Invoke();
    }
    #endregion

    #region DisplayPublicMethods
    public void DisplayField(Field field)
    {
        Console.SetCursorPosition(0, 2);
        Console.WriteLine(field.Render());
    }
    public void DisplayHorizontalMenu(Menu menu)
    {
        if (_settings.GameState != GameState.Pause)
        {
            _settings.GameState = GameState.Menu;
        }
        var optionKeys = menu.Options.Keys.ToList();
        if (optionKeys.Count == 0) return;

        int totalOptionsLength = optionKeys.Sum(key => key.Length);
        int gameHeight = _settings.GameSize;
        int gameWidth = _settings.GameSize * 2;
        int startY = gameHeight + 4;

        int spacing = Math.Max(1, (gameWidth - totalOptionsLength) / optionKeys.Count);
        int startX = 1;

        // Clear the line before drawing
        Console.SetCursorPosition(0, startY);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(startX, startY);

        for (int i = 0; i < optionKeys.Count; i++)
        {
            string prefix = i == menu.Selected ? "▸" : " ";
            string optionText = optionKeys[i];

            if (i == menu.Selected) Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"{prefix}{optionText}{new string(' ', spacing)}");

            if (i == menu.Selected) Console.ResetColor();
        }

    }
    public void DisplayMenu(Menu menu)
    {
        if(_settings.GameState != GameState.Pause){
            _settings.GameState = GameState.Menu;
        }
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
            string prefix = i == menu.Selected ? "▸" : " ";
            string optionText = options[i];

            Console.SetCursorPosition(startX, startY + i);

            if (i == menu.Selected)
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"{prefix}{optionText}");

            if (i == menu.Selected)
                Console.ResetColor();
        }
    }
    public void DisplaySnakeInfo(SnakeModel snake)
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Score: {snake.FoodEated}");
        Console.ResetColor();
    }
    public void DisplayHighestScore(string name, int record)
    {
        int scoreLineLength = name.Length + 6 + $"{record}".Length;
        Console.SetCursorPosition(_settings.GameSize * 2 - scoreLineLength, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Top♔ - {name}:{record}");
        Console.ResetColor();
    }
    public void DisplayRecords(List<KeyValuePair<string, int>> leaderboard, int pageDelta = 0)
    {
        var backMenu = new Menu(new OrderedDictionary<string, Action>
    {
        { "Back", Back },
    });

        DisplayBackground();

        string title = "♔ Leaderboard ♔";
        int gameHeight = _settings.GameSize;
        int gameWidth = _settings.GameSize * 2;

        DisplayTitle(title, gameWidth, y: 3);

        int recordsStartY = 5;
        int paddingX = 5;
        int maxWidth = gameWidth - (paddingX * 2);
        int maxHeight = gameHeight - 4 - 2;

        int totalPages = (int)Math.Ceiling((double)leaderboard.Count / maxHeight);
        _currentLeaderboardPage = Math.Clamp(_currentLeaderboardPage + pageDelta, 0, totalPages);

        AddPaginationOptions(backMenu, _currentLeaderboardPage, totalPages);

        int start = _currentLeaderboardPage * maxHeight;
        int take = Math.Min(maxHeight, leaderboard.Count - start);
        var pageItems = leaderboard.GetRange(start, take);

        // Prepare full display list with true rank info
        List<(int Rank, KeyValuePair<string, int> Entry)> displayItems = [];

        for (int i = 0; i < pageItems.Count; i++)
        {
            displayItems.Add((start + i + 1, pageItems[i]));
        }

        // Add divider and last record only on non-final pages
        if (_currentLeaderboardPage < totalPages - 1)
        {
            displayItems.Add((-1, new KeyValuePair<string, int>("...", 0))); // divider
            var lastEntry = leaderboard.Last();
            int lastRank = leaderboard.Count;
            displayItems.Add((lastRank, lastEntry));
        }

        PageViewWithRanks(paddingX, recordsStartY, maxWidth, displayItems);

        DisplayHorizontalMenu(backMenu);
        _input.ReadHorisontalMenuOption(backMenu);
    }
    public void DisplayGameOver(int score, (int rank, int recordsCount) rate)
    {
        DisplayBackground();

        const int WindowHeight = 7;
        const int WindowWidth = 30;
        int consoleWidth = _settings.GameSize * 2;
        int consoleHeight = _settings.GameSize;

        (int boxStartX, int boxStartY) = CalculateCenteredPosition(consoleWidth, consoleHeight, WindowWidth, WindowHeight);

        DrawBox(boxStartX, boxStartY, WindowWidth, WindowHeight);
        DisplayCenteredText(BuildGameOverMessage(score, rate.rank, rate.recordsCount), boxStartX, boxStartY, WindowWidth, WindowHeight);
        DisplayHorizontalMenu(_gameOverMenu);
        _input.ReadHorisontalMenuOption(_gameOverMenu);
    }
    public string DisplaySaveWindowAndGetName(int score)
    {
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
        int inputX = labelStartX + (WindowWidth - lines.Length) / 4;
        int inputY = boxStartY + ((WindowHeight - lines.Length) / 2) + labelLineIndex;
        string name = _input.ReadNameInput(inputX, inputY);
        return name;
    }
    public void DisplayBoxWithCenteredMessage(string message)
    {
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
    #endregion

    #region PrivateHelperMethods
    private void DisplayBackground()
    {
        //Clear header
        Console.SetCursorPosition(0, 0);
        Console.Write(new string(' ', _settings.GameSize * 2 + 10));
        Console.SetCursorPosition(0, 1);
        Console.Write(new string(' ', _settings.GameSize * 2 + 10));

        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        DisplayField(_backgroundField);
        Console.ResetColor();
        //Clear footer
        Console.SetCursorPosition(0, _settings.GameSize + 4);
        Console.Write(new string(' ', _settings.GameSize * 2 + 10));
        Console.SetCursorPosition(0, _settings.GameSize + 5);
        Console.Write(new string(' ', _settings.GameSize * 2 + 10));
    }
    private void DisplayTitle(string title, int width, int y)
    {
        int x = (width - title.Length) / 2;
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(title);
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
    private void PageViewWithRanks(int x, int y, int maxWidth, List<(int Rank, KeyValuePair<string, int> Entry)> rankedEntries)
    {
        for (int i = 0; i < rankedEntries.Count; i++)
        {
            var (rank, entry) = rankedEntries[i];

            Console.SetCursorPosition(x, y + i);

            if (rank == -1)
            {
                Console.WriteLine(new string('.', 3)); // Divider line 
                continue;
            }

            string name = entry.Key;
            int score = entry.Value;
            string rankName = $"{rank}. {name}";
            string line = rankName + new string('.', maxWidth - score.ToString().Length - rankName.Length) + score;
            Console.WriteLine(line);
        }
    }

    private void AddPaginationOptions(Menu menu, int currentPage, int totalPages)
    {
        if (currentPage > 0)
            menu.Add("Previous page", PreviousPage);

        if (currentPage < totalPages - 1)
            menu.Add("Next page", NextPage);
    }
    private void DisplayCenteredText(string[] lines, int boxStartX, int boxStartY, int boxWidth, int boxHeight)
    {
        int contentStartY = boxStartY + (boxHeight - lines.Length) / 2;

        for (int i = 0; i < lines.Length; i++)
        {
            int contentStartX = boxStartX + (boxWidth - lines[i].Length) / 2;
            Console.SetCursorPosition(contentStartX, contentStartY + i);
            if (lines[i].Trim().Contains("Game Over", StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(lines[i]);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine(lines[i]);
            }
        }
    }

    private string[] BuildGameOverMessage(int score, int rank, int recordsCount)
    {
        return
        [
        "  Game Over  ",
        "",
        $"Your score: {score}",
        $"Your rank: {rank}/{recordsCount}"
    ];
    }
    private string[] BuildEnterNewName(int score)
    {
        return
        [
            "Enter your name:",
            "",
            "",
            $"Score: {score}"
        ];
    }
    private (int startX, int startY) CalculateCenteredPosition(int containerWidth, int containerHeight, int contentWidth, int contentHeight)
    {
        int startX = containerWidth > contentWidth ? (containerWidth - contentWidth) / 2 + 1 : 0;
        int startY = containerHeight > contentHeight ? (containerHeight - contentHeight) / 2 + 2 : 0;
        return (startX, startY);
    }

    #endregion

    #region MenuPrivateMethods
    private void SetSpeed()
    {
        void SetGameSpeed(Speeds speed)
        {
            _settings.Speed = (int)speed;
            SetSpeed(); //to redraw
        }

        string GetLabel(Speeds speed) =>
            _settings.Speed == (int)speed ? $"● {speed}" : speed.ToString();

        var speedMenu = new Menu(new OrderedDictionary<string, Action> {
        { GetLabel(Speeds.Slow),     () => SetGameSpeed(Speeds.Slow) },
        { GetLabel(Speeds.Normal),   () => SetGameSpeed(Speeds.Normal) },
        { GetLabel(Speeds.Moderate), () => SetGameSpeed(Speeds.Moderate) },
        { GetLabel(Speeds.Fast),     () => SetGameSpeed(Speeds.Fast) },
        { "Back",                    EditSettings },
        });

        DisplayMenu(speedMenu);
        _input.ReadMenuOption(speedMenu);
    }
    private void Back(){
        if(_settings.GameState == GameState.Pause){
            OnPause();
        }else{
            Start();
        }
    }
    private void Save()
    {
        Notify(Event.Save);
    }
    private void EditSettings()
    {
        DisplayMenu(_settingsMenu);
        _input.ReadMenuOption(_settingsMenu);
    }
    private void SetSize()
    {
        void SetGameSize(GameSizes size)
        {
            if (_mainMenu.Options.TryGetValue("Continue", out _)) _mainMenu.Remove("Continue"); //changing size unable continue 
            _settings.GameSize = (int)size;
            SetSize(); // to redraw
        }
        string GetLabel(GameSizes size) =>
            _settings.GameSize == (int)size ? $"● {size}" : size.ToString();

        var _gameSizeMenu = new Menu(new OrderedDictionary<string, Action>{
            {GetLabel(GameSizes.Normal), () => SetGameSize(GameSizes.Normal)},
            {GetLabel(GameSizes.Medium), () => SetGameSize(GameSizes.Medium)},
            {GetLabel(GameSizes.Big),    () => SetGameSize(GameSizes.Big)},
            {"Back",                      EditSettings},
        });

        DisplayMenu(_gameSizeMenu);
        _input.ReadMenuOption(_gameSizeMenu);

    }
    private void PreviousPage()
    {
        Notify(Event.Rating, -1);
    }
    private void NextPage()
    {
        Notify(Event.Rating, 1);
    }
    #endregion

}