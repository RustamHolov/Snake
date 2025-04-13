using System.Drawing;

public class Cell : IRenderable
{
    public int Height {get; set;} 

    public int Width {get; set;}

    public int Size {get; set;}
    public string Content { get => _content; set { _content = value; } }
    public char BorderType { get; set; }
    private readonly string[,] _contentField; 
    private string _content = string.Empty;
    public Cell(int size, string background){
        Size = size;
        Height = size;
        Width = size * 2;
        _content = background;
        _contentField = new string[Height,Width];
        FillField(background);
    }
    public Cell(int size) : this(size, "🔲") { }
    public Cell() : this(3) { }
    
    public bool HasContent{get => !string.IsNullOrEmpty(_content);}
    public string[,] ContentField {get => _contentField;}

    public void SetSize(int newSize){
        Size = newSize;
        Height = Size;
        Width = Size * 2;
    }
    public string Render()
    {
        return _content;
    }
    private void FillField(string background){
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                _contentField[i, j] = background;
            }
        }
    }

}