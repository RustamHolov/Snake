public class Controller{
    private const int _cellSize = 1;

    private readonly int Height = 20;
    private readonly int Width = 20;
    private View _view;
    private SnakeModel _snake;
    private Field _field;

    public Controller(){
        _view = new View();
        _snake = new SnakeModel(_cellSize);
        _field = new Field(Height, Width,_cellSize, _snake);
    }

    public void MainFlow(){
        _snake.Subscribe(_field);
        _field.Subscribe(_view);
        _snake.Events.Subscribe("Eat", _view);
        _view.DisplaySnakeInfo(_snake);
        _view.DisplayField(_field);
        while(true){
            try{
                if (Console.KeyAvailable)
                {
                    _snake.Turn(Console.ReadKey(true).Key switch
                    {
                        ConsoleKey.W or ConsoleKey.UpArrow =>  Vector.Up,
                        ConsoleKey.S or ConsoleKey.DownArrow => Vector.Down,
                        ConsoleKey.A or ConsoleKey.LeftArrow => Vector.Left,
                        ConsoleKey.D or ConsoleKey.RightArrow =>  Vector.Right,
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
        _snake.Events.Unscribe("Eat", _view);
    }
}