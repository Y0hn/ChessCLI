using System;
using Chess;

namespace Program
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Hello world !");

            Board brd = new();
            brd.ConsoleWriteOut();
        }
    }
}