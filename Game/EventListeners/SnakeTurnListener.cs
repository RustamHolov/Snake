public class SnakeTurnListener : IEventListener
{
    private SnakeModel _snake;
    public SnakeTurnListener(SnakeModel snake)
    {
        _snake = snake;
    }
    public void Update(object? args)
    {
        if (args is Vector direction && _snake.MoveDirection != direction)
        {
            _snake.Turn(direction);
        }
    }
}