namespace Snake;
using System.Text;
class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        //View view = new View();
        Field field = new Field(7, 7, 3);
        field.PlaceAll();
        field.Place(new Cell(3, "🔳"), 12);
        Console.WriteLine(field.Cells.Count);
        foreach(var kvp in field.CellsLinks){
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
        View.DisplayMatrix(field.Matrix);
        //view.Display();
    }
}
