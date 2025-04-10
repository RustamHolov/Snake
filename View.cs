using System.Text;

public class View
{

    public static string DisplayField()
    {
        string[,] buttons = new string[40,20];
        StringBuilder buffered = new StringBuilder();
        int rows = buttons.GetLength(0);
        int cols = buttons.GetLength(1);
        

        // Build the top border
        string topBorder = "┌" + new string('─', rows ) + "┐" + "\n";
        
        buffered.Append(topBorder);

        // Build the rows
        for (int i = 0; i < cols; i++)
        {
            buffered.Append("│" + new string(' ', rows ) + "│" + "\n");
        }

        // Build the bottom border
        string bottomBorder = "└" + new string('─', rows ) + "┘";
        buffered.Append(bottomBorder);
        return buffered.ToString();
    }
    public void Display(){
        Console.WriteLine(DisplayField());
    }

}