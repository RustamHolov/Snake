public class GameDependencies{
    public EventManager EventManager { get; }
    public View View { get; }
    public SnakeModel Snake { get; }
    public Field Field { get; }
    public Input Input { get; }
    public Controller Controller { get; }
    public EatListener EatListener { get; }
    public MoveListener MoveListener { get; }
    public NewGameListener NewGameListener { get; }
    public SetSizeListener SetSizeListener { get; }
    public PlaceListener PlaceListener { get; }
    public MenuHoverListener MenuHoverListener { get; }
    public MenuSelectedListener MenuSelectedListener { get; }

    public GameDependencies(){
        EventManager = new EventManager();
        Input = new Input(EventManager);
        View = new View(EventManager, Input);
        Snake = new SnakeModel((int)CellSize.Normal, EventManager);
        Field = new Field ((int)GameSize.Normal, (int)GameSize.Normal, (int) CellSize.Normal, Snake, EventManager);
        Controller = new Controller(View, Snake,Field, EventManager);
        EatListener = new EatListener(View);
        MoveListener = new MoveListener(Field);
        SetSizeListener = new SetSizeListener(Controller);
        NewGameListener = new NewGameListener(Controller);
        PlaceListener = new PlaceListener(View);
        MenuHoverListener = new MenuHoverListener(View);
        MenuSelectedListener = new MenuSelectedListener(View);

        Input.Subscribe(Event.MenuHover, MenuHoverListener);
        Input.Subscribe(Event.MenuSelect, MenuSelectedListener);
        Field.Subscribe(Event.Place, PlaceListener);
        Snake.Subscribe(Event.Eat, EatListener);
        Snake.Subscribe(Event.Move, MoveListener);
        View.Subscribe(Event.NewGame, NewGameListener);
        View.Subscribe(Event.Size, SetSizeListener);
        
    }
    public void UnscribeAllListeneres(){
        Field.Unscribe(Event.Place, PlaceListener);
        Snake.Unscribe(Event.Eat, EatListener);
        Snake.Unscribe(Event.Move, MoveListener);
        View.Unscribe(Event.NewGame, NewGameListener);
        View.Unscribe(Event.Size, SetSizeListener);
        Input.Unscribe(Event.MenuHover, MenuHoverListener);
        Input.Unscribe(Event.MenuSelect, MenuSelectedListener);
    }


}