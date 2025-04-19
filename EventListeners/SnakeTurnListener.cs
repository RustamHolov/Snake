public class SnakeTurnListener : EventListener
{
    private SnakeModel _snake;
    public SnakeTurnListener(SnakeModel snake){
        _snake = snake;
    }
    public void Update(IObservable publisher)
    {
    }
    public void Update(Vector vector){
        if (_snake.MoveDirection != vector){
            _snake.Turn(vector);
        }
    }
}