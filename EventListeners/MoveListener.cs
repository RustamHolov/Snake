public class MoveListener : EventListener
{
    private Field _field;
    public MoveListener(Field field)
    {
        _field = field;
    }

    public void Update(object? args)
    {
        if (args is Vector moveDirection)
        {
            _field.RedrawSnake(moveDirection);
        }
    }
}