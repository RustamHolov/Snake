using Snake;

public class NewGameListener : EventListener
{
    private GameDependencies _gameDependencies;
    public NewGameListener(GameDependencies gameDependencies)
    {
        _gameDependencies = gameDependencies;
    }
    public void Update(object? args = null)
    {
        _gameDependencies.Run();
        _gameDependencies.NewGame();
    }
}