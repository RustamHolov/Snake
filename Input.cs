using System.Text;
public class Input : IObservable
{

    private EventManager _events;
    public EventManager Events { get => _events; set { _events = value; } }
    public Input(EventManager events)
    {
        _events = events;
    }

    public void ReadMenuOption(Menu menu)
    {
        while (true)
        {
            if(Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        Notify(Event.MenuSelect, menu);
                        break;
                    case ConsoleKey.W or ConsoleKey.UpArrow:
                        Notify(Event.MenuHover, (menu, -1));
                        continue;
                    case ConsoleKey.S or ConsoleKey.DownArrow:
                        Notify(Event.MenuHover, (menu, 1));
                        continue;
                }
            }
            
        }
    }
    public void ReadHorisontalMenuOption(Menu menu){
        while (true)
        {
            if (Console.KeyAvailable)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        Notify(Event.MenuSelect, menu);
                        break;
                    case ConsoleKey.D or ConsoleKey.RightArrow:
                        Notify(Event.MenuHover, (menu, 1, true));
                        continue;
                    case ConsoleKey.A or ConsoleKey.LeftArrow:
                        Notify(Event.MenuHover, (menu, -1, true));
                        continue;
                }
            }
        }
    }
    public void ReadSnakeControll(){
        ConsoleKey? latestKey = null;
        while (Console.KeyAvailable)
        {
            latestKey = Console.ReadKey(true).Key;
        }
        if (latestKey.HasValue){
            switch (latestKey)
            {
                case ConsoleKey.W or ConsoleKey.UpArrow: Notify(Event.SnakeTurn, Vector.Up); break;
                case ConsoleKey.S or ConsoleKey.DownArrow: Notify(Event.SnakeTurn, Vector.Down); break;
                case ConsoleKey.A or ConsoleKey.LeftArrow: Notify(Event.SnakeTurn, Vector.Left); break;
                case ConsoleKey.D or ConsoleKey.RightArrow: Notify(Event.SnakeTurn, Vector.Right); break;
                case ConsoleKey.Escape : Notify(Event.Pause); break;
                default: break;
            }
        }
    }
    public string ReadNameInput(int startX, int startY, int maxLength = 15)
    {
        var input = new StringBuilder();
        int warningX = startX;
        int warningY = startY + 1;

        // Draw input field
        Console.SetCursorPosition(startX, startY);
        Console.Write(new string(' ', maxLength));
        Console.SetCursorPosition(startX, startY);

        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace && input.Length > 0)
            {
                input.Length--;
                ClearWarning(warningX, warningY);
            }

            else if ((char.IsLetterOrDigit(key.KeyChar) || "-.,".Contains(key.KeyChar)) && input.Length < maxLength)
            {
                input.Append(key.KeyChar);
                ClearWarning(warningX, warningY);
            }
            else if (input.Length >= maxLength)
            {
                ShowWarning("Limit reached!", warningX, warningY);
            }

            // Redraw input cleanly each loop
            Console.SetCursorPosition(startX, startY);
            Console.Write(new string(' ', maxLength)); // clear
            Console.SetCursorPosition(startX, startY);
            Console.Write(input.ToString());
            Console.SetCursorPosition(startX + input.Length, startY);
        }

        ClearWarning(warningX, warningY);
        return input.ToString();
    }
    private void ShowWarning(string message, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(message); // avoid leftover text
        Console.ResetColor();
    }

    private void ClearWarning(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(new string(' ', 15));
    }

    public void Notify(Event eventType, object? args = null){
        _events.Notify(eventType, args);
    }

    public void Subscribe(Event eventType, EventListener subscriber)
    {
        _events.Subscribe(eventType, subscriber);
    }

    public void Unscribe(Event eventType, EventListener subscriber)
    {
        _events.Unscribe(eventType, subscriber);
    }
}