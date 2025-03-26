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
    class Editor
    {
        // Constructor and instance check method.
        private static Editor? instance;

        private Editor()
        {
            // PASS
        } 
        
        // Ensure that only one instance of the editor exists at one time.
        public static Editor GetInstance()
        {
            if (instance == null) {
                instance = new Editor();
            }
            return instance;
        }
        
        // Destructor.
        ~Editor()
        {
            // PASS
        }
       
        // Methods. 
        private static ConsoleKeyInfo key; // The main key-by-key input.

        public bool EditorShouldClose()
        {
            if (key.Key == ConsoleKey.Escape) {
                return true;
            } 
            else {
                return false;
            }
        }

        public int RunMainEventLoop()
        {        
            do {
               key = Console.ReadKey(true);
               Console.WriteLine($"Key Pressed: {key.KeyChar}");
            }
            while (!EditorShouldClose()); 
            return 0;
        }
    }
        
    class Program
    {
        static int Main(string [] args)
        {
            Editor editor = Editor.GetInstance();
            editor.RunMainEventLoop();
            return 0;
        }
    }
}

