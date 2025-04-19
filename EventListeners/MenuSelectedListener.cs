public class MenuSelectedListener : EventListener
{
    private View _view;
    public MenuSelectedListener(View view){
        _view = view;
    }

    public void Update(IObservable publisher)
    {
    }

    public void Update(Menu menu){
        _view.InvokeAction(menu);
    }
}