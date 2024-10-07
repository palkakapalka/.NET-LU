using System; // Импортируем пространство имен для работы с DateTime

namespace project // Пространство имен
{
    class Program // Класс Program
    {
        static void Main(string[] args) // Основной метод (Main method)
        {
            string path = "data.txt"; // Путь к файлу (Path to file) - относительный путь
            var dm = new DataManager(); // Создаем экземпляр DataManager (Create DataManager instance)
            dm.CreateTestData(); // Создаем тестовые данные (Create test data)
            Console.WriteLine(dm.Print()); // Печатаем данные (Print data)
            dm.Save(path); // Сохраняем данные в файл (Save data to file)
            dm.Reset(); // Сбрасываем данные (Reset data)
            Console.WriteLine(dm.Print()); // Печатаем данные (Print data)
            dm.Load(path); // Загружаем данные из файла (Load data from file)
            Console.WriteLine(dm.Print()); // Печатаем данные (Print data)
            Console.ReadLine(); // Ожидаем ввода пользователя (Wait for user input)
        }
    }
}
