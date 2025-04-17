namespace Snake;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Snake";
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.Clear();
        Controller controller = new Controller();
        controller.MainFlow();
    }
}
