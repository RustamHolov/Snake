using System.Diagnostics;

public class Controller
{
    private Settings _settings;
    private View _view;
    private SnakeModel _snake;
    private Field _field;
    private Input _input;
    private DataBase _db;
    public Settings Settings { get => _settings; }
    public bool GameRuning { get; set; } = true;
    private bool _firstRun = true;
    public bool FirstRun { get => _firstRun; }
    public Controller(View view, SnakeModel snake, Field field, Input input, Settings settings, DataBase dataBase)
    {
        _view = view;
        _snake = snake;
        _field = field;
        _input = input;
        _settings = settings;
        _db = dataBase;
    }
    public void WelcomeMenu()
    {
        _view.Start();
    }
    public void GameLoop()
    {
        _firstRun = false;
        _settings.GameState = GameState.Game;
        _view.DisplaySnakeInfo(_snake);
        var (Name, HighestValue) = CalculateBestScore();
        _view.DisplayHighestScore(Name, HighestValue);
        _view.DisplayField(_field);
        Stopwatch stopwatch = new Stopwatch();
        long lastUpdate = 0;
        int moveInterval = _settings.Speed; // milliseconds between moves

        stopwatch.Start();
        while (GameRuning)
        {
            long elapsed = stopwatch.ElapsedMilliseconds;
            _input.ReadSnakeControll();
            // Only move the snake when enough time has passed
            if (elapsed - lastUpdate >= moveInterval)
            {
                _snake.Move();
                lastUpdate = elapsed;
            }
            Thread.Sleep(1);
        }
    }
    public void GameOver()
    {
        _field.GetRidOfSnake();
        Thread.Sleep(200); //delay before showing game over message
        var rate = GetRank(_snake.FoodEated);
        _view.DisplayGameOver(_snake.FoodEated, rate);
    }
    private (int rank, int recordsCount) GetRank(int score)
    {
        var ranks = new List<int>();
        _db.Records.ToList().ForEach(pair => pair.Value.ForEach(score => ranks.Add(score)));
        ranks.Add(score);
        ranks = ranks.OrderByDescending(rank => rank).ToList();
        return (ranks.LastIndexOf(score), ranks.Count);
    }
    public void SaveRecord()
    {
        string name = _view.DisplaySaveWindowAndGetName(_snake.FoodEated);
        _db.AddRecord(name, _snake.FoodEated);
        if (_db.SaveToCSV())
        {
            _view.DisplayBoxWithCenteredMessage("Saved successfully!");
        }
        else
        {
            _view.DisplayBoxWithCenteredMessage("Fail saving...");
        }
    }
    public (string Name, int HighestValue) CalculateBestScore()
    {
        var records = _db.Records;
        if (records == null || !records.Any())
        {
            return ("", int.MinValue);
        }

        string? keyWithMax = null;
        int maxRecord = int.MinValue;

        foreach (var pair in records)
        {
            foreach (int record in pair.Value)
            {
                if (record > maxRecord)
                {
                    maxRecord = record;
                    keyWithMax = pair.Key;
                }
            }
        }
        return (keyWithMax ?? string.Empty, maxRecord);
    }
    public void ViewLeaderboard(int page = 0)
    {
        var records = _db.Records;

        if (records == null || !records.Any())
            throw new Exception("No records provided");

        var leaderboard = new List<KeyValuePair<string, int>>();

        foreach (var pair in records)
        {
            foreach (int score in pair.Value)
            {
                leaderboard.Add(new KeyValuePair<string, int>(pair.Key, score));
            }
        }

        var sorted = leaderboard.OrderByDescending(entry => entry.Value).ToList();
        if (page != 0)
        {
            _view.DisplayRecords(sorted, page);
        }
        else
        {
            _view.DisplayRecords(sorted);
        }
    }
}