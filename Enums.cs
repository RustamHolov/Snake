public enum Vector
{
    Left,
    Right,
    Up,
    Down,
    NotMoving
}
public enum Event{
    Eat,
    Move,
    NewGame,
    Size,
    Place,
    MenuHover,
    MenuSelect
}
public enum GameSize{
    Normal = 20,
    Medium = 30,
    Big = 40
}

public enum CellSize{
    Normal = 1, 
    Medium = 2,
    Big = 3
}

public enum Speed{
    Slow = 120,
    Normal = 80,
    Moderate = 60,
    Fast = 40
}