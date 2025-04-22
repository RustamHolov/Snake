using System.Text;


public class Cell : IRenderable
{
    
    private readonly char[,] _canvas;
    private string _content = string.Empty;
    private bool _empty;
    public int Height { get; set; }

    public int Width { get; set; }

    public int Size { get; set; }
    public string Content { get => _content; set { _content = value; } }
    public virtual bool Empty { get => _empty; }
    public char[,] Canvas { get => _canvas; }
    public Cell(int size, bool empty)
    {
        Size = size;
        Height = size;
        Width = size * 2;
        _empty = empty;
        _canvas = new char[Height, Width];
        _content = Render();
        FillCanvas();
    }
    public Cell(int size) : this(size, true) { }
    public Cell() : this(3) { }



    public void SetSize(int newSize)
    {
        Size = newSize;
        Height = Size;
        Width = Size * 2;
    }
    public virtual string Render()
    {
        StringBuilder builder = new StringBuilder();
        if (Empty)
        {
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string(' ', Width));
            }
        }
        else
        {
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string('◌', Width));
            }
        }
        return builder.ToString();
    }

    private void FillCanvas()
    {
        string[] lines = Content.Split('\n');
        for (int i = 0; i < _canvas.GetLength(0); i++)
        {
            for (int j = 0; j < _canvas.GetLength(1); j++)
            {
                var line = lines[i];
                _canvas[i, j] = line[j];
            }
        }
    }


}