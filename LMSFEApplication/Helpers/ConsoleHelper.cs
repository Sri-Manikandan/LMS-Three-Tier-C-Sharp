namespace LMSFEApplication.Helpers
{
    public static class ConsoleHelper
    {
        public static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        public static string ReadString(string prompt, bool required = true)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine()?.Trim() ?? string.Empty;
                if (!required || !string.IsNullOrWhiteSpace(input))
                    return input;
                Console.WriteLine("[ERROR] This field is required.");
            }
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
                    return value;
                Console.WriteLine("[ERROR] Please enter a valid positive number.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal value) && value > 0)
                    return value;
                Console.WriteLine("[ERROR] Please enter a valid positive amount.");
            }
        }

        public static bool ReadBool(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (y/n): ");
                var input = Console.ReadLine()?.Trim().ToLower();
                if (input == "y" || input == "yes") return true;
                if (input == "n" || input == "no") return false;
                Console.WriteLine("[ERROR] Please enter 'y' or 'n'.");
            }
        }

        public static int ReadMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.Write($"Enter choice ({min}-{max}): ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                    return choice;
                Console.WriteLine($"[ERROR] Invalid choice. Enter a number between {min} and {max}.");
            }
        }
    }
}
