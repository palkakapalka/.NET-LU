using System.Collections.Generic; // Импортируем пространство имен для работы с коллекциями

namespace project // Пространство имен
{
    public interface IDataManager // Интерфейс IDataManager
    {
        string Print(); // Метод для вывода информации (Method to print information)
        void Save(string path); // Метод для сохранения данных (Method to save data)
        void Load(string path); // Метод для загрузки данных (Method to load data)
        void CreateTestData(); // Метод для создания тестовых данных (Method to create test data)
        void Reset(); // Метод для сброса данных (Method to reset data)
    }
}
