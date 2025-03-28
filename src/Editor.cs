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
        private MenuBar MainMenu;
        private Window MainWindow;
        private TextView MainTextView;

        // Both the field and property are nullable in case just `csed` is entered.
        private string? Field_PathToFile;
        public string? Property_PathToFile
        {
            get { return Field_PathToFile; }
            set { Field_PathToFile = value; }
        }
        
        public Editor()
        {
            Application.Init();

            // Gui setup.
            MainMenu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem("_File", new MenuItem[] {
                    new MenuItem("_Save", "", () => {
                        CsedSaveFile(Field_PathToFile, MainTextView); 
                    }),
                    new MenuItem("_Close", "", () => {
                        CsedCloseFile(MainTextView); 
                    }),
                    new MenuItem("_Quit", "", () => {
                        Application.RequestStop();
                    }),
                }),
            });

            // Nest a MainWindowdow for the editor.
            MainWindow = new Window() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            MainTextView = new TextView() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            }; 
        }

        private void CsedSaveFile(string? pathToFile, TextView? tv)  
        {
            if ((pathToFile != null) && (tv != null)) {
                System.IO.File.WriteAllText(pathToFile, tv.Text.ToString());
            }
        }

        private void CsedOpenFile(string? pathToFile, TextView? tv) 
        {
            if ((pathToFile != null) && (tv != null)) { 
                tv.LoadFile(pathToFile);
            }
        }

        private void CsedCloseFile(TextView? tv) 
        {
            if (tv != null) {
                tv.CloseFile();
            }
        }
        
        public void CsedRun()
        {
            CsedOpenFile(Field_PathToFile, MainTextView); 

            MainWindow.Add(MainTextView);

            Application.Top.Add(MainMenu, MainWindow);

            Application.Run();

            Application.Shutdown();
        }
    }
}
