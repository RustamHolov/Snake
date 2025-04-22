public class RatingListener : EventListener
{
    private Controller _controller;
    public RatingListener(Controller controller)
    {
        _controller = controller;
    }
    public void Update(object? args = null)
    {
        _controller.Leaderboard();
    }
}