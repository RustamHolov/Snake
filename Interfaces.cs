public interface IRenderable
{
    int Height { get; }
    int Width { get; }
    char[,] Canvas { get; }
    string Render();
}
public interface EventListener
{
    void Update(IObservable publisher);
}

public interface IObservable
{
    void Subscribe(Event eventType, EventListener subscriber);
    void Unscribe(Event eventType, EventListener subscriber);
    void Notify(Event eventType, IObservable publisher);
}
public interface ICommand { }