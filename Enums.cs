public enum Vector
{
    Left,
    Right,
    Up,
    Down,
    NotMoving
}
public enum Event
{
    Eat,
    Move,
    NewGame,
    Size,
    Place,
    MenuHover,
    MenuSelect,
    SnakeTurn,
    Pause,
    Continue,
    GameOver,
    State,
    Save,
    Rating
}
public enum GameSizes
{
    Normal = 20,
    Medium = 30,
    Big = 35
}

public enum PixelSizes
{
    Normal = 1,
    Medium = 2,
    Big = 3
}

public enum Speeds
{
    Slow = 120,
    Normal = 80,
    Moderate = 60,
    Fast = 40
}

public enum GameState
{
    Menu,
    Game,
    Over,
    Pause
}