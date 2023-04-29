namespace SqlParser.Repl;

public static class Program
{
    public static void Main()
    {
        bool cancelRequested = false;

        while (!cancelRequested)
        {
            Console.Write("> ");

            var line = Console.ReadLine();

            switch (line)
            {
                case "help":
                    PrintHelp();
                    break;
                case "clear":
                    HandleClear();
                    break;
                case "exit":
                    cancelRequested = true;
                    break;
                default:
                    PrintUnkownCommand(line);
                    break;
            }

            if (cancelRequested)
                break;
        }
    }

    private static void PrintHelp()
    {
        Console.WriteLine("| 'help'  => lists all commands.");
        Console.WriteLine("| 'clear' => clears the console.");
        Console.WriteLine("| 'exit'  => exits the program.");
    }

    private static void HandleClear()
    {
        Console.Clear();
    }

    private static void PrintUnkownCommand(string? line)
    {
        Console.WriteLine($"| Unknown command '{line}'");
        Console.WriteLine($"| Try 'help' to see all available commands");
    }
}