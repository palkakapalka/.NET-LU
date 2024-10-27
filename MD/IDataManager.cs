using System.Collections.Generic; 

namespace project 
{
    public interface IDataManager 
    {
        string Print(); 
        void Save(string path); 
        void Load(string path);
        void CreateTestData();
        void Reset(); 
    }
}
