public class GameDependencies{
    public DataBase DataBase {get; private set;} = null!;
    public Settings Settings {get; private set;} = null!;
    public EventManager EventManager { get; private set; } = null!;
    public View View { get; private set; } = null!;
    public SnakeModel Snake { get; private set; } = null!;
    public Field Field { get; private set; } = null!;
    public Input Input { get; private set; } = null!;
    public Controller Controller { get; private set; } = null!;
    public EatListener EatListener { get; private set; } = null!;
    public MoveListener MoveListener { get; private set; } = null!;
    public NewGameListener NewGameListener { get; private set; } = null!;
    public SetSizeListener SetSizeListener { get; private set; } = null!;
    public PlaceListener PlaceListener { get; private set; } = null!;
    public MenuHoverListener MenuHoverListener { get; private set; } = null!;
    public MenuSelectedListener MenuSelectedListener { get; private set; } = null!;
    public SnakeTurnListener SnakeTurnListener { get; private set; } = null!;
    public PauseListener PauseListener {get; private set;} = null!;
    public ContinueListener ContinueListener {get; private set;} = null!;
    public GameOverListener GameOverListener {get; private set;} = null!;
    public GameStateListener GameStateListener {get; private set;} = null!;
    public SaveListener SaveListener { get; private set; } = null!;
    private bool _firstRun = true;

    public GameDependencies(){
        DataBase = new DataBase();
        EventManager = new EventManager();
        Settings = new Settings(EventManager);

        InitializeGame();
        SubscribeByState();        
    }
    public void NewGame()
    {
        if(!_firstRun){
            EventManager.ClearAllSubscriptions();
            InitializeGame();
            SubscribeByState();
        }
        Controller.GameLoop();
    }
    public void InitializeGame(){
        Snake = new SnakeModel(Settings.PixelSize, EventManager);
        Field = new Field(Settings.GameSize, Settings.GameSize, Settings.PixelSize, Snake, EventManager);
        Input = new Input(EventManager);
        View = new View(EventManager, Input, Field, Settings);
        Controller = new Controller(View, Snake, Field, Input, Settings, DataBase);
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
        GameOverListener = new GameOverListener(Controller);
        GameStateListener = new GameStateListener(this);
        SaveListener = new SaveListener(Controller);
    }
    public void SubscribeByState()
    {
        switch (Settings.GameState)
        {
            case GameState.Menu:
                SubscribeMenuState();
                break;
            case GameState.Game:
                SubscribeGameState();
                break;
            case GameState.Over:
                SubscribeOverState();
                break;
        }
    }

    private void SubscribeMenuState()
    {
        Input.Subscribe(Event.MenuHover, MenuHoverListener);
        Input.Subscribe(Event.MenuSelect, MenuSelectedListener);
        View.Subscribe(Event.NewGame, NewGameListener);
        View.Subscribe(Event.Continue, ContinueListener);
        Settings.Subscribe(Event.Size, SetSizeListener);
        View.Subscribe(Event.Save, SaveListener);
        Settings.Subscribe(Event.State, GameStateListener);
    }

    private void SubscribeGameState()
    {
        Input.Subscribe(Event.SnakeTurn, SnakeTurnListener);
        Input.Subscribe(Event.Pause, PauseListener);
        Field.Subscribe(Event.Place, PlaceListener);
        Snake.Subscribe(Event.Eat, EatListener);
        Snake.Subscribe(Event.Move, MoveListener);
        Field.Subscribe(Event.GameOver, GameOverListener);
        Settings.Subscribe(Event.State, GameStateListener);
    }

    private void SubscribeOverState()
    {
        Input.Subscribe(Event.MenuHover, MenuHoverListener);
        Input.Subscribe(Event.MenuSelect, MenuSelectedListener);
        Settings.Subscribe(Event.State, GameStateListener);
    }
    public void Run(){
        _firstRun = false;
    }
}