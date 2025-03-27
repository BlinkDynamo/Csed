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

using Terminal.Gui;

namespace Csed
{
    public class Editor
    {
        // Fields.
        private MenuBar menu;
        private Window win;
        private TextView editor;
        
        public Editor()
        {
            Application.Init();

            menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Close", "", () => {
                        Application.RequestStop();
                    }),
                    new MenuItem("_Save", "", () => {
                        Application.RequestStop();
                    })
                }),
            });

            // Nest a window for the editor.
            win = new Window() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            editor = new TextView() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
        }

        public void Edit(string filename)
        {
            editor.Text = System.IO.File.ReadAllText(filename);
            win.Add(editor);
            Application.Top.Add(menu, win);

            Application.Run();

            Application.Shutdown();
        }
    }
}
