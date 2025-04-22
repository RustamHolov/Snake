public class GameOverListener : EventListener
{
    private Controller _controller;

    public GameOverListener(Controller controller){
        _controller = controller;
    }
    public void Update(object? args = null)
    {
        _controller.GameRuning = false;
        _controller.Settings.GameState = GameState.Over;
        _controller.GameOver();
    }
}