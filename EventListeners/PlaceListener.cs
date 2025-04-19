public class PlaceListener : EventListener
{
    private View _view;
    public PlaceListener(View view)
    {
        _view = view;
    }
    public void Update(IObservable publisher)
    {
        if (publisher is Field field)
        {
            _view.DisplayField(field);
        }
    }
}