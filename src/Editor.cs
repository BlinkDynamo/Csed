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

        // CurrentFile will be set to the filename of the currently open file, and null upon closing of the file.
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
                Height = Dim.Fill(),
            };
            MainWindow.Border.Background = Color.Black;
            MainWindow.Border.BorderBrush = Color.Black;
            MainWindow.Border.BorderStyle = BorderStyle.None;

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
                        CsedMenuOpenFile(); 
                    }),
                    // Save.
                    new MenuItem("_Save", "", () => {    
                        CsedMenuSaveFile(); 
                    }),
                    // Save As.
                    new MenuItem("Save _As", "", () => {    
                        CsedMenuSaveAsFile(); 
                    }),
                    // Close.
                    new MenuItem("_Close", "", () => {
                        CsedMenuCloseFile(); 
                    }),
                    // Quit.
                    new MenuItem("_Quit", "", () => {
                        CsedMenuQuit(); 
                    }),
                }),
            }); 
        }

        // Methods.
        private void CsedCreateEmptyFile(string PathToFile)
        {
            File.Create(PathToFile).Dispose();
        }

        private void CsedMenuOpenFile()
        {
            OpenDialog OpenFileDialog = new OpenDialog("Open", "") {
                CanChooseDirectories = false,
                CanChooseFiles = true,
                AllowsMultipleSelection = false,
            };

            Application.Run(OpenFileDialog); 
             
            if (!OpenFileDialog.Canceled) { 
                // Prompt the user if they have unsaved changes.
                if (MainTextView.IsDirty == true) {
                    int ChoiceIndex = MessageBox.Query("Attention", "You have unsaved changes.", ["Discard", "Save"]); 
                    switch (ChoiceIndex)
                    {
                        case -1:
                            return;

                        case 0:
                            CsedCloseFile(); 
                            CurrentFile = OpenFileDialog.FilePath.ToString();
                            MainTextView.LoadFile(CurrentFile);
                            break;

                        case 1:
                            CsedMenuSaveFile();
                            CsedCloseFile();
                            CurrentFile = OpenFileDialog.FilePath.ToString();
                            MainTextView.LoadFile(CurrentFile);
                            break;
                    }
                }
                else {
                    // Since FileDialog will only let you open a file that exists, no checking of CurrentFile is required.
                    CurrentFile = OpenFileDialog.FilePath.ToString();
                    MainTextView.LoadFile(CurrentFile); 
                }
            }
        }

        private void CsedMenuSaveFile()
        {
            if (CurrentFile != null) {
                System.IO.File.WriteAllText(CurrentFile, MainTextView.Text.ToString()); 
                MainTextView.LoadFile(CurrentFile);
            }
            else {
               CsedMenuSaveAsFile(); 
            }
        }
        
        private void CsedMenuSaveAsFile()
        { 
            SaveDialog WriteFileDialog = new SaveDialog("Save", "");
            Application.Run(WriteFileDialog);
            if (!WriteFileDialog.Canceled) {
                CurrentFile = WriteFileDialog.FilePath.ToString();
                CsedCreateEmptyFile(CurrentFile);
                System.IO.File.WriteAllText(CurrentFile, MainTextView.Text.ToString()); 
                MainTextView.LoadFile(CurrentFile); 
            }
        }

        private void CsedMenuCloseFile()
        {
            // Prompt the user if they have unsaved changes.
            if (CurrentFile != null) {
                if (MainTextView.IsDirty) {
                    int ChoiceIndex = MessageBox.Query("Attention", "You have unsaved changes.", ["Discard", "Save"]); 
                    switch (ChoiceIndex) {
                        case -1:
                            return;

                        case 0:
                            break;

                        case 1:
                            CsedMenuSaveFile();
                            break;
                    }
                } 
                CsedCloseFile(); 
            } 
        }

        private void CsedCloseFile()
        {
            if (CurrentFile == null) {
                return;
            }
            MainTextView.CloseFile();
            MainTextView.Text = string.Empty; // Doing this after CloseFile() for some reason resets IsDirty.
            CurrentFile = null;
        }
        
        private void CsedMenuQuit()
        {
            if ((MainTextView.IsDirty) && (CurrentFile != null)) {
                MessageBox.Query("Attention", "You have unsaved changes."); 
            }
            else {
                Application.RequestStop();
            }
        }

        // Driver method.

        public void CsedRun()
        {
            if (CurrentFile != null) {
                MainTextView.LoadFile(CurrentFile); 
            }

            MainWindow.Add(MainTextView);

            Application.Top.Add(MainWindow, MainMenu);

            Application.Run();

            Application.Shutdown();
        }
    }
}
