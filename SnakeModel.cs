using System;
using System.Text;
public enum Vector
{
    Left,
    Right,
    Up,
    Down,
    NotMoving
}
public class SnakeModel : IObservable
{
    private List<IObserver> _subscribers {get;} = [];
    private readonly int _cellSize;
    private Vector _moveDirection = Vector.NotMoving;
    private List<Cell> _parts;
    private Cell _head;
    private Cell _body;
    private Cell _tail;
    public Vector MoveDirection {get => _moveDirection;}
    public List<Cell> Parts { get => _parts; set { _parts = value; } }

    public SnakeModel(int size)
    {
        _cellSize = size;
        _head = new Head(_cellSize);
        _body = new Body(_cellSize);
        _tail = new Tail(_cellSize);
        _parts = [_tail, _body, _head];
    }

    public void Turn(Vector direction){
        _moveDirection = direction;
    }
    public void Move()
    {
        if(_moveDirection != Vector.NotMoving){
            Notify();
        }
    }
    public void Eat(Cell food, out Body newBodyPart){
        food.SetSize(0); //to fake eating process
        Body newPart = new Body(_cellSize);
        _parts.Insert(1, newPart); //insert new part before tail
        newBodyPart = newPart;
    }

    public void Subscribe(IObserver subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void Unscribe(IObserver subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    public void Notify()
    {
        foreach(var subscriber in _subscribers){
            subscriber.Update(this);
        }
    }

    public class Head : Cell{
        public Head(int size) : base(size, empty:false){

        }
        public override string Render()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Height; i++){
                builder.AppendLine(new string('▓', Width));
            }
            
            return builder.ToString();
        }
    }

    public class Body : Cell{
        public Body(int size) : base(size, empty: false){

        }
        public override string Render()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                builder.AppendLine(new string('▒', Width ));
            }

            return builder.ToString();
        }
    }

    public class Tail : Cell{
        public Tail(int size) : base(size, empty: false){

        }
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


}