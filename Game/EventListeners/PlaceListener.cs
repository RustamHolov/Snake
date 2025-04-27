public class PlaceListener : IEventListener
{
    private View _view;
    public PlaceListener(View view)
    {
        _view = view;
    }
    public void Update(object? args)
    {
        if (args is Field field)
        {
            _view.DisplayField(field);
        }
    }
}