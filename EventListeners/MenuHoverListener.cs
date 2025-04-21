public class MenuHoverListener : EventListener
{
    private View _view;

    public MenuHoverListener(View view)
    {
        _view = view;
    }

    public void Update(object? args)
    {   
        if(args is (Menu menu1, int hover1, bool horisontal)){
            menu1.UpdateSelected(hover1);
            _view.DisplayHorisontalMenu(menu1);
        }
        if(args is (Menu menu, int hover)){
            menu.UpdateSelected(hover);
            _view.DisplayMenu(menu);
        }
    }
}