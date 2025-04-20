public class SetSizeListener : EventListener
{
    private Field _field;
    public SetSizeListener(Field field)
    {
        _field = field;
    }
    public void Update(object? args)
    {
       if(args is int size){
            _field.SetSize(size);
        }
    }
}