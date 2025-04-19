using Snake;

public class NewGameListener : EventListener
{
    private GameDependencies _gameDependencies;
    public NewGameListener(GameDependencies gameDependencies)
    {
        _gameDependencies = gameDependencies;
    }
    public void Update(IObservable publisher)
    {
        _gameDependencies.NewGame();
    }
}