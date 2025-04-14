using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

public class Cell : IRenderable
{
    public int Height {get; set;} 

    public int Width {get; set;}

    public int Size {get; set;}
    public string Content { get => _content; set { _content = value; } }
    private readonly char[,] _contentField; 
    private string _content = string.Empty;
    private bool _hasContent;
    public Cell(int size, char background){
        Size = size;
        Height = size;
        Width = size * 2;
        _contentField = new char[Height, Width];
        _content = Render(background);
        _hasContent = background != ' ';
        FillField();
    }
    public Cell(int size) : this(size, ' ') { }
    public Cell() : this(3) { }
    
    public bool HasContent { get => _hasContent;}
    public char[,] ContentField {get => _contentField;}

    public void SetSize(int newSize){
        Size = newSize;
        Height = Size;
        Width = Size * 2;
    }
    public string Render(char background)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("┌" + new string('─', Width-2)+ "┐");
        for(int i = 0; i < Height - 2; i++){
            builder.AppendLine("│" + new string(background, Width -2) + "│");
        }
        builder.AppendLine("└" + new string('─', Width-2)+ "┘");
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

    public string Render() => Render(' ');
}