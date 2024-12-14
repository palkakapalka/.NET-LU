using System;
using System.IO;

namespace MD3
{
    public static class ConnectionStringReader
    {
        public static string GetConnectionString()
        {
            string filePath = @"C:\Temp\ConnS.txt";

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Connection string file not found.");
            }

            try
            {
                return File.ReadAllText(filePath).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading connection string file.", ex);
            }
        }
    }
}
