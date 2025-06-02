using System;

namespace TamagotchiGame
{
    public class MiniGame
    {
        public bool PlayGuessNumber()
        {
            Random random = new Random();
            int targetNumber = random.Next(1, 11);
            int attempts = 3;

            Console.Clear();
            Console.WriteLine("=== МІНІ-ГРА: ВГАДАЙ ЧИСЛО ===");
            Console.WriteLine("Я загадав число від 1 до 10!");
            Console.WriteLine($"У вас є {attempts} спроби");
            Console.WriteLine();

            for (int i = 0; i < attempts; i++)
            {
                Console.Write($"Спроба {i + 1}: Введіть число: ");
                if (int.TryParse(Console.ReadLine(), out int guess))
                {
                    if (guess == targetNumber)
                    {
                        Console.WriteLine("🎉 Вітаю! Ви вгадали!");
                        Console.WriteLine("Натисніть будь-яку клавішу...");
                        Console.ReadKey();
                        return true;
                    }
                    else if (guess < targetNumber)
                    {
                        Console.WriteLine("Занадто мало!");
                    }
                    else
                    {
                        Console.WriteLine("Занадто багато!");
                    }
                }
                else
                {
                    Console.WriteLine("Неправильний ввід!");
                }
            }

            Console.WriteLine($"😞 Гра закінчена! Число було: {targetNumber}");
            Console.WriteLine("Натисніть будь-яку клавішу...");
            Console.ReadKey();
            return false;
        }

        public bool PlayRockPaperScissors()
        {
            Random random = new Random();
            string[] choices = { "камінь", "ножиці", "папір" };

            Console.Clear();
            Console.WriteLine("=== МІНІ-ГРА: КАМІНЬ-НОЖИЦІ-ПАПІР ===");
            Console.WriteLine("Виберіть: 1-Камінь, 2-Ножиці, 3-Папір");

            ConsoleKeyInfo key = Console.ReadKey(true);
            int playerChoice = -1;

            switch (key.Key)
            {
                case ConsoleKey.D1: playerChoice = 0; break;
                case ConsoleKey.D2: playerChoice = 1; break;
                case ConsoleKey.D3: playerChoice = 2; break;
                default:
                    Console.WriteLine("Неправильний вибір!");
                    Console.ReadKey();
                    return false;
            }

            int computerChoice = random.Next(3);

            Console.WriteLine($"Ви обрали: {choices[playerChoice]}");
            Console.WriteLine($"Комп'ютер обрав: {choices[computerChoice]}");

            if (playerChoice == computerChoice)
            {
                Console.WriteLine("Нічия!");
                Console.ReadKey();
                return true;
            }
            else if ((playerChoice == 0 && computerChoice == 1) ||
                     (playerChoice == 1 && computerChoice == 2) ||
                     (playerChoice == 2 && computerChoice == 0))
            {
                Console.WriteLine("🎉 Ви перемогли!");
                Console.ReadKey();
                return true;
            }
            else
            {
                Console.WriteLine("😞 Ви програли!");
                Console.ReadKey();
                return false;
            }
        }
    }
}