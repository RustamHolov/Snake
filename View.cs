using System.Text;

public class View
{

    public static string BuildField(int width = 50, int height = 20, (int x, int y)? coords = null, string? insert = null, char symbol = '○')
    {
        char _symbol = symbol;
        StringBuilder buffered = new StringBuilder();
          //height |   y
          //width  -   x

        // Build the top border
        string topBorder ="▄" + new string('▄', width ) + "▄" + "\n";
        string[]? insertList = null;
        
        buffered.Append(topBorder);
        if (insert != null){
            insertList = insert.Split('\n');
            if(coords == null){
                coords = ((width - insertList[0].Length) / 2,(height - insertList.Length) / 2);
            }
        }

        // Build the border and content

        for (int i = 0; i < height; i++) // |
        {
            string FillContent(string? content = null)
            {
                string tempContent = "";
                for (int j = 0; j < width; j++) // - 
                {
                    if(insertList != null && content != null && coords.HasValue && j >= coords.Value.x && j < coords.Value.x + insertList[0].Length){
                        tempContent += content;
                        j += content.Length - 1;
                    }else{
                        tempContent += _symbol;
                    }
                }
                return tempContent;
            }
            
            string leftSide = "█";
            string rightSide = "█";
            string content;
            if(insertList!= null && coords.HasValue && i >= coords.Value.y && i < coords.Value.y + insertList.Length) {
                content = FillContent(content: insertList[i + insertList.Length - coords.Value.y - insertList.Length]);
            }else{
                content = FillContent();
            }
            buffered.Append(leftSide + content + rightSide + "\n");
        }

        // Build the bottom border

        string bottomBorder = "▀" + new string('▀', width ) + "▀";
        buffered.Append(bottomBorder);
        return buffered.ToString();
    }
    int x = 0;
    int y = 0;
    public void Display(){
        Console.CursorVisible = false;
        Console.Clear();
        Console.WriteLine(BuildField(insert: "●●●\n●●●\n●●●", coords: (x, y)));
        switch (Console.ReadKey(true).Key){
            case ConsoleKey.W : y--;break;
            case ConsoleKey.S: y++; break;
            case ConsoleKey.A: x--; break;
            case ConsoleKey.D: x++; break;
        }

        Display();
    }

}