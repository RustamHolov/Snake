namespace Snake;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        const int CellSize = 1;
        int Rows = Console.WindowWidth / CellSize - 110 * CellSize;
        int Cols = Console.WindowHeight / CellSize - 10 * CellSize;
        Console.Title = "Snake";
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        //View view = new View();
        Field field = new Field(Cols, Rows, CellSize);
        Vector direction = Vector.NotMoving;
        Console.SetCursorPosition(0, 0);
        Console.WriteLine(View.DisplayMatrix(field.Canvas));
        while (true)
        {
            if (Console.KeyAvailable)
            {
                direction = Console.ReadKey(true).Key switch
                {
                    ConsoleKey.W or ConsoleKey.UpArrow => direction == Vector.Down ? Vector.Down : Vector.Up,
                    ConsoleKey.S or ConsoleKey.DownArrow => direction == Vector.Up ? Vector.Up : Vector.Down,
                    ConsoleKey.A or ConsoleKey.LeftArrow => direction == Vector.Right ? Vector.Right : Vector.Left,
                    ConsoleKey.D or ConsoleKey.RightArrow => direction == Vector.Left ? Vector.Left : Vector.Right,
                    _ => throw new Exception("fail button")
                };
            }
            
            try
            {
                field.Snake.Move(direction);
                field.ReDrawSnake();
                if(direction != Vector.NotMoving){
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(View.DisplayMatrix(field.Canvas));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                break;
            }
            Thread.Sleep(80);
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
