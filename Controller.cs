public class Controller{
    private int _cellSize = 1;

    private readonly int Height = 20;
    private readonly int Width = 20;
    private  int GameSpeed = 80;
    private View _view;
    private SnakeModel _snake;
    private Field _field;

    private EatListener _eatListener;
    private MoveListener _moveListener;
    private NewGameListener _newGameListener;
    private SetSizeListener _setSizeListener;

    public Controller(View view){
        _view = view;
        _snake = new SnakeModel(_cellSize);
        _field = new Field(Height, Width,_cellSize, _snake);
        _eatListener = new EatListener(_view);
        _moveListener = new MoveListener(_field);
        _setSizeListener = new SetSizeListener(this);
        _newGameListener = new NewGameListener(this);
        _field.Subscribe(_view);
        _snake.Events.Subscribe(Event.Eat, _eatListener);
        _snake.Events.Subscribe(Event.Move, _moveListener);
        _view.Events.Subscribe(Event.NewGame, _newGameListener);
        _view.Events.Subscribe(Event.Size, _setSizeListener);
    }

    public void SetGameSize(GameSize size){
        _field = new Field((int)size, (int)size, _cellSize, _snake);
    }
    public void SetPixelSize(CellSize size){
        _snake = new SnakeModel((int)size);
        _field = new Field(_field.Height, _field.Width, (int)size, _snake);
    }
    public void SetSpeed(Speed speed){
        GameSpeed = (int)speed;
    }
    public void MainFlow(){
        _view.DisplaySnakeInfo(_snake);
        _view.DisplayField(_field);
        while(true){
            try{
                if (Console.KeyAvailable)
                {
                    _snake.Turn(Console.ReadKey(true).Key switch
                    {
                        ConsoleKey.W or ConsoleKey.UpArrow => Vector.Up,
                        ConsoleKey.S or ConsoleKey.DownArrow => Vector.Down,
                        ConsoleKey.A or ConsoleKey.LeftArrow => Vector.Left,
                        ConsoleKey.D or ConsoleKey.RightArrow => Vector.Right,
                        _ => Vector.Right
                    });
                }
                _snake.Move();
                Thread.Sleep(GameSpeed);
            }catch (Exception e){
                Console.WriteLine(e.Message);
                break;
            }
        }
        _field.Unscribe(_view);
        _snake.Events.Unscribe(Event.Eat, _eatListener);
        _snake.Events.Unscribe(Event.Move, _moveListener);
        _view.Events.Unscribe(Event.NewGame, _newGameListener);
        _view.Events.Unscribe(Event.Size, _setSizeListener);
    }
}