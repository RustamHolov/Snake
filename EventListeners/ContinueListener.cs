public class ContinueListener : EventListener{
    private Controller _controller;
    public ContinueListener(Controller controller){
        _controller = controller;
    }

    public void Update(object? args = null)
    {
        _controller.Settings.GameState = GameState.Game;
        _controller.GameRuning = true;
        _controller.GameLoop();
    }
}