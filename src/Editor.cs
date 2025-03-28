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
        private MenuBar mainMenu;
        private Window mainWindow;
        private TextView mainTextView;
        private TextView commandTextView;

        // Both the field and property are nullable in case just `csed` is entered.
        private string? filename;
        public string? Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        
        public Editor()
        {
            Application.Init();

            // Gui setup.
            mainMenu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Save", "", () => {
                        CsedSaveFile(); 
                    }),
                    new MenuItem("_Close", "", () => {
                        CsedCloseFile(); 
                    }),
                    new MenuItem("_Quit", "", () => {
                        Application.RequestStop();
                    }),
                }),
            });

            // Nest a mainWindowdow for the editor.
            mainWindow = new Window() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            mainTextView = new TextView() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
        }

        private void CsedSaveFile() 
        {
            if (filename != null) {
                System.IO.File.WriteAllText(filename, mainTextView.Text.ToString());
            }
        }

        private void CsedCloseFile()
        {
            if (mainTextView != null) {
                mainTextView.CloseFile();
            }
        }
        
        public void CsedEditFile()
        {
            // Load the text from file "filename" into the object "text".
            if (filename != null) {
                mainTextView.LoadFile(filename);
            }
            mainWindow.Add(mainTextView);

            Application.Top.Add(mainMenu, mainWindow);

            Application.Run();

            Application.Shutdown();
        }
    }
}
