using System.ComponentModel.DataAnnotations.Schema;

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
        _settings.GameState = GameState.Game;
        _view.DisplaySnakeInfo(_snake);
        var bestScore = CalculateBestScore();
        _view.DisplaRecord(bestScore.Name, bestScore.HighestValue);
        _view.DisplayField(_field);
        while (GameRuning)
        {
            _input.ReadSnakeControll();
            _snake.Move();
            Thread.Sleep(_settings.Speed);
        }
    }
    public void GameOver()
    {
        _field.GetRidOfSnake();
        _view.DisplayGameOver(_snake.FoodEated);
    }
    public void SaveRecord()
    {
        string name = _view.DisplaySaveWindow(_snake.FoodEated);
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
    public void Leaderboard()
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

        _view.DisplayRecords(sorted);
    }
}