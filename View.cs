using System.Text;


public class View : IObserver
{
    public void DisplayField(Field field){
        Console.SetCursorPosition(0, 2);
        Console.WriteLine(field.Render());
    }
    public void RedrawPart(){

    }
    public void DisplaySnakeInfo(SnakeModel snake){
        Console.SetCursorPosition(0,0);
        Console.WriteLine($"Score: {snake.FoodEated}");
    }
    public void Update(IObservable publisher)
    {
        if( publisher is Field field){
            DisplayField(field);
        }
        if(publisher is SnakeModel snake){
            DisplaySnakeInfo(snake);
        }
    }
}