public class MenuHoverListener : EventListener
{
    private View _view;

    public MenuHoverListener(View view)
    {
        _view = view;
    }

    public void Update(object? args)
    {   
        if(args is (Menu menu, int hover)){
            menu.UpdateSelected(hover);
            _view.DisplayMenu(menu);
        }
    }
}