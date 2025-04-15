public enum Vector
{
    Left,
    Right,
    Up,
    Down,
    NotMoving
}
public class SnakeModel
{
    private readonly int _cellSize;
    private Vector _moveDirection = Vector.NotMoving;
    private List<Cell> _parts;
    private Cell _head;
    private Cell _body;
    private Cell _tail;
    public List<Cell> Parts { get => _parts; set { _parts = value; } }
    public Vector MoveDirection { get => _moveDirection; set { _moveDirection = value; } }
    public SnakeModel(int size)
    {
        _cellSize = size;
        _head = new Cell(_cellSize, empty: false, withBorders: false);
        _body = new Cell(_cellSize, empty: false, withBorders: false);
        _tail = new Cell(_cellSize, empty: false, withBorders: false);
        _parts = [_head, _body, _tail];
    }

    public void Move()
    {
    }



}