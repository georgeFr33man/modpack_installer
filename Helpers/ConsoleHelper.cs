using System;

namespace ModpackInstaller.Helpers
{
    public class ConsoleHelper
    {
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: " + message);
            Console.ResetColor();
        }
        
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Info: " + message);
            Console.ResetColor();
        }
        
        public static void SuperInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Info: " + message);
            Console.ResetColor();
        }
        
        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success: " + message);
            Console.ResetColor();
        }
        
        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warn: " + message);
            Console.ResetColor();
        }
    }
}