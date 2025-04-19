public class EatListener : EventListener
{
    private View _view;
    public EatListener(View view)
    {
        _view = view;
    }
    public void Update(IObservable publisher)
    {
        if (publisher is SnakeModel snake)
        {
            _view.DisplaySnakeInfo(snake);
        }
    }
}