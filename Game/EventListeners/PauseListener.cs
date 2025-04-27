public class PauseListener : IEventListener
{
    private Controller _controller;
    private View _view;
    public PauseListener(Controller controller, View view)
    {
        _controller = controller;
        _view = view;
    }

    public void Update(object? args = null)
    {
        _controller.GameRuning = false;
        _controller.Settings.GameState = GameState.Pause;
        _view.OnPause();
    }
}