public interface IRenderable
{
    int Height { get; }
    int Width { get; }
    char[,] Canvas { get; }
    string Render();
}
public interface EventListener
{
    void Update(object? args = null);
}

public interface IObservable
{
    void Subscribe(Event eventType, EventListener subscriber);
    void Unscribe(Event eventType, EventListener subscriber);
    void Notify(Event eventType, object? args = null);
}
public interface ICommand { }