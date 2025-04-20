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
        GameDependencies Game = new GameDependencies();
        Game.Controller.WelcomeMenu();
        Game.UnscribeAllListeneres();
    }
}
