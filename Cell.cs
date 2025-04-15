using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

public class Cell : IRenderable
{
    public int Height {get; set;} 

    public int Width {get; set;}

    public int Size {get; set;}
    private readonly bool _hasBorders;
    public string Content { get => _content; set { _content = value; } }
    private readonly char[,] _contentField; 
    private string _content = string.Empty;
    private bool _hasContent;
    public Cell(int size, bool empty = true, bool withBorders = true){
        Size = size;
        Height = size;
        Width = size * 2;
        _hasBorders = withBorders;
        _contentField = new char[Height, Width];
        _content = Render();
        _hasContent = !empty;
        FillField();
    }
    public Cell() : this(3) { }
    
    public bool HasContent { get => _hasContent;}
    public char[,] ContentField {get => _contentField;}

    public void SetSize(int newSize){
        Size = newSize;
        Height = Size;
        Width = Size * 2;
    }
    public virtual string Render()
    {
        StringBuilder builder = new StringBuilder();
        if(_hasBorders){
            builder.AppendLine("┌" + new string('─', Width - 2) + "┐");
            for (int i = 0; i < Height - 2; i++)
            {
                builder.AppendLine("│" + new string(' ', Width - 2) + "│");
            }
            builder.AppendLine("└" + new string('─', Width - 2) + "┘");
        }else{
            for(int i = 0; i < Height; i++){
                builder.AppendLine(new string(_hasContent ? ' ' : '█', Width));
            }
        }
        return builder.ToString();
    }
    
    private void FillField(){
        string[] lines = Content.Split('\n');
        for(int i = 0; i < _contentField.GetLength(0); i++){
            for(int j = 0; j < _contentField.GetLength(1); j++){
                var line = lines[i];
                _contentField[i,j] = line[j];
            }
        }
    }

    
}