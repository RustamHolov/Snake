namespace Snake;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        View view = new View();
        view.Display();
    }
}
