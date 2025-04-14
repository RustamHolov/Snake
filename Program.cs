namespace Snake;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        //View view = new View();
        Field field = new Field(15, 15, 3);
        field.PlaceAll();
        for(int i = 0; i < 220; i++){
            field.SpawnFood();
        }
        Console.WriteLine(field.Cells.Count);
        foreach(var kvp in field.CellsLinks){
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        View.DisplayMatrix(field.Matrix);
        Console.WriteLine(new Cell(5, '░').Content);
        //view.Display();
    }
}
