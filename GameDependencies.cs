public class GameDependencies{
    public EventManager EventManager { get; private set;}
    public View View { get; private set; }
    public SnakeModel Snake { get; private set; }
    public Field Field { get; private set; }
    public Input Input { get; private set; }
    public Controller Controller { get; private set; }
    public EatListener EatListener { get; private set; }
    public MoveListener MoveListener { get; private set; }
    public NewGameListener NewGameListener { get; private set; }
    public SetSizeListener SetSizeListener { get; private set; }
    public PlaceListener PlaceListener { get; private set; }
    public MenuHoverListener MenuHoverListener { get; private set; }
    public MenuSelectedListener MenuSelectedListener { get; private set; }
    public SnakeTurnListener SnakeTurnListener { get; private set; }

    public GameDependencies(){
        EventManager = new EventManager();
        Input = new Input(EventManager);
        View = new View(EventManager, Input);
        Snake = new SnakeModel((int)CellSize.Normal, EventManager);
        Field = new Field ((int)GameSize.Normal, (int)GameSize.Normal, (int) CellSize.Normal, Snake, EventManager);
        Controller = new Controller(View, Snake, Field, Input);
        EatListener = new EatListener(View);
        MoveListener = new MoveListener(Field);
        SetSizeListener = new SetSizeListener(Controller);
        NewGameListener = new NewGameListener(this);
        PlaceListener = new PlaceListener(View);
        MenuHoverListener = new MenuHoverListener(View);
        MenuSelectedListener = new MenuSelectedListener(View);
        SnakeTurnListener = new SnakeTurnListener(Snake);

        SubscribeAllListeners();
        
    }
    public void SubscribeAllListeners(){
        Input.Subscribe(Event.MenuHover, MenuHoverListener);
        Input.Subscribe(Event.MenuSelect, MenuSelectedListener);
        Input.Subscribe(Event.SnakeTurn, SnakeTurnListener);
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

    public void NewGame(){
        UnscribeAllListeneres();

        EventManager = new EventManager();
        Input = new Input(EventManager);
        View = new View(EventManager, Input);
        Snake = new SnakeModel((int)CellSize.Normal, EventManager);
        Field = new Field((int)GameSize.Normal, (int)GameSize.Normal, (int)CellSize.Normal, Snake, EventManager);
        Controller = new Controller(View, Snake, Field, Input);
        EatListener = new EatListener(View);
        MoveListener = new MoveListener(Field);
        SetSizeListener = new SetSizeListener(Controller);
        NewGameListener = new NewGameListener(this);
        PlaceListener = new PlaceListener(View);
        MenuHoverListener = new MenuHoverListener(View);
        MenuSelectedListener = new MenuSelectedListener(View);
        SnakeTurnListener = new SnakeTurnListener(Snake);
        SubscribeAllListeners();
        Controller.GameLoop();
        
    }
}