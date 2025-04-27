public class SetSizeListener : IEventListener
{
    private Field _field;
    public SetSizeListener(Field field)
    {
        _field = field;
    }
    public void Update(object? args)
    {
        if (args is int size)
        {
            Console.Clear();
            _field.SetSize(size);
        }
    }
}