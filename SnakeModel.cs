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
        if ((_moveDirection == Vector.Left && direction == Vector.Right) ||
            (_moveDirection == Vector.Right && direction == Vector.Left) ||
            (_moveDirection == Vector.Up && direction == Vector.Down) ||
            (_moveDirection == Vector.Down && direction == Vector.Up) ||
             (_moveDirection == Vector.NotMoving && direction == Vector.Left)) // cannot move initialy backwards
        {
            return; // Cannot move in the opposite direction
        }
        _moveDirection = direction;
    }
    public void Move()
    {
        if (_moveDirection != Vector.NotMoving)
        {
            Notify(Event.Move, this);
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

    public void Subscribe(Event eventType, EventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }


    public void Notify(Event eventType, IObservable publisher)
    {
        _events.Notify(eventType, publisher);
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
    #endregion
}