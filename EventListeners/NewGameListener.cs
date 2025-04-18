public class NewGameListener : IObserver
{
    private Controller _controller;
    public NewGameListener(Controller controller){
        _controller = controller;
    }
    public void Update(IObservable publisher)
    {
        _controller.MainFlow();
    }
}