public class Controller
{
    private View _view;
    private SnakeModel _snake;
    private Field _field;
    private Input _input;

    public Controller(View view, SnakeModel snake, Field field, Input input)
    {
        _view = view;
        _snake = snake;
        _field = field;
        _input = input;
    }

    public void GameLoop()
    {
        _view.DisplaySnakeInfo(_snake);
        _view.DisplayField(_field);
        while (true)
        {
            try
            {
                _input.ReadSnakeControll();
                _snake.Move();
                Thread.Sleep((int)Speed.Normal);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                break;
            }
        }
    }
}