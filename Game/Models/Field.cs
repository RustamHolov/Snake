using System.Text;

public class Field : IRenderable, IObservable
{

    private int _cellSize;
    private int _height;
    private int _width;
    private Cell[,] _grid;

    private char[,] _canvas;
    Dictionary<(int, int), (int, int)> _cellPixelMap;
    private SnakeModel _snake;
    private EventManager _events;

    private Dictionary<Cell, (int, int)> _snakeLocation = new Dictionary<Cell, (int, int)>();
    private static readonly Random _random = new Random();
    public EventManager Events { get => _events; set { _events = value; } }
    public int Size { get => _cellSize; set { _cellSize = value; } }
    public int Height { get => _height; set { _height = value; } }

    public int Width { get => _width * 2; set { _width = value; } }

    public Cell[,] Grid { get => _grid; private set { _grid = value; } }

    public char[,] Canvas { get => _canvas; private set { _canvas = value; } }
    public SnakeModel Snake { get => _snake; }
    public Dictionary<(int, int), (int, int)> CellPixelMap { get => _cellPixelMap; private set { _cellPixelMap = value; } }

    public Field(int height, int width, int cellsSize, SnakeModel snake, EventManager events)
    {
        _cellSize = cellsSize;
        _height = height;
        _width = width;
        _snake = snake;
        _events = events;
        _grid = InitialiseGrid(_height, _width);
        _canvas = new char[Height * Size, Width * Size];
        _cellPixelMap = BuildCellPixelMapping();
        FillCanvas();
        PlaceSnake();
        SpawnFood();
    }


    public Dictionary<(int, int), (int, int)> BuildCellPixelMapping()
    {
        Dictionary<(int, int), (int, int)> links = new Dictionary<(int, int), (int, int)>();
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                var cellAdress = (i, j);
                // The row index is scaled by `_cellSize`, and the column index is scaled by `_cellSize * 2` 
                // to account for the rectangular aspect ratio of the canvas cells.
                var matrixAdress = (i * _cellSize, j * _cellSize * 2);
                links.Add(cellAdress, matrixAdress);
            }
        }
        return links;
    }
    public Cell[,] InitialiseGrid(int height, int width)
    {
        Cell[,] grid = new Cell[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                grid[i, j] = new Cell(_cellSize, empty: true);
            }
        }
        return grid;
    }
    public void PlaceOnCanvas(Cell element, (int x, int y) position)
    {
        if (_cellPixelMap.TryGetValue(position, out (int x, int y) coordinates))
        {
            char[,] insert = element.Canvas;
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
        Notify(Event.Place, this);
    }
    public void SetSize(int size)
    {
        _height = size;
        _width = size;
        GetRidOfSnake();
        _grid = InitialiseGrid(_height, _width);
        _canvas = new char[Height * Size, Width * Size];
        _cellPixelMap = BuildCellPixelMapping();
        FillCanvas();
    }
    public void GetRidOfSnake()
    {
        _snakeLocation.Clear();
    }
    public void FillCanvas()
    {
        for (int i = 0; i < _grid.GetLength(0); i++)
        {
            for (int j = 0; j < _grid.GetLength(1); j++)
            {
                PlaceOnCanvas(_grid[i, j], (i, j));
            }
        }
    }

    public string Render()
    {
        if (_canvas == null || _canvas.Length == 0)
        {
            return "Canvas is null or empty.";
        }

        int rows = _canvas.GetLength(0);
        int cols = _canvas.GetLength(1);

        // Calculate the maximum string length in the matrix for padding
        int maxStringLength = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // In case chars so max length is 1.
                maxStringLength = 1;
            }
        }
        // Calculate total width of the box
        int boxWidth = cols * maxStringLength + 2; // +2 for the box borders

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
                result.Append(_canvas[i, j]);
            }
            result.AppendLine("║"); // Right border
        }

        // Draw bottom border
        result.Append("╚");
        result.Append(new string('═', boxWidth - 2));
        result.AppendLine("╝");

        return result.ToString();
    }

    public bool CellIsEmpty((int x, int y) position)
    {
        return _grid[position.x, position.y].Empty;
    }

    public (int, int) GetRandomPosition()
    {
        return (_random.Next(0, _grid.GetLength(0)), _random.Next(0, _grid.GetLength(1)));
    }
    public void PlaceSnake()
    {
        (int x, int y) midPoint = (_height / 2, (_width + Snake.Parts.Count) / 2);
        for (int i = 0; i < Snake.Parts.Count; i++)
        {
            PlaceOnCanvas(Snake.Parts[i], midPoint);
            _snakeLocation.Add(Snake.Parts[i], midPoint);
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
    public void RedrawSnake(Vector direction)
    {

        void replaceSnakePart(Cell part, (int x, int y) coordinates)
        {
            PlaceOnCanvas(part, coordinates);
            _snakeLocation[part] = coordinates;
        }
        (int x, int y) oldTailCoordinates = _snakeLocation.First().Value;
        (int x, int y) oldHeadCoordinates = _snakeLocation.Last().Value;
        Cell head = _snakeLocation.Last().Key;
        (int x, int y) newHeadCoordinates = GetNewHeadPosition(oldHeadCoordinates, direction);
        bool isFood = !CellIsEmpty(newHeadCoordinates); //check if next cell is food
        bool uroboros = _snakeLocation.ContainsValue(newHeadCoordinates); // check if next cell is snake itself
        if (uroboros)
        {
            var uroborosCell = new SnakeModel.Uroboros(Size);
            PlaceOnCanvas(uroborosCell, newHeadCoordinates);
            _grid[newHeadCoordinates.x, newHeadCoordinates.y] = uroborosCell;
            Notify(Event.GameOver);
            return;
        }
        else if (isFood)
        {
            var food = Grid[newHeadCoordinates.x, newHeadCoordinates.y];
            Snake.Eat(food, out SnakeModel.Body newBodyPart);
            _snakeLocation.Remove(head);
            _snakeLocation.Add(newBodyPart, oldHeadCoordinates);
            _snakeLocation.Add(head, newHeadCoordinates);
            SpawnFood();
        }
        replaceSnakePart(head, newHeadCoordinates);
        var keys = _snakeLocation.Keys.ToList();
        for (int i = keys.Count - 2; i >= 0; i--)
        {
            var part = keys[i];
            var prev = _snakeLocation[part];
            replaceSnakePart(part, oldHeadCoordinates);
            oldHeadCoordinates = prev;
        }
        PlaceOnCanvas(new Cell(Size), oldTailCoordinates);
    }
    public void SpawnFood()
    {
        Cell food = new Food(Size);
        (int x, int y) spawnPosition;
        do
        {
            spawnPosition = GetRandomPosition();
        } while (!CellIsEmpty(spawnPosition));
        PlaceOnCanvas(food, spawnPosition);
        _grid[spawnPosition.x, spawnPosition.y] = food;
    }

    public void Subscribe(Event eventType, IEventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, IEventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }

    public void Notify(Event eventType, object? args = null)
    {
        _events.Notify(eventType, args);
    }
}