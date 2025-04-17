public class Controller{
    private const int _cellSize = 1;
    private readonly int Rows = Console.WindowWidth / _cellSize - 110 * _cellSize;
    private readonly int Cols = Console.WindowHeight / _cellSize - 10 * _cellSize;
    private View _view;
    private SnakeModel _snake;
    private Field _field;

    public Controller(){
        _view = new View();
        _snake = new SnakeModel(_cellSize);
        _field = new Field(Cols, Rows,_cellSize, _snake);
    }

    public void MainFlow(){
        _snake.Subscribe(_field);
        _field.Subscribe(_view);
        _view.DisplayField(_field);
        while(true){
            try{
                if (Console.KeyAvailable)
                {
                    _snake.Turn(Console.ReadKey(true).Key switch
                    {
                        ConsoleKey.W or ConsoleKey.UpArrow => _snake.MoveDirection == Vector.Down ? Vector.Down : Vector.Up,
                        ConsoleKey.S or ConsoleKey.DownArrow => _snake.MoveDirection == Vector.Up ? Vector.Up : Vector.Down,
                        ConsoleKey.A or ConsoleKey.LeftArrow => _snake.MoveDirection == Vector.Right ? Vector.Right : Vector.Left,
                        ConsoleKey.D or ConsoleKey.RightArrow => _snake.MoveDirection == Vector.Left ? Vector.Left : Vector.Right,
                        _ => throw new Exception("fail button")
                    });
                }
                _snake.Move();
                Thread.Sleep(80);
            }catch (Exception e){
                Console.WriteLine(e.Message);
                break;
            }

        }
        _snake.Unscribe(_field);
        _field.Unscribe(_view);
    }
}