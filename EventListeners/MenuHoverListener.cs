public class MenuHoverListener : EventListener
{
    private View _view;

    public MenuHoverListener(View view)
    {
        _view = view;
    }

    public void Update(IObservable publisher)
    {
    }
    public void Update((Menu menu, int hover) args)
    {
        args.menu.UpdateSelected(args.hover);
        _view.DisplayMenu(args.menu);
    }
}