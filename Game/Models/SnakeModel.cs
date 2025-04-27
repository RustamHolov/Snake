using System;
using System.Text;

public class SnakeModel : IObservable
{
    private EventManager _events;
    private readonly int _cellSize;
    private Vector _moveDirection = Vector.NotMoving;
    private List<Cell> _parts;
    private Cell _head;
    private Cell _body;
    private Cell _tail;
    private int _foodEated = 0;
    private Queue<Vector> _turns = [];
    public Vector MoveDirection { get => _moveDirection; }
    public EventManager Events { get => _events; set { _events = value; } }
    public List<Cell> Parts { get => _parts; set { _parts = value; } }
    public int FoodEated { get => _foodEated; }
    public SnakeModel(int size, EventManager events)
    {
        _cellSize = size;
        _head = new Head(_cellSize);
        _body = new Body(_cellSize);
        _tail = new Tail(_cellSize);
        _parts = [_tail, _body, _head];
        _events = events;
    }

    public void Turn(Vector direction)
    {
        // Check for opposite directions
        Vector currentDirection = _turns.Count > 0 ? _turns.Last() : _moveDirection;
        if ((currentDirection == Vector.Left && direction == Vector.Right) ||
            (currentDirection == Vector.Right && direction == Vector.Left) ||
            (currentDirection == Vector.Up && direction == Vector.Down) ||
            (currentDirection == Vector.Down && direction == Vector.Up) ||
            (currentDirection == Vector.NotMoving && direction == Vector.Left)) // cannot move initialy backwards
        {
            return; // Cannot move in the opposite direction
        }
        if (_moveDirection == Vector.NotMoving)
        { // to start the game (first move)
            _moveDirection = direction;
            _turns.Enqueue(direction);
        }
        else
        {
            _turns.Enqueue(direction);
        }

    }
    public void Move()
    {
        if (_moveDirection != Vector.NotMoving)
        {
            if (_turns.Count > 0)
            {
                _moveDirection = _turns.Dequeue();
                Notify(Event.Move, _moveDirection);
            }
            else
            {
                Notify(Event.Move, _moveDirection);
            }
        }
    }
    public void Eat(Cell food, out Body newBodyPart)
    {
        food.SetSize(0); //to fake eating process
        Body newPart = new Body(_cellSize);
        _parts.Insert(1, newPart); //insert new part before tail
        newBodyPart = newPart;
        _foodEated++;
        Notify(Event.Eat, this);
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
    #region SnakeParts
    public class Head(int size) : Cell(size, empty: false)
    {
        public override string Render()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string('▓', Width));
            }

            return builder.ToString();
        }
    }

    public class Body(int size) : Cell(size, empty: false)
    {
        public override string Render()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string('▒', Width));
            }

            return builder.ToString();
        }
    }

    public class Tail(int size) : Cell(size, empty: false)
    {
        public override string Render()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string('░', Width));
            }

            return builder.ToString();
        }

    }
    public class Uroboros(int size) : Cell(size, empty: false)
    {
        public override string Render()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string('▚', Width));
            }

            return builder.ToString();
        }

    }
    #endregion
}