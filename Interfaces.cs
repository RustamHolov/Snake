public interface IRenderable
{
    int Height {get;}
    int Width {get;}
    char[,] Canvas{get;} 
    string Render();
}
public interface IObserver{
    void Update(IObservable publisher);
}

public interface IObservable{
    void Subscribe(Event eventType, IObserver subscriber);
    void Unscribe(Event eventType, IObserver subscriber);
    void Notify(Event eventType, IObservable publisher);
}
public interface ICommand{}