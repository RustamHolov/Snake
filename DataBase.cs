using System.Collections.ObjectModel;
using System.Text;

public class DataBase
{
    private Dictionary<string, List<int>> _records { get; set; } = new();
    public Dictionary<string, List<int>> Records {get => _records;}
    public bool edited = false;
    private readonly string _csvPath = "recordsDB.csv";
    private readonly string _titles = "Name,Score";
    public void AddRecord(string name, int score)
    {

        if(!_records.ContainsKey(name)){
            _records.Add(name, new List<int>{score});
        }else{
            _records.TryGetValue(name, out List<int>? scores);
            scores?.Add(score);
        }

    }
    public void DeleteUser(string name)
    {
        if (_records.ContainsKey(name))
        {
            _records.Remove(name);
        }
        else
        {
            throw new Exception("Member not found");
        }
    }

    public DataBase()
    {
        InitializeDataBase();
    }
    public DataBase(string path)
    {
        _csvPath = path;
        InitializeDataBase();
    }
    private void InitializeDataBase()
    {
        if (File.Exists(_csvPath))
        {
            LoadFromCSV();
        }
        else
        {
            _records = new ();
            SaveToCSV();
        }
    }
    public void LoadFromCSV()
    {
        _records.Clear();
        try
        {
            using (StreamReader reader = new StreamReader(_csvPath))
            {
                string[] titles = reader.ReadLine()?.Split(',') ?? throw new Exception("No header found");
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = line.Split(','); // getting values from entry
                    if (titles.Length != values.Length) throw new Exception("Something went wrong with tables");
                    if(int.TryParse(values[1], out int score)){
                        AddRecord(values[0], score);
                    }else{
                        throw new Exception("wrong data");
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error loading from CSV: " + e.Message);
        }
    }

    public void SaveToCSV()
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(_csvPath))
            {
                sw.WriteLine(_titles);
                foreach (var recordEntry in _records)
                {
                    string line;
                    string name = recordEntry.Key;
                    List<int> scores = recordEntry.Value;
                    string[] titlesArray = GetTitles(_titles);
                    foreach(int score in scores){
                        line = $"{name},{score}";
                        sw.WriteLine(line);
                    }
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception("Error saving to CSV: " + e.Message);
        }
    }

    public string[] GetTitles(string titles) => titles.Split(',');
}