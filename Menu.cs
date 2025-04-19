public class Menu{
    private int _selected;
    private Dictionary<string, Action> _options;

    public int Selected { get => _selected; set{_selected = value;}}
    public Dictionary<string, Action> Options {get => _options; set {_options = value;}}

    public Menu(){
        _options = new Dictionary<string, Action>();
    }
    public Menu(Dictionary<string, Action> options){
        _options = options;
    }

    public void Add(string name, Action action){
        if(!_options.ContainsKey(name) && action != null){
            _options.Add(name, action);
        }
        
    }
    public void Remove(string name){
        _options.Remove(name);
    }
    public void UpdateSelected(int count){
        _selected += count;
        if (_selected > _options.Keys.Count - 1){
            _selected = 0;
        }else if (_selected < 0){
            _selected = _options.Keys.Count - 1;
        }
    }
    public Action GetSelectedAction(){
        return _options.Values.ElementAt(_selected);
    }
}
