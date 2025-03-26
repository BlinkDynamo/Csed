/**********************************************************************************************
*
*   Csed - A C# terminal text editor.
*
*   LICENSE: zlib/libpng 
*
*   Copyright (c) 2024-2025 Josh Hayden (@BlinkDynamo)
*
*   This software is provided ‘as-is’, without any express or implied
*   warranty. In no event will the authors be held liable for any damages
*   arising from the use of this software.
*
*   Permission is granted to anyone to use this software for any purpose,
*   including commercial applications, and to alter it and redistribute it
*   freely, subject to the following restrictions:
*
*   1. The origin of this software must not be misrepresented; you must not
*   claim that you wrote the original software. If you use this software
*   in a product, an acknowledgment in the product documentation would be
*   appreciated but is not required.
*
*   2. Altered source versions must be plainly marked as such, and must not be
*   misrepresented as being the original software.
*
*   3. This notice may not be removed or altered from any source
*   distribution. 
*
*********************************************************************************************/

using System;

namespace Csed
{
    public class Editor
    {
        // Constructor and instance check method.
        private static Editor? instance;
        
        // Ensure that only one instance of the editor exists at one time.
        public static Editor GetInstance()
        {
            if (instance == null) {
                instance = new Editor();
            }
            return instance;
        }
        
        private Editor()
        {
           Console.TreatControlCAsInput = true; 
           RefreshInterface();
        } 
        
        // Destructor.
        ~Editor()
        {
            // PASS
        }
       
        // Class variables. 
        private static ConsoleKeyInfo cki; // The main key-by-key input.
        private bool ShouldClose = false;

        // Class methods. 
        private void ProcessKeypress() 
        {
            cki = Console.ReadKey(true);
            if ((cki.Modifiers & ConsoleModifiers.Alt) != 0) Console.Write("ALT+");
            if ((cki.Modifiers & ConsoleModifiers.Shift) != 0) Console.Write("SHIFT+");
            if ((cki.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");
            if (cki.Key == ConsoleKey.Escape) ShouldClose = true;
            Console.WriteLine("{0} (character '{1}')", cki.Key, cki.KeyChar);
        }

        private void RefreshInterface()
        {
            Console.Clear();
        }

        public int MainEventLoop()
        {        
            while (!ShouldClose)
            { 
                ProcessKeypress(); 
            }
            return 0;
        }
    } 
}
