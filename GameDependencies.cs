public class GameDependencies{
    public Settings Settings {get; private set;}
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
    public PauseListener PauseListener {get; private set;}
    public ContinueListener ContinueListener {get; private set;}

    public GameDependencies(){
        EventManager = new EventManager();
        Settings = new Settings(EventManager);
        Input = new Input(EventManager);
        Snake = new SnakeModel(Settings.PixelSize, EventManager);
        Field = new Field (Settings.GameSize, Settings.GameSize, Settings.PixelSize, Snake, EventManager);
        View = new View(EventManager, Input, Field, Settings);
        Controller = new Controller(View, Snake, Field, Input, Settings);
        EatListener = new EatListener(View);
        MoveListener = new MoveListener(Field);
        SetSizeListener = new SetSizeListener(Field);
        NewGameListener = new NewGameListener(this);
        PlaceListener = new PlaceListener(View);
        MenuHoverListener = new MenuHoverListener(View);
        MenuSelectedListener = new MenuSelectedListener(View);
        SnakeTurnListener = new SnakeTurnListener(Snake);
        PauseListener = new PauseListener(Controller, View);
        ContinueListener = new ContinueListener(Controller);


        SubscribeAllListeners();
        
    }
    public void SubscribeAllListeners(){
        Input.Subscribe(Event.MenuHover, MenuHoverListener);
        Input.Subscribe(Event.MenuSelect, MenuSelectedListener);
        Input.Subscribe(Event.SnakeTurn, SnakeTurnListener);
        Input.Subscribe(Event.Pause, PauseListener);
        Field.Subscribe(Event.Place, PlaceListener);
        Snake.Subscribe(Event.Eat, EatListener);
        Snake.Subscribe(Event.Move, MoveListener);
        View.Subscribe(Event.NewGame, NewGameListener);
        View.Subscribe(Event.Continue, ContinueListener);
        Settings.Subscribe(Event.Size, SetSizeListener);
    }
    public void UnscribeAllListeneres(){
        Field.Unscribe(Event.Place, PlaceListener);
        Snake.Unscribe(Event.Eat, EatListener);
        Snake.Unscribe(Event.Move, MoveListener);
        View.Unscribe(Event.NewGame, NewGameListener);
        View.Unscribe(Event.Continue, ContinueListener);
        Input.Unscribe(Event.MenuHover, MenuHoverListener);
        Input.Unscribe(Event.MenuSelect, MenuSelectedListener);
        Input.Unscribe(Event.Pause, PauseListener);
        Settings.Unscribe(Event.Size, SetSizeListener);
    }

    public void NewGame(){
        if(Snake.Moved){ //in case there was no game before

            UnscribeAllListeneres();

            EventManager = new EventManager();
            Settings = new Settings(EventManager);
            Input = new Input(EventManager);
            Snake = new SnakeModel(Settings.PixelSize, EventManager);
            Field = new Field(Settings.GameSize, Settings.GameSize, Settings.PixelSize, Snake, EventManager);
            View = new View(EventManager, Input, Field, Settings);
            Controller = new Controller(View, Snake, Field, Input, Settings);
            EatListener = new EatListener(View);
            MoveListener = new MoveListener(Field);
            SetSizeListener = new SetSizeListener(Field);
            NewGameListener = new NewGameListener(this);
            PlaceListener = new PlaceListener(View);
            MenuHoverListener = new MenuHoverListener(View);
            MenuSelectedListener = new MenuSelectedListener(View);
            SnakeTurnListener = new SnakeTurnListener(Snake);
            PauseListener = new PauseListener(Controller, View);
            ContinueListener = new ContinueListener(Controller);

            SubscribeAllListeners();
        }
        Controller.GameLoop();
    }
}