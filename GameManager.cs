using System;
using System.IO;
using System.Text.Json;
using System.Threading;

namespace TamagotchiGame
{
    public class GameManager
    {
        private Pet currentPet;
        private Timer gameTimer;
        private Timer animationTimer;
        private MiniGame miniGame;
        private const string SaveFileName = "tamagotchi_save.json";
        private int selectedMenuItem = 0;
        private string[] menuItems = { "Годувати", "Грати", "Лікувати", "Прибирати", "Покласти спати", "Статистика", "Зберегти і вийти" };
        private bool gameRunning = true;
        private bool showMenu = true;
        private bool isInterfaceInitialized = false;

        public GameManager()
        {
            miniGame = new MiniGame();
        }

        public void StartGame()
        {
            Console.CursorVisible = false;
            ShowMainMenu();
        }

        private void ShowMainMenu()
        {
            string[] mainMenuItems = { "Нова гра", "Завантажити гру", "Про гру", "Вихід" };
            int selectedMainMenuItem = 0;
            bool showingMainMenu = true;

            while (showingMainMenu)
            {
                Console.Clear();
                DrawMainMenuHeader();

                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("           📋 ГОЛОВНЕ МЕНЮ 📋");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine();

                for (int i = 0; i < mainMenuItems.Length; i++)
                {
                    if (i == selectedMainMenuItem)
                        Console.WriteLine($"        ► {mainMenuItems[i]}");
                    else
                        Console.WriteLine($"          {mainMenuItems[i]}");
                }

                Console.WriteLine();
                Console.WriteLine("Використовуйте ↑↓ для навігації, Enter для вибору");

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedMainMenuItem = (selectedMainMenuItem - 1 + mainMenuItems.Length) % mainMenuItems.Length;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedMainMenuItem = (selectedMainMenuItem + 1) % mainMenuItems.Length;
                        break;
                    case ConsoleKey.Enter:
                        switch (selectedMainMenuItem)
                        {
                            case 0: // Нова гра
                                CreateNewPet();
                                showingMainMenu = false;
                                break;
                            case 1: // Завантажити гру
                                if (File.Exists(SaveFileName))
                                {
                                    LoadGame();
                                    showingMainMenu = false;
                                }
                                else
                                {
                                    ShowMessage("❌ Файл збереження не знайдено!");
                                }
                                break;
                            case 2: // Про гру
                                ShowAboutGame();
                                break;
                            case 3: // Вихід
                                Environment.Exit(0);
                                break;
                        }
                        break;
                }
            }
            if (currentPet == null && showingMainMenu)
            {
                Environment.Exit(0);
            }
            else if (currentPet != null)
            {
                StartTimers();
                GameLoop();
            }
        }

        private void DrawMainMenuHeader()
        {
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║        🎮 КОНСОЛЬНИЙ ТАМАГОЧІ 🎮        ║");
            Console.WriteLine("╚═══════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("          🐱      🐶      🤖");
            Console.WriteLine("         Кіт     Собака   Робот");
            Console.WriteLine();
        }

        private void ShowAboutGame()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════╗");
            Console.WriteLine("║              📖 ПРО ГРУ 📖              ║");
            Console.WriteLine("╚═══════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("🎯 Мета гри:");
            Console.WriteLine("    Доглядайте за своїм віртуальним улюбленцем!");
            Console.WriteLine();
            Console.WriteLine("🎮 Можливості:");
            Console.WriteLine("    • Виберіть улюбленця: Кіт, Собака або Робот");
            Console.WriteLine("    • Годуйте, грайте, лікуйте, прибирайте та кладіть спати");
            Console.WriteLine("    • Грайте в міні-ігри для підвищення щастя");
            Console.WriteLine("    • Стежте за 5 характеристиками");
            Console.WriteLine("    • Ваш улюбленець живе навіть коли гра закрита!");
            Console.WriteLine();
            Console.WriteLine("⚠️  Увага:");
            Console.WriteLine("    Якщо не доглядати за улюбленцем,");
            Console.WriteLine("    він може померти!");
            Console.WriteLine();
            Console.WriteLine("🎨 Створено з ❤️ для любителів Тамагочі");
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey(true);
        }

        private void CreateNewPet()
        {
            Console.Clear();
            Console.WriteLine("=== СТВОРЕННЯ НОВОГО УЛЮБЛЕНЦЯ ===");
            Console.WriteLine("Виберіть тип персонажа:");
            Console.WriteLine("1. 🐱 Кіт");
            Console.WriteLine("2. 🐶 Собака");
            Console.WriteLine("3. 🤖 Робот");

            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D3);

            PetType petType = key.Key switch
            {
                ConsoleKey.D1 => PetType.Cat,
                ConsoleKey.D2 => PetType.Dog,
                ConsoleKey.D3 => PetType.Robot,
                _ => PetType.Cat
            };

            string defaultName = petType switch
            {
                PetType.Cat => "Мурзик",
                PetType.Dog => "Рекс",
                PetType.Robot => "R2D2",
                _ => "Улюбленець"
            };

            Console.WriteLine($"\nВведіть ім'я для вашого улюбленця (за замовчуванням: {defaultName}):");
            Console.CursorVisible = true;
            string name = Console.ReadLine();
            Console.CursorVisible = false;

            if (string.IsNullOrWhiteSpace(name))
                name = defaultName;

            currentPet = new Pet(petType, name);
            isInterfaceInitialized = false;
        }

        private void SaveGame()
        {
            if (currentPet == null) return;

            var saveData = new SaveData
            {
                PetType = currentPet.Type,
                Name = currentPet.Name,
                Hunger = currentPet.Hunger,
                Happiness = currentPet.Happiness,
                Health = currentPet.Health,
                Cleanliness = currentPet.Cleanliness,
                Energy = currentPet.Energy,
                LastSaveTime = DateTime.Now
            };

            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SaveFileName, json);
        }

        private void LoadGame()
        {
            try
            {
                string json = File.ReadAllText(SaveFileName);
                var saveData = JsonSerializer.Deserialize<SaveData>(json);

                currentPet = new Pet(saveData.PetType, saveData.Name)
                {
                    Hunger = saveData.Hunger,
                    Happiness = saveData.Happiness,
                    Health = saveData.Health,
                    Cleanliness = saveData.Cleanliness,
                    Energy = saveData.Energy
                };

                TimeSpan timePassed = DateTime.Now - saveData.LastSaveTime;
                int intervalsPassed = (int)(timePassed.TotalSeconds / 30);

                if (intervalsPassed > 0)
                {
                    Console.WriteLine($"Обробка часу, що минув ({intervalsPassed * 30} секунд)...");
                    Thread.Sleep(1000);
                }

                for (int i = 0; i < intervalsPassed; i++)
                {
                    if (currentPet.IsDead()) break;
                    currentPet.UpdateStats();
                }

                if (currentPet.IsDead())
                {
                    ShowMessage($"😢 На жаль, {currentPet.Name} помер, поки вас не було.");
                    if (File.Exists(SaveFileName)) File.Delete(SaveFileName);
                    currentPet = null;
                    ShowMainMenu();
                    return;
                }

                Console.WriteLine($"Гра завантажено! Минуло приблизно {intervalsPassed * 30 / 60} хвилин.");
                Thread.Sleep(2000);
                isInterfaceInitialized = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка завантаження: {ex.Message}. Створюємо нову гру...");
                Thread.Sleep(2000);
                CreateNewPet();
            }
        }

        private void StartTimers()
        {
            if (currentPet == null) return;
            gameTimer = new Timer(UpdatePetStats, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
            animationTimer = new Timer(UpdateDisplay, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(800));
        }

        private void UpdatePetStats(object state)
        {
            if (currentPet != null && !currentPet.IsDead())
            {
                currentPet.UpdateStats();
                if (currentPet.IsDead())
                {
                    gameRunning = false;
                }
            }
        }

        private void UpdateDisplay(object state)
        {
            if (showMenu && currentPet != null && gameRunning && !currentPet.IsDead())
            {
                DrawInterface();
            }
        }

        private void GameLoop()
        {
            if (currentPet == null) return;

            while (gameRunning && !currentPet.IsDead())
            {
                if (showMenu)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    HandleInput(key);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            StopTimers();

            if (currentPet.IsDead())
            {
                Console.Clear();
                Console.WriteLine($"😢 {currentPet.Name} помер... Гра закінчена!");
                Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
                Console.ReadKey();

                if (File.Exists(SaveFileName))
                    File.Delete(SaveFileName);
            }
            else if (!gameRunning)
            {
                SaveGame();
                Console.Clear();
                Console.WriteLine("Дякуємо за гру! Збережено. До побачення!");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        private void HandleInput(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedMenuItem = (selectedMenuItem - 1 + menuItems.Length) % menuItems.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selectedMenuItem = (selectedMenuItem + 1) % menuItems.Length;
                    break;
                case ConsoleKey.Enter:
                    ExecuteMenuItem();
                    break;
            }
        }

        private void ExecuteMenuItem()
        {
            if (currentPet == null || currentPet.IsDead()) return;

            showMenu = false;
            isInterfaceInitialized = false;

            switch (selectedMenuItem)
            {
                case 0: // Годувати
                    currentPet.Feed();
                    ShowMessage($"🍎 Ви погодували {currentPet.Name}!");
                    break;
                case 1: // Грати
                    PlayMiniGame();
                    break;
                case 2: // Лікувати
                    currentPet.Heal();
                    ShowMessage($"💊 Ви полікували {currentPet.Name}!");
                    break;
                case 3: // Прибирати
                    currentPet.Clean();
                    ShowMessage($"🧹 Ви прибрали за {currentPet.Name}!");
                    break;
                case 4: // Покласти спати
                    currentPet.GoToSleep();
                    ShowMessage($"💤 {currentPet.Name} гарно поспав(ла) та відновив(ла) енергію!");
                    break;
                case 5: // Статистика
                    ShowStats();
                    break;
                case 6: // Зберегти і вийти
                    gameRunning = false;
                    break;
            }

            if (gameRunning && !currentPet.IsDead())
            {
                showMenu = true;
                DrawInterface();
            }
        }

        private void PlayMiniGame()
        {
            Console.Clear();
            Console.WriteLine("Виберіть міні-гру:");
            Console.WriteLine("1. Вгадай число");
            Console.WriteLine("2. Камінь-ножиці-папір");
            Console.WriteLine("3. Повернутися");

            ConsoleKeyInfo key;
            bool validChoice = false;
            bool won = false;
            bool played = false;

            while (!validChoice)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        won = miniGame.PlayGuessNumber();
                        played = true;
                        validChoice = true;
                        break;
                    case ConsoleKey.D2:
                        won = miniGame.PlayRockPaperScissors();
                        played = true;
                        validChoice = true;
                        break;
                    case ConsoleKey.D3:
                        validChoice = true;
                        played = false;
                        break;
                    default:
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.WriteLine("Неправильний вибір! Спробуйте 1, 2 або 3.");
                        break;
                }
            }

            if (played)
            {
                currentPet.Play();
                if (won)
                {
                    currentPet.Happiness = Math.Clamp(currentPet.Happiness + 15, 0, 100);
                    ShowMessage($"🎉 Ви виграли! {currentPet.Name} дуже щасливий!");
                }
                else
                {
                    ShowMessage($"😊 {currentPet.Name} все одно задоволений грою!");
                }
            }
        }

        private void ShowStats()
        {
            Console.Clear();
            Console.WriteLine($"=== СТАТИСТИКА {currentPet.Name.ToUpper()} ===");
            Console.WriteLine();
            Console.WriteLine($"Тип: {GetPetTypeName()}");
            Console.WriteLine($"Стан: {GetStateName()}");
            Console.WriteLine();
            Console.WriteLine($"Голод:     [{GetStatBar(currentPet.Hunger),-10}] {currentPet.Hunger}%");
            Console.WriteLine($"Щастя:     [{GetStatBar(currentPet.Happiness),-10}] {currentPet.Happiness}%");
            Console.WriteLine($"Здоров'я:  [{GetStatBar(currentPet.Health),-10}] {currentPet.Health}%");
            Console.WriteLine($"Чистота:   [{GetStatBar(currentPet.Cleanliness),-10}] {currentPet.Cleanliness}%");
            Console.WriteLine($"Енергія:   [{GetStatBar(currentPet.Energy),-10}] {currentPet.Energy}%");
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення...");
            Console.ReadKey(true);
        }

        private string GetPetTypeName()
        {
            return currentPet.Type switch
            {
                PetType.Cat => "🐱 Кіт",
                PetType.Dog => "🐶 Собака",
                PetType.Robot => "🤖 Робот",
                _ => "Невідомо"
            };
        }

        private string GetStateName()
        {
            return currentPet.GetCurrentState() switch
            {
                PetState.Happy => "😊 Щасливий",
                PetState.Hungry => "😋 Голодний",
                PetState.Sad => "😢 Сумний",
                PetState.Sleeping => "😴 Спить",
                PetState.Normal => "😐 Нормальний",
                _ => "Невідомо"
            };
        }

        private string GetStatBar(int value)
        {
            int blocks = Math.Clamp(value / 10, 0, 10);
            return new string('█', blocks) + new string('░', 10 - blocks);
        }

        private void ShowMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
            Console.ReadKey(true);
        }

        private void DrawInterface()
        {
            if (!showMenu || currentPet == null || !gameRunning || currentPet.IsDead()) return;

            if (!isInterfaceInitialized)
            {
                Console.Clear();
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine($"         🎮 ТАМАГОЧІ: {currentPet.Name} 🎮");
                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine();

                for (int i = 0; i < 5; i++)
                    Console.WriteLine(new string(' ', 39));

                Console.WriteLine();
                Console.WriteLine($"           Стан: {GetStateName()}");
                Console.WriteLine();

                Console.WriteLine($"   Голод: {GetMiniBar(currentPet.Hunger)} Щастя: {GetMiniBar(currentPet.Happiness)}");
                Console.WriteLine($"   Здоров'я: {GetMiniBar(currentPet.Health)} Енергія: {GetMiniBar(currentPet.Energy)}");
                Console.WriteLine($"   Чистота: {GetMiniBar(currentPet.Cleanliness)}");
                Console.WriteLine();

                Console.WriteLine("═══════════════════════════════════════");
                Console.WriteLine("             🎯 МЕНЮ ДІЙ 🎯");
                Console.WriteLine("═══════════════════════════════════════");
                isInterfaceInitialized = true;
            }

            string animation = currentPet.GetCurrentFrame();
            string[] lines = animation.Split('\n');
            int startRow = 4;
            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(0, startRow + i);
                int padding = (39 - lines[i].TrimEnd().Length) / 2;
                Console.Write(new string(' ', Math.Max(0, padding)) + lines[i].TrimEnd() + new string(' ', 39 - padding - lines[i].TrimEnd().Length));
            }

            Console.SetCursorPosition(0, startRow + 6);
            Console.Write($"           Стан: {GetStateName()}" + new string(' ', 20));

            Console.SetCursorPosition(0, startRow + 8);
            Console.Write($"   Голод: {GetMiniBar(currentPet.Hunger)} Щастя: {GetMiniBar(currentPet.Happiness)}");
            Console.SetCursorPosition(0, startRow + 9);
            Console.Write($"   Здоров'я: {GetMiniBar(currentPet.Health)} Енергія: {GetMiniBar(currentPet.Energy)}");
            Console.SetCursorPosition(0, startRow + 10);
            Console.Write($"   Чистота: {GetMiniBar(currentPet.Cleanliness)}" + new string(' ', 20));

            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.SetCursorPosition(0, startRow + 13 + i);
                string prefix = (i == selectedMenuItem) ? " ► " : "   ";
                Console.Write($"{prefix}{menuItems[i]}" + new string(' ', 35 - menuItems[i].Length - prefix.Length));
            }
        }

        private string GetMiniBar(int value)
        {
            int blocks = Math.Clamp(value / 20, 0, 5);
            return "[" + new string('█', blocks) + new string('░', 5 - blocks) + "]";
        }

        private void StopTimers()
        {
            gameTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            animationTimer?.Change(Timeout.Infinite, Timeout.Infinite);
            gameTimer?.Dispose();
            animationTimer?.Dispose();
            gameTimer = null;
            animationTimer = null;
        }
    }
}