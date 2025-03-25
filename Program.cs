using System;
using System.Runtime.InteropServices;

namespace Csed
{
    class Editor
    {
        public Editor()
        {
            // PASS
        }

        ~Editor()
        {
            // PASS
        }

        static void Main(string [] args)
        {
            Editor editor = new Editor();

            ConsoleKeyInfo c;
            
            do {
               c = Console.ReadKey(true);
               Console.WriteLine($"Key Pressed: {c.KeyChar}");
            }
            while (c.Key != ConsoleKey.Escape); 

            return;
        }
    }
}

