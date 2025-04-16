using System.Text;


public class View
{
    public static string BuildField(int width = 50, int height = 20, (int x, int y)? coords = null, string? insert = null, char symbol = '◇')
    {
        // --- Basic Input Validation ---
        if (width <= 0 || height <= 0)
        {
            // Return empty or throw, depending on desired behavior for invalid dimensions
            Console.Error.WriteLine("Error: Width and height must be positive.");
            return string.Empty;
            // Or: throw new ArgumentOutOfRangeException("Width and height must be positive.");
        }

        string[]? insertLines = null;
        int insertWidth = 0;
        int insertHeight = 0;
        int insertX = 0;
        int insertY = 0;
        bool hasInsert = !string.IsNullOrEmpty(insert); // Use IsNullOrEmpty for better check

        if (hasInsert)
        {
            insertLines = insert!.Split('\n');
            // Filter out potential empty strings from split if needed (e.g., trailing newline)
            // insertLines = insert.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            if (insertLines.Length > 0)
            {
                insertHeight = insertLines.Length;
                // Calculate max width based on ALL lines in the insert string
                insertWidth = insertLines.Max(line => line?.Length ?? 0);

                if (insertWidth > 0) // Only proceed if there's actually content
                {
                    // --- Calculate Coordinates & Perform Boundary Checks ---
                    if (coords.HasValue)
                    {
                        insertX = coords.Value.x;
                        insertY = coords.Value.y;
                    }
                    else // Center the insert block
                    {
                        insertX = Math.Max(0, (width - insertWidth) / 2); // Ensure non-negative
                        insertY = Math.Max(0, (height - insertHeight) / 2); // Ensure non-negative
                    }

                    // **Critical Stability Check:** Ensure insert fits within bounds
                    if (insertX < 0 )  
                    {
                        insertX = width;
                        //Console.Error.WriteLine($"Error: Insert content (Width:{insertWidth}, Height:{insertHeight}) does not fit at coordinates ({insertX}, {insertY}) within the field (Width:{width}, Height:{height}).");
                        // Option 1: Disable insert if it doesn't fit
                        //hasInsert = false;
                        // Option 2: Return empty string / throw exception
                        // return string.Empty;
                        // throw new ArgumentException("Insert content does not fit within the specified field dimensions and coordinates.");
                    }
                    if (insertY < 0){
                        insertY = height - insertHeight;
                    }
                    if (insertX + insertWidth > width){
                        insertX = 0 + insertWidth;
                    }
                    if (insertY + insertHeight > height){
                        insertY = 0 + insertHeight;
                    }


                }   
                else
                {
                    // Insert string was provided but had no printable width (e.g. only newlines)
                    hasInsert = false;
                }
            }
            else
            {
                // Split resulted in no lines
                hasInsert = false;
            }
        }
        

        // --- Build the Field ---
        StringBuilder buffered = new StringBuilder((width + 3) * (height + 2)); // Pre-allocate approximate size

        // Top border
        buffered.Append('▄').Append(new string('▄', width)).Append('▄').Append('\n');

        // Content rows
        for (int y = 0; y < height; y++)
        {
            buffered.Append('█'); // Left border

            // Use a StringBuilder for the content part of the line for efficiency
            StringBuilder lineContent = new StringBuilder(width);

            for (int x = 0; x < width; x++)
            {
                bool insertedChar = false;
                if (hasInsert && insertLines != null) // Check insertLines != null for safety
                {
                    // Check if current (x, y) is within the insert area
                    if (x >= insertX && x < insertX + insertWidth &&
                        y >= insertY && y < insertY + insertHeight)
                    {
                        int relativeY = y - insertY;
                        int relativeX = x - insertX;

                        // Check if the character exists in the specific line of the insert string
                        if (relativeY < insertLines.Length && // Should always be true due to outer check, but good practice
                            relativeX < insertLines[relativeY].Length)
                        {
                            lineContent.Append(insertLines[relativeY][relativeX]);
                            insertedChar = true;
                        }
                    }
                }

                // If no character was inserted, append the default symbol
                if (!insertedChar)
                {
                    lineContent.Append(symbol);
                }
            }
            buffered.Append(lineContent); // Append the built content line
            buffered.Append('█').Append('\n'); // Right border and newline
        }

        // Bottom border
        buffered.Append('▀').Append(new string('▀', width)).Append('▀');

        return buffered.ToString();
    }
    public static string DisplayMatrix(char[,] matrix){
        if (matrix == null || matrix.Length == 0)
        {
            return "Matrix is null or empty.";
        }

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        // Calculate the maximum string length in the matrix for padding
        int maxStringLength = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // In this case, we are dealing with chars, so the max length is 1.
                maxStringLength = 1;
            }
        }

        // Calculate total width of the box
        int boxWidth = cols * (maxStringLength + 0) + 2; // +2 for the box borders.  No padding between chars.

        StringBuilder result = new StringBuilder();

        // Draw top border
        result.Append("╔");
        result.Append(new string('═', boxWidth - 2));
        result.AppendLine("╗");

        // Draw matrix content with side borders
        for (int i = 0; i < rows; i++)
        {
            result.Append('║'); // Left border
            for (int j = 0; j < cols; j++)
            {
                result.Append(matrix[i, j]);
            }
            result.AppendLine("║"); // Right border
        }

        // Draw bottom border
        result.Append("╚");
        result.Append(new string('═', boxWidth - 2));
        result.AppendLine("╝");

        return result.ToString();
    }
    int x = 0;
    int y = 0;
    public void Display(){
        Console.CursorVisible = false;
        Console.SetCursorPosition(0,0);
        Console.WriteLine(BuildField(insert: " - ", coords: (x, y)));
        switch (Console.ReadKey(true).Key){
            case ConsoleKey.W: y--; break;
            case ConsoleKey.S: y++; break;
            case ConsoleKey.A: x--; break;
            case ConsoleKey.D: x++; break;
        }   

        Display();
    }

}