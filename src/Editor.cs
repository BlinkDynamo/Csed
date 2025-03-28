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
        private MenuBar MainMenu;
        private Window MainWindow;
        private TextView MainTextView;

        // Both the field and property are nullable in case just `csed` is entered.
        private string? CurrentFile;
        public string? Property_CurrentFile
        {
            get { return CurrentFile; }
            set { CurrentFile = value; }
        }
       
        // Constructor.
        public Editor()
        {
            Application.Init();

            // The main window.
            MainWindow = new Window() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };
            
            // The main text view that will be nested in the main window.
            MainTextView = new TextView() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            // Menu Bar.
            MainMenu = new MenuBar(new MenuBarItem[] {
                // File Menu.
                new MenuBarItem("_File", new MenuItem[] {
                    // Open.
                    new MenuItem("_Open", "", () => { 
                        OpenDialog OpenFileDialog = new OpenDialog("Open", "") {
                            CanChooseDirectories = false,
                            CanChooseFiles = true,
                            AllowsMultipleSelection = false,
                        };

                        Application.Run(OpenFileDialog);

                        if ((MainTextView.IsDirty) && (CurrentFile != null)) {
                            MessageBox.Query("", "You have unsaved changes."); 
                        }
                        else {
                            CurrentFile = OpenFileDialog.FilePath.ToString();
                            CsedOpenFile(CurrentFile, MainTextView); 
                        }
                    }),
                    // Save.
                    new MenuItem("_Save", "", () => {
                        // This checks null twice with CsedSaveFile(). Change this later.
                        if (CurrentFile != null) {
                            CsedSaveFile(CurrentFile, MainTextView); 
                        }
                        else {
                            SaveDialog SaveFileDialog = new SaveDialog("Save", "");
                            Application.Run(SaveFileDialog);
                            CurrentFile = SaveFileDialog.FilePath.ToString();
                            CsedSaveFile(CurrentFile, MainTextView);
                        }
                    }),
                    // Close.
                    new MenuItem("_Close", "", () => {
                        CsedCloseFile(ref CurrentFile, MainTextView); 
                    }),
                    // Quit.
                    new MenuItem("_Quit", "", () => {
                        Application.RequestStop();
                    }),
                }),
            }); 
        }

        // Methods.
        
        private void CsedSaveFile(string? CurrentFile, TextView tv)  
        {
            if (CurrentFile != null) {
                System.IO.File.WriteAllText(CurrentFile, tv.Text.ToString());
            }
        }

        private void CsedOpenFile(string? CurrentFile, TextView tv) 
        {
            if (CurrentFile != null) {
                tv.LoadFile(CurrentFile);
            }
        }

        private void CsedCloseFile(ref string? CurrentFile, TextView tv) 
        {
            if (CurrentFile != null) {
                tv.CloseFile();
                CurrentFile = null;
            }
        }
        
        // Driver method.

        public void CsedRun()
        {
            CsedOpenFile(CurrentFile, MainTextView); 

            MainWindow.Add(MainTextView);

            Application.Top.Add(MainMenu, MainWindow);

            Application.Run();

            Application.Shutdown();
        }
    }
}
