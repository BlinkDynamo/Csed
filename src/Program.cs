﻿/**********************************************************************************************
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
    public class Program
    {
        static int Main(string [] args)
        {
            // Only 'csed' and 'csed filename.txt' are valid input.
            if (args.Length > 1) {
                Console.WriteLine("Invalid usage.");
                Console.WriteLine("Usage: `csed <FILENAME>`, or simply `csed`");
                return 1;
            }
            // Create an Editor e, set Property_CurrentFile if a filename was provided, and begin.
            Editor e = new Editor();
            if (args.Length == 1) {
                e.Property_CurrentFile = args[0];
            }
            e.CsedRun();
            return 0;
        }
            
    }
}


