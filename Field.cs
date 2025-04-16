using System.Reflection.PortableExecutable;
using System.Text;

public class Field : IRenderable
{
    private SnakeModel _snake;
    private int _cellSize;
    private int _height;
    private int _width;
    readonly Cell[,] _grid;
    private Dictionary<(int, int), Cell> _snakeLocation = new Dictionary<(int, int), Cell>();
    readonly char[,] _canvas;
    Dictionary<(int, int), (int, int)> _cellLinsk;
    public int Size { get => _cellSize; }
    public SnakeModel Snake { get => _snake; }
    public Dictionary<(int, int), (int, int)> CellsLinks { get => _cellLinsk; set { _cellLinsk = value; } }
    public Cell[,] Grid { get => _grid; }

    public char[,] Canvas { get => _canvas; }
    public int Height { get => _height; }

    public int Width { get => _width * 2; } 
    public Field(int height, int width, int cellsSize)
    {
        _cellSize = cellsSize;
        _height = height;
        _width = width;
        _snake = new SnakeModel(Size);
        _grid = InitialiseGrid(height, width);
        _canvas = new char[Height * Size, Width * Size];
        _cellLinsk = InitialiseLinks();
        PlaceAll();
        PlaceSnake();
        SpawnFood();
    }
    public Field(int height, int width) : this(height, width, 1) { }
    public Field(int cellsSize) : this(10, 10, cellsSize) { }
    public Field() : this(cellsSize: 1) { }


    public Dictionary<(int, int), (int, int)> InitialiseLinks()
    {
        Dictionary<(int, int), (int, int)> links = [];
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                var cellAdress = (i, j);
                var matrixAdress = (i * _cellSize, j * _cellSize * 2); //????????????????
                links.Add(cellAdress, matrixAdress);
            }
        }
        return links;
    }
    public Cell[,] InitialiseGrid(int height, int width)
    {
        Cell[,] temp = new Cell[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                temp[i, j] = new Cell(_cellSize, empty: true);
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
                s.Append(_grid[i, j].Render());
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
                    Canvas[i, j] = insert[i - coordinates.x, j - coordinates.y];
                }
            }
            _grid[position.x, position.y] = element;
        }
        else
        {
            throw new ArgumentOutOfRangeException($"Position not found: {position}");
        }

    }
    public bool CellIsEmpty((int x, int y) position)
    {
        return _grid[position.x, position.y].Empty;
    }
    public void PlaceAll()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                Place(_grid[i, j], (i, j));
            }
        }
    }
    public (int, int) GetRandomPosition()
    {
        return (new Random().Next(0, _grid.GetLength(0)), new Random().Next(0, _grid.GetLength(1)));
    }
    public void PlaceSnake()
    {
        (int x, int y) midPoint = (_height / 2, (_width + Snake.Parts.Count) / 2);
        for (int i = 0; i < Snake.Parts.Count; i++)
        {
            Place(Snake.Parts[i], midPoint);
            _snakeLocation.Add(midPoint, Snake.Parts[i]);
            midPoint.y++;
        }
    }
    public (int, int) GetNewHeadPosition((int x, int y) currentHead, Vector direction)
    {
        return direction switch
        {
            Vector.Right => currentHead.y + 1 < _width ? (currentHead.x, currentHead.y + 1) : (currentHead.x, 0),
            Vector.Left => currentHead.y - 1 >= 0 ? (currentHead.x, currentHead.y - 1) : (currentHead.x, _width - 1),
            Vector.Up => currentHead.x - 1 >= 0 ? (currentHead.x - 1, currentHead.y) : (_height - 1, currentHead.y),
            Vector.Down => currentHead.x + 1 < _height ? (currentHead.x + 1, currentHead.y) : (0, currentHead.y),
            _ => currentHead
        };
    }
    public void ReDrawSnake()
    {
        if (Snake.MoveDirection == Vector.NotMoving) return;
        Dictionary<(int, int), Cell> _newSnakePositions = [];
        (int x, int y) _tailPosition = _snakeLocation.First().Key;
        (int x, int y) _headPosition = _snakeLocation.Last().Key;
        Cell _headCell = _snakeLocation.Last().Value;
        (int x, int y) _newHeadPosition = GetNewHeadPosition(_headPosition, Snake.MoveDirection);
        bool isFood = !CellIsEmpty(_newHeadPosition); //check if food
        if (_snakeLocation.ContainsKey(_newHeadPosition)) //if eat itself
        {
            throw new Exception("Game Over");
        }
        Place(_headCell, _newHeadPosition);
        _newSnakePositions.Add(_newHeadPosition, _headCell);
        foreach (((int x, int y) coords, Cell snakePart) in _snakeLocation.Take(_snakeLocation.Count - 1).Reverse().ToDictionary())
        {
            var previousPosition = coords;
            Place(snakePart, _headPosition);
            _newSnakePositions.Add(_headPosition, snakePart);
            _headPosition = previousPosition;
        }
        if (isFood)
        {
            var food = Grid[_newHeadPosition.x, _newHeadPosition.y];
            var tail = _newSnakePositions.Last();
            _newSnakePositions.Remove(tail.Key);
            Cell extraPart = Snake.Eat(food);
            Place(extraPart, tail.Key);
            _newSnakePositions.Add(tail.Key, extraPart);
            Place(tail.Value, _tailPosition);
            _newSnakePositions.Add(_tailPosition, tail.Value);
            SpawnFood();
        }
        else
        {
            Place(new Cell(Size), _tailPosition);
        }
        _snakeLocation = _newSnakePositions.Reverse().ToDictionary();
    }
    public void SpawnFood()
    {
        Cell food = new Food(Size);
        (int x, int y) spawnPosition;
        do
        {
            spawnPosition = GetRandomPosition();
        } while (!CellIsEmpty(spawnPosition));
        _grid[spawnPosition.x, spawnPosition.y] = food;
        Place(food, spawnPosition);
    }

}