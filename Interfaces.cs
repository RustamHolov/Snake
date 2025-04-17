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
    void Subscribe(IObserver subscriber);
    void Unscribe(IObserver subscriber);
    void Notify();
}
public interface ICommand{}