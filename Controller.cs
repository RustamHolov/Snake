public class Controller
{
    private Settings _settings;
    private View _view;
    private SnakeModel _snake;
    private Field _field;
    private Input _input;
    public bool GameRuning {get; set;} = true;
    public Controller(View view, SnakeModel snake, Field field, Input input, Settings settings)
    {
        _view = view;
        _snake = snake;
        _field = field;
        _input = input;
        _settings = settings;
    }
    public void WelcomeMenu(){
        _view.Start();
    }
    public void GameLoop()
    {
        _view.DisplaySnakeInfo(_snake);
        _view.DisplayField(_field);
        while (GameRuning)
        {
            try
            {
                
                _input.ReadSnakeControll();
                _snake.Move();
                Thread.Sleep(_settings.Speed);
            }
            catch (Exception)
            {
                
                break;
            }
        }
        _view.DisplayGameOver(_snake.FoodEated);
    }
}