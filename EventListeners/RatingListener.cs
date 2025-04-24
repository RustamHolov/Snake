public class RatingListener : EventListener
{
    private Controller _controller;
    public RatingListener(Controller controller)
    {
        _controller = controller;
    }
    public void Update(object? args = null)
    {
        if (args != null && args is int page)
        {
            _controller.ViewLeaderboard(page);
        }
        else
        {
            _controller.ViewLeaderboard();
        }
        
    }
}