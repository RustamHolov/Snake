public class SaveListener : IEventListener
{
    private Controller _controller;
    public SaveListener(Controller controller)
    {
        _controller = controller;
    }
    public void Update(object? args = null)
    {

        _controller.SaveRecord();
    }
}