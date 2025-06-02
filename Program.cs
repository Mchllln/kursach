using System;

namespace TamagotchiGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Консольний Тамагочі";

            try
            {
                GameManager game = new GameManager();
                game.StartGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критична помилка: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
                Console.ReadKey();
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}