public class MenuSelectedListener : EventListener
{
    private View _view;
    public MenuSelectedListener(View view){
        _view = view;
    }

    public void Update(object? args){
        if(args is Menu menu){
            _view.InvokeAction(menu);
        }   
    }
}