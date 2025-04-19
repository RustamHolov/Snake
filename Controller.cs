public class Controller{
    private View _view;
    private SnakeModel _snake;
    private Field _field;

    private EventManager _events;

    public Controller(View view, SnakeModel snake, Field field, EventManager events){
        _view = view;
        _snake = snake;
        _field = field;
        _events = events;
        
        
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
                Thread.Sleep((int)Speed.Normal);
            }catch (Exception e){
                Console.WriteLine(e.Message);
                break;
            }
        }
    }
}