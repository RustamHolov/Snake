
public class DataBase
{
    private readonly string _titles = "Name,Score";
    private string _csvPath;
    private Dictionary<string, List<int>> _records = new();

    public IReadOnlyDictionary<string, List<int>> Records => _records;
    public bool Edited { get; private set; } = false;

    public DataBase(string path = "recordsDB.csv")
    {
        _csvPath = path;
        InitializeDataBase();
    }

    public void AddRecord(string name, int score)
    {
        if (!_records.TryGetValue(name, out var scores))
        {
            _records[name] = new List<int> { score };
        }
        else
        {
            scores.Add(score);
        }
        Edited = true;
    }

    public void DeleteUser(string name)
    {
        if (!_records.Remove(name))
        {
            throw new InvalidOperationException("User not found.");
        }
        Edited = true;
    }

    private void InitializeDataBase()
    {
        if (File.Exists(_csvPath))
            LoadFromCSV();
        else
            SaveToCSV();
    }

    public void LoadFromCSV()
    {
        _records.Clear();
        try
        {
            using var reader = new StreamReader(_csvPath);
            string[] headers = reader.ReadLine()?.Split(',') ?? throw new InvalidDataException("CSV missing header.");
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var (name, score) = ParseCsvLine(line);
                AddRecord(name, score);
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error loading from CSV: " + e.Message, e);
        }
    }

    private (string name, int score) ParseCsvLine(string line)
    {
        var parts = line.Split(',');
        if (parts.Length != 2 || !int.TryParse(parts[1], out var score))
            throw new FormatException($"Invalid line: {line}");
        return (parts[0], score);
    }

    public bool SaveToCSV()
    {
        try
        {
            using var sw = new StreamWriter(_csvPath);
            sw.WriteLine(_titles);
            foreach (var kvp in _records)
            {
                foreach (var score in kvp.Value)
                {
                    sw.WriteLine($"{kvp.Key},{score}");
                }
            }
            Edited = false;
            return true;
        }
        catch (Exception e)
        {
            throw new Exception("Error saving to CSV: " + e.Message, e);
        }
    }
}