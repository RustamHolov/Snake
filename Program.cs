namespace Snake;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        //View view = new View();
        Field field = new Field(12, 12, 2);
        field.PlaceAll();
        field.SpawnFood();
        Vector direction = Vector.Right;
        while (true){
            if (Console.KeyAvailable){
                direction = Console.ReadKey(true).Key switch
                {
                    ConsoleKey.W or ConsoleKey.UpArrow => direction == Vector.Down ? Vector.Down : Vector.Up,
                    ConsoleKey.S or ConsoleKey.DownArrow => direction == Vector.Up ? Vector.Up : Vector.Down,
                    ConsoleKey.A or ConsoleKey.LeftArrow => direction == Vector.Right ? Vector.Right : Vector.Left,
                    ConsoleKey.D or ConsoleKey.RightArrow => direction == Vector.Left ? Vector.Left : Vector.Right,
                    _ => throw new Exception("fail button")
                };
            }
            Console.SetCursorPosition(0,0);
            View.DisplayMatrix(field.Matrix);
            field.MoveSnake(direction);
            Thread.Sleep(50);
        }



        // View.DisplayMatrix(field.Matrix);
        // int count = 1;
        // foreach(var kvp in field.CellsLinks){
        //     Console.Write($"{count}: ");
        //     Console.WriteLine($"{kvp.Key} : {kvp.Value}");
        //     count++;
        // }

        //view.Display();
    }
}
