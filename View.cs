using System.Text;


public class View : IObserver
{
    public void DisplayField(Field field){
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(field.Render());
    }

    public void Update(IObservable publisher)
    {
        if( publisher is Field field){
            DisplayField(field);
        }
    }
}