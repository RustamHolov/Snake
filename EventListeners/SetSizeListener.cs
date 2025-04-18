public class SetSizeListener : IObserver
{
    private Controller _controller;
    public SetSizeListener(Controller controller){
        _controller  = controller;
    }
    public void Update(IObservable publisher)
    {
    }
    public void Update(GameSize size){
        _controller.SetGameSize(size);
    }
}