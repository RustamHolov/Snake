using System.Reflection.PortableExecutable;
using System.Text;

public class Field : IRenderable
{
    private SnakeModel _snake;
    private int _cellSize;
    private int _height;
    private int _width;
    private string _content = string.Empty;
    readonly Cell[,] _cells;
    private Dictionary<(int, int), Cell> _snakePositions = new Dictionary<(int, int), Cell>();
    readonly char[,] _matrix;
    Dictionary<(int, int), (int, int)> _cellLinsk;
    public int Size { get => _cellSize;}
    public SnakeModel Snake { get => _snake; }
    public Dictionary<(int, int), (int, int)> CellsLinks { get => _cellLinsk; set { _cellLinsk = value; }}
    public string Content { get => _content; set { _content = value; } }
    public Cell[,] Cells { get => _cells; }

    public char[,] Matrix { get => _matrix; }
    public int Height { get => _height; }

    public int Width { get => _width * 2; }
    public Field(int height, int width, int cellsSize)
    {
        _cellSize = cellsSize;
        _height = height;
        _width = width;
        _snake = new SnakeModel(Size);
        _cells = InitialiseCells(height,width);
        _matrix = new char[Height * Size, Width * Size];
        _cellLinsk = InitialiseLinks();
        PlaceSnakeDefault();
    }
    public Field(int height, int width) : this(height, width, 1) { }
    public Field(int cellsSize) : this(5, 5, cellsSize) { }
    public Field() : this(cellsSize: 1) { }

    
    public Dictionary<(int, int), (int, int)> InitialiseLinks()
    {
        Dictionary<(int, int), (int, int)> links = [];
        for(int i = 0; i < _cells.GetLength(0); i++){
            for(int j = 0; j < _cells.GetLength(1); j++){
                var cellAdress = (i,j);
                var matrixAdress = (i * _cellSize, j * _cellSize * 2); //????????????????
                links.Add(cellAdress, matrixAdress);
            }
        }
        return links;
    }
    public Cell[,] InitialiseCells(int height, int width){
        Cell[,] temp = new Cell[height,width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                temp[i, j] = new Cell(_cellSize);
            }
        }
        return temp;
    }

    public string Render()
    {
        StringBuilder s = new StringBuilder();
        for(int i = 0; i < Height; i++){
            for (int j = 0; j < Width; j++){
                s.Append(_cells[i,j].Render());
            }
            s.Append('\n');
        }
        return s.ToString();
    }
    public void Place(Cell element, (int x, int y) position)
    {
        if (_cellLinsk.TryGetValue(position, out (int x, int y) coordinates))
        {
            char[,] insert = element.ContentField;
            for (int i = coordinates.x; i < insert.GetLength(0) + coordinates.x; i++)
            {
                for (int j = coordinates.y; j < insert.GetLength(1) + coordinates.y; j++)
                {
                    Matrix[i, j] = insert[i - coordinates.x, j - coordinates.y];
                }
            }
            _cells[position.x, position.y] = element;
        }
        else
        {
            throw new ArgumentOutOfRangeException($"Position not found: {position}");
        }

    }
    public bool CouldBePlaced((int x, int y) position)
    {
        return _cells[position.x, position.y].HasContent;
    }
    public void PlaceAll()
    {
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for(int j = 0; j < _cells.GetLength(1); j++){
                Place(_cells[i,j], (i,j));
            }
        }
    }
    public (int, int) GetRandomPosition()
    {
        return (new Random().Next(0, _cells.GetLength(0)), new Random().Next(0, _cells.GetLength(1)));
    }
    public void PlaceSnakeDefault()
    {
        _snakePositions = new Dictionary<(int, int), Cell>();
        (int x, int y) midPoint = (_height / 2, (_width + Snake.Parts.Count)/2);
        for (int i = 0; i < Snake.Parts.Count; i++){
            Place(Snake.Parts[i], midPoint);
            _snakePositions.Add(midPoint, Snake.Parts[i]);
            midPoint.y++;
        }
    }
    public void MoveSnake(Vector direction){
        (int x, int y) previousTailPosition = _snakePositions.ElementAt(0).Key;
        (int x, int y) previousLeadPosition = _snakePositions.ElementAt(_snakePositions.Count-1).Key;
        Dictionary<(int, int), Cell> snakePositionsReversed = _snakePositions.Reverse().ToDictionary();
        _snakePositions.Clear();

        if (direction == Vector.Right && Snake.MoveDirection != Vector.Left){
            Snake.MoveDirection = Vector.Right;
            foreach(((int x, int y) coords, Cell cell) in snakePositionsReversed){   
                if(coords == previousLeadPosition){ //head
                    var toGoCoords = coords.y + 1 < _width ? (coords.x , coords.y + 1) : (coords.x, 0);
                    Place(cell, toGoCoords);
                    _snakePositions.Add(toGoCoords,cell);
                }else{ //other parts
                    var tempCurrentPosition = coords;
                    Place(cell, previousLeadPosition);
                    _snakePositions.Add(previousLeadPosition, cell);
                    previousLeadPosition = tempCurrentPosition;
                }
            }
            Place(new Cell(Size), previousTailPosition); //place empty cell instead of previous tail position

        }else if (direction == Vector.Left && Snake.MoveDirection != Vector.Right){
            Snake.MoveDirection = Vector.Left;
            foreach (((int x, int y) coords, Cell cell) in snakePositionsReversed)
            {
                if (coords == previousLeadPosition)
                { //head
                    var toGoCoords = coords.y - 1 >= 0 ? (coords.x, coords.y - 1) : (coords.x, _width - 1);
                    Place(cell, toGoCoords);
                    _snakePositions.Add(toGoCoords, cell);
                }
                else
                { //other parts
                    var tempCurrentPosition = coords;
                    Place(cell, previousLeadPosition);
                    _snakePositions.Add(previousLeadPosition, cell);
                    previousLeadPosition = tempCurrentPosition;
                }
            }
            Place(new Cell(Size), previousTailPosition); //place empty cell instead of previous tail position
        }
        else if(direction == Vector.Down && Snake.MoveDirection != Vector.Up){
            Snake.MoveDirection = Vector.Down;
            foreach (((int x, int y) coords, Cell cell) in snakePositionsReversed){
                if(coords == previousLeadPosition){
                    var toGoCoords = coords.x + 1 < _height ? (coords.x + 1, coords.y) : (0, coords.y);
                    Place(cell, toGoCoords);
                    _snakePositions.Add(toGoCoords, cell);
                }else{
                    var tempCurrentPosition = coords;
                    Place(cell, previousLeadPosition);
                    _snakePositions.Add(previousLeadPosition, cell);
                    previousLeadPosition = tempCurrentPosition;
                }
            }
            Place(new Cell(Size), previousTailPosition);
        }else if(direction == Vector.Up && Snake.MoveDirection != Vector.Down){
            Snake.MoveDirection = Vector.Up;
            foreach (((int x, int y) coords, Cell cell) in snakePositionsReversed)
            {
                if (coords == previousLeadPosition)
                {
                    var toGoCoords = coords.x - 1 >= 0 ? (coords.x - 1, coords.y) : (_height - 1, coords.y);
                    Place(cell, toGoCoords);
                    _snakePositions.Add(toGoCoords, cell);
                }
                else
                {
                    var tempCurrentPosition = coords;
                    Place(cell, previousLeadPosition);
                    _snakePositions.Add(previousLeadPosition, cell);
                    previousLeadPosition = tempCurrentPosition;
                }
            }
            Place(new Cell(Size), previousTailPosition);
        }
        _snakePositions = _snakePositions.Reverse().ToDictionary();
    }
    public void SpawnFood()
    {
        Cell food = new Food(Size);
        (int x, int y) spawn;
        do
        {
            spawn = GetRandomPosition();
        } while (CouldBePlaced(spawn));
        _cells[spawn.x, spawn.y] = food;
        Place(food, spawn);
    }

}