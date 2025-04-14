using System.Text;

public class Field : IRenderable
{
    public int Size {get; set;}
    private string _content = string.Empty;
    readonly List<Cell> _cells;
    readonly char[,] _matrix;
    Dictionary<int, (int, int)> _cellLinsk;
    public Dictionary<int,(int, int)> CellsLinks {get => _cellLinsk; set{_cellLinsk = value;}}
    public Field (int height, int width, int cellsSize){
        Size = cellsSize;
        Height = height;
        Width = width * 2;
        _cells = [.. Enumerable.Repeat(new Cell(cellsSize), height * width)];
        _matrix = new char[Height * cellsSize, Width * cellsSize];
        _cellLinsk = InitialiseLinks();
        FillField();
    }
    public Field(int height, int width) : this(height, width, 1){}
    public Field(int cellsSize) : this(5, 5, cellsSize){}
    public Field() : this(cellsSize:1){}

    public string Content { get => _content; set{ _content = value;}}
    public List<Cell> Cells {get => _cells;}

    public char[,] Matrix {get => _matrix;}
    public int Height {get; set;}

    public int Width { get; set; }
    public Dictionary<int, (int, int)> InitialiseLinks(){
        Dictionary<int, (int, int)> links = [];
        for (int i = 0,k = 0, j = 0; i < Height * (Width/2) ; i++){
            if (j == Size * Width)
            {
                j = 0;
                k += Size;
            }
            links.Add(i, (k, j));
            j += Size * 2;
        }
        return links;
    }

    public string Render()
    {
        StringBuilder s = new StringBuilder();
        for(int i = 0, j = 0; i < Height * Width; i++){
            if (j == Width){
                s.Append('\n');
                j = 0;
            }
            s.Append(_cells[i].Render());
            j++;
        }
        return s.ToString();
    }
    public void Place(Cell element, int position){
        if(_cellLinsk.TryGetValue(position, out (int x, int y) coordinates)){
            char[,] insert = element.ContentField;
            for (int i = coordinates.x; i < insert.GetLength(0) + coordinates.x; i++)
            {
                for (int j = coordinates.y; j < insert.GetLength(1) + coordinates.y; j++)
                {
                    Matrix[i, j] = insert[i - coordinates.x, j - coordinates.y];
                }
            }
            _cells[position] = element;
        }else{
            throw new ArgumentOutOfRangeException($"Position not found: {position}");
        }
        
    }
    public bool CouldBePlaced(int position){
           return _cells[position].HasContent;
    }
    public void PlaceAll(){
        for(int i = 0; i < _cells.Count; i++){
            Place(_cells[i], i);
        }
    }
    public void FillField(){
        for(int i = 0; i < _matrix.GetLength(0); i++){
            for (int j = 0; j < _matrix.GetLength(1); j++){
                _matrix[i,j] = '*';
            }
        }
    }
    public int GetRandomPosition(){
        return new Random().Next(0, _cells.Count);
    }
    public void SpawnFood(){
        Cell food = new Cell(Size, '░');
        int spawn;
        do
        {
            spawn = GetRandomPosition();
        } while (CouldBePlaced(spawn));
        _cells[spawn] = food;
        Place(food, spawn);
    }

}