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
    public int Size { get => _cellSize; }
    public SnakeModel Snake { get => _snake; }
    public Dictionary<(int, int), (int, int)> CellsLinks { get => _cellLinsk; set { _cellLinsk = value; } }
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
        _cells = InitialiseCells(height, width);
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
        for (int i = 0; i < _cells.GetLength(0); i++)
        {
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                var cellAdress = (i, j);
                var matrixAdress = (i * _cellSize, j * _cellSize * 2); //????????????????
                links.Add(cellAdress, matrixAdress);
            }
        }
        return links;
    }
    public Cell[,] InitialiseCells(int height, int width)
    {
        Cell[,] temp = new Cell[height, width];
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
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                s.Append(_cells[i, j].Render());
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
            for (int j = 0; j < _cells.GetLength(1); j++)
            {
                Place(_cells[i, j], (i, j));
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
        (int x, int y) midPoint = (_height / 2, (_width + Snake.Parts.Count) / 2);
        for (int i = 0; i < Snake.Parts.Count; i++)
        {
            Place(Snake.Parts[i], midPoint);
            _snakePositions.Add(midPoint, Snake.Parts[i]);
            midPoint.y++;
        }
    }
    public void MoveSnake(Vector direction)
    {
        Dictionary<(int, int), Cell> _newSnakePositions = [];
        (int x, int y) _tailPosition = _snakePositions.First().Key;
        (int x, int y) _headPosition = _snakePositions.Last().Key;
        Cell _headCell = _snakePositions.Last().Value;
        (int x, int y) _newHeadPosition = direction switch
        {
            Vector.Right => _headPosition.y + 1 < _width ? (_headPosition.x, _headPosition.y + 1) : (_headPosition.x, 0),
            Vector.Left => _headPosition.y - 1 >= 0 ? (_headPosition.x, _headPosition.y - 1) : (_headPosition.x, _width - 1),
            Vector.Up => _headPosition.x - 1 >= 0 ? (_headPosition.x - 1, _headPosition.y) : (_height - 1, _headPosition.y),
            Vector.Down => _headPosition.x + 1 < _height ? (_headPosition.x + 1, _headPosition.y) : (0, _headPosition.y),
            _ => _headPosition
        };
        Place(_headCell, _newHeadPosition);
        _newSnakePositions.Add(_newHeadPosition, _headCell);
        foreach (((int x, int y) coords, Cell cell) in _snakePositions.Take(_snakePositions.Count - 1).Reverse().ToDictionary())
        {
            var previousPosition = coords;
            Place(cell, _headPosition);
            _newSnakePositions.Add(_headPosition, cell);
            _headPosition = previousPosition;
        }
        Place(new Cell(Size), _tailPosition);
        _snakePositions = _newSnakePositions.Reverse().ToDictionary();
        Snake.MoveDirection = direction;
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