public class MoveListener : EventListener
{
    private Field _field;
    public MoveListener(Field field)
    {
        _field = field;
    }

    public void Update(IObservable publisher)
    {
        if (publisher is SnakeModel Snake)
        {
            _field.RedrawSnake(Snake.MoveDirection);
        }
    }
}