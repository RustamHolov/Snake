public class SetSizeListener : EventListener
{
    private Controller _controller;
    public SetSizeListener(Controller controller)
    {
        _controller = controller;
    }
    public void Update(IObservable publisher)
    {
    }
    public void Update(GameSize size)
    {
    }
}