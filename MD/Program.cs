using System;
// 10.punkts izmanroju W#school un ChatGPT
namespace project 
{
    class Program 
    {
        static void Main(string[] args) 
        {
            string path = "data.txt"; // path to file
            var dm = new DataManager(); // veidojam DataManager 
            dm.CreateTestData(); // veidojam testa datus
            Console.WriteLine(dm.Print()); // izvadam datus konsole
            
             dm.Save(path); // galbjam datus faila
            dm.Reset(); // Reset data
            Console.WriteLine(dm.Print());// izvadam datus konsole 
            dm.Load(path); // ladejam datus no faila
            Console.WriteLine(dm.Print()); // izvadam datus konsole 
            Console.ReadLine(); 
             
            
        }
    }
}
