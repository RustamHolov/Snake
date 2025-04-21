public class GameStateListener : EventListener{
    private GameDependencies _gameDependencies;
    public GameStateListener(GameDependencies gameDependencies)
    {
        _gameDependencies = gameDependencies;
    }
    public void Update(object? args = null)
    {
        _gameDependencies.EventManager.ClearAllSubscriptions();
        _gameDependencies.SubscribeByState();
    }
}