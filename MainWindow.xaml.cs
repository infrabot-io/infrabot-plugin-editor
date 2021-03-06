﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TelegramBotPluginEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Command command = null;
        string JsonPluginFile = "";
        bool NewPluginCreating = false;
        bool NewFileCreating = false;
        public static string TempPluginPath = AppDomain.CurrentDomain.BaseDirectory + "TempPlugin";
        string PluginJsonTemplate = "{ \"command_starts_with\": \"/sayhello\", \"command_data_id\": [ 2 ], \"command_execute_file\": \"sayhello.ps1\", \"command_help_manual\": \"Says hello. Write `/sayhello John` to say hello\", \"command_help_short\": \"Says hello\", \"command_author\": \"infrabot.io\", \"command_version\": \"1.0.0.0\", \"command_website\": \"https://infrabot.io\", \"command_default_error\": \"Saying hello to `{DATA}` was not succeded! Unexpected error! Result was: {RESULT}\", \"command_execute_type\": 3, \"command_allowed_users_id\": [], \"command_allowed_chats_id\": [], \"command_show_in_get_commands_list\": true, \"command_execute_results\": [ { \"result_value\": \"0\", \"result_output\": \"User `{DATA}` was not disabled! User name was not sent as an argument to script\", \"result_checktype\": 1 } ] }";
        string CurrentFileViewerFolder = "";
        string PreviousFileViewerFolder = "";
        List<string> FileViewerFolderHistory = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowExecuteResultsWindow(object sender, RoutedEventArgs e)
        {
            ExecuteResultsWindow erw = new ExecuteResultsWindow();
            erw.ShowDialog();
        }

        private void MainMenuInfrabotIO_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://infrabot.io") { UseShellExecute = true });
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ManualViewer.ShowInfo(button);
        }

        private void MainMenuDocumentation_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://infrabot.io/documentation") { UseShellExecute = true });
        }

        private void MainMenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChangeCheckItemText(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            if (cb.IsChecked == false)
            {
                cb.Content = "False";
            }
            else
            {
                cb.Content = "True";
            }
        }

        private void MainMenuCloseFile_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to close this file? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                this.Title = "infrabot - Plugin Editor";
                command = null;
                JsonPluginFile = "";
                MainPanelData.IsEnabled = false;
                MainPluginJsonData.IsEnabled = false;
                MainPluginFilesData.IsEnabled = false;
                MainMenuSaveFile.IsEnabled = false;
                MainMenuCloseFile.IsEnabled = false;

                MainMenuNewPlugin.IsEnabled = true;
                MainMenuOpenPlugin.IsEnabled = true;
                MainMenuSavePlugin.IsEnabled = false;
                MainMenuClosePlugin.IsEnabled = false;

                MainMenuNewFile.IsEnabled = true;
                MainMenuOpenFile.IsEnabled = true;

                MainPluginJsonDataScroll.ScrollToTop();
                NewFileCreating = false;
                CleanFormData();
            }
        }

        private void MainMenuNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (NewFileCreating)
            {
                if (MessageBox.Show("Are you sure that you want to close this file? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewFileCreating = false;
                }
                else
                {
                    return;
                }
            }

            MainMenuNewPlugin.IsEnabled = false;
            MainMenuOpenPlugin.IsEnabled = false;
            MainMenuNewFile.IsEnabled = true;
            MainMenuOpenFile.IsEnabled = true;
            MainPluginFilesData.IsEnabled = false;
            MainPluginJsonData.IsEnabled = true;
            MainMenuSavePlugin.IsEnabled = false;
            MainMenuClosePlugin.IsEnabled = false;
            MainPanelData.IsEnabled = true;
            MainMenuSaveFile.IsEnabled = true;
            MainMenuCloseFile.IsEnabled = true;

            command = JsonConvert.DeserializeObject<Command>(PluginJsonTemplate);
            InitFormWithCommand(command);
            this.Title = "infrabot - Plugin Editor * - Not Saved - New plugin.json file";
        }

        private void MainMenuOpenPlugin_Click(object sender, RoutedEventArgs e)
        {
            if (NewPluginCreating)
            {
                if (MessageBox.Show("Are you sure that you want to close this plugin? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewPluginCreating = false;
                }
                else
                {
                    return;
                }
            }

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".plug";
            dlg.Filter = "Plugin Files (*.plug)|*.plug";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    if (Directory.Exists(TempPluginPath))
                    {
                        Directory.Delete(TempPluginPath, true);
                        WaitForDeletion(TempPluginPath);
                    }
                    ZipFile.ExtractToDirectory(dlg.FileName, TempPluginPath);
                    JsonPluginFile = "";
                    command = null;
                    JsonPluginFile = File.ReadAllText(TempPluginPath + @"\" + "plugin.json");
                    command = JsonConvert.DeserializeObject<Command>(JsonPluginFile);
                    InitFormWithCommand(command);
                    this.Title = "infrabot - Plugin Editor * - Not Saved - " + dlg.FileName;
                    MainPanelData.IsEnabled = true;
                    MainPluginJsonData.IsEnabled = true;
                    MainMenuNewFile.IsEnabled = false;
                    MainMenuOpenFile.IsEnabled = false;
                    MainMenuSaveFile.IsEnabled = false;
                    MainMenuCloseFile.IsEnabled = false;
                    MainPluginFilesData.IsEnabled = true;
                    MainMenuNewPlugin.IsEnabled = true;
                    MainMenuOpenPlugin.IsEnabled = true;
                    MainMenuSavePlugin.IsEnabled = true;
                    MainMenuClosePlugin.IsEnabled = true;
                    NewPluginCreating = true;
                    LoadNewPluginFolder(TempPluginPath);
                }
                catch
                {
                    NewPluginCreating = false;
                    this.Title = "infrabot - Plugin Editor";
                    command = null;
                    JsonPluginFile = "";
                    MainPanelData.IsEnabled = false;
                    MainPluginJsonData.IsEnabled = false;
                    MainPluginFilesData.IsEnabled = false;
                    MainMenuSaveFile.IsEnabled = false;
                    MainMenuCloseFile.IsEnabled = false;
                    MainMenuNewPlugin.IsEnabled = true;
                    MainMenuOpenPlugin.IsEnabled = true;
                    MainMenuSavePlugin.IsEnabled = false;
                    MainMenuClosePlugin.IsEnabled = false;
                    MainMenuNewFile.IsEnabled = true;
                    MainMenuOpenFile.IsEnabled = true;
                    MainPluginJsonDataScroll.ScrollToTop();
                    CleanFormData();
                    MessageBox.Show("This is not an infrabot plugin file!");
                }
            }
        }

        private void MainMenuSavePlugin_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Plugin Files (*.plug)|*.plug";
            if (saveFileDialog.ShowDialog() == true)
            {
                command.command_starts_with = command_starts_with.Text;
                List<int> command_data_id_array = new List<int> { };
                foreach (AllowedIDs id in command_data_id_add_list.Items)
                {
                    command_data_id_array.Add(id.ID);
                }
                command.command_data_id = command_data_id_array;

                command.command_execute_file = command_execute_file.Text;
                command.command_help_manual = command_help_manual.Text;
                command.command_help_short = command_help_short.Text;
                command.command_author = command_author.Text;
                command.command_version = command_version.Text;
                command.command_website = command_website.Text;
                command.command_default_error = command_default_error.Text;
                command.command_execute_type = command_execute_type.SelectedIndex + 1;

                List<int> command_allowed_users_id_array = new List<int> { };
                foreach (AllowedIDs id in command_allowed_users_id_add_list.Items)
                {
                    command_allowed_users_id_array.Add(id.ID);
                }
                command.command_allowed_users_id = command_allowed_users_id_array;

                command.command_show_in_get_commands_list = (bool)command_show_in_get_commands_list.IsChecked;

                string command_serialized = JsonConvert.SerializeObject(command);
                File.WriteAllText(TempPluginPath + @"\" + "plugin.json", command_serialized);
                ZipFile.CreateFromDirectory(TempPluginPath, saveFileDialog.FileName);
                this.Title = "infrabot - Plugin Editor - " + saveFileDialog.FileName;
            }
        }

        private void MainMenuClosePlugin_Click(object sender, RoutedEventArgs e)
        {
            if (NewPluginCreating)
            {
                if (MessageBox.Show("Are you sure that you want to close this plugin? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewPluginCreating = false;
                }
                else
                {
                    return;
                }
            }

            CleanFormData();
            command = null;

            MainMenuNewPlugin.IsEnabled = true;
            MainMenuOpenPlugin.IsEnabled = true;
            MainMenuNewFile.IsEnabled = true;
            MainMenuOpenFile.IsEnabled = true;
            MainPluginFilesData.IsEnabled = false;
            MainPluginJsonData.IsEnabled = false;
            MainMenuSavePlugin.IsEnabled = false;
            MainMenuClosePlugin.IsEnabled = false;
            MainPanelData.IsEnabled = false;
            MainMenuSaveFile.IsEnabled = false;
            MainMenuCloseFile.IsEnabled = false;
            MainPluginJsonDataScroll.ScrollToTop();

            try
            {
                if (Directory.Exists(TempPluginPath))
                {
                    Directory.Delete(TempPluginPath, true);
                    WaitForDeletion(TempPluginPath);
                }
            }
            catch { }
        }

        private void MainMenuOpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (NewFileCreating)
            {
                if (MessageBox.Show("Are you sure that you want to close this file? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                }
                else
                {
                    return;
                }
            }

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                try
                {
                    JsonPluginFile = "";
                    command = null;
                    JsonPluginFile = File.ReadAllText(dlg.FileName);
                    command = JsonConvert.DeserializeObject<Command>(JsonPluginFile);
                    InitFormWithCommand(command);
                    this.Title = "infrabot - Plugin Editor * - Not Saved - " + dlg.FileName;
                    MainPanelData.IsEnabled = true;
                    MainPluginJsonData.IsEnabled = true;
                    MainMenuSaveFile.IsEnabled = true;
                    MainMenuCloseFile.IsEnabled = true;
                    MainPluginFilesData.IsEnabled = false;
                    MainMenuNewPlugin.IsEnabled = false;
                    MainMenuOpenPlugin.IsEnabled = false;
                    MainMenuSavePlugin.IsEnabled = false;
                    MainMenuClosePlugin.IsEnabled = false;
                    NewFileCreating = true;
                }
                catch (Exception ex)
                {
                    NewFileCreating = false;
                    MessageBox.Show("Cannot read file Or READ permission error, or json file constitency is wrong, or this is not a JSON file! \n\nAdditional info: " + ex.Message);
                }
            }
        }

        private void MainMenuSaveFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Config Files (*.json)|*.json";
            if (saveFileDialog.ShowDialog() == true)
            {
                command.command_starts_with = command_starts_with.Text;
                List<int> command_data_id_array = new List<int> { };
                foreach (AllowedIDs id in command_data_id_add_list.Items)
                {
                    command_data_id_array.Add(id.ID);
                }
                command.command_data_id = command_data_id_array;

                command.command_execute_file = command_execute_file.Text;
                command.command_help_manual = command_help_manual.Text;
                command.command_help_short = command_help_short.Text;
                command.command_author = command_author.Text;
                command.command_version = command_version.Text;
                command.command_website = command_website.Text;
                command.command_default_error = command_default_error.Text;
                command.command_execute_type = command_execute_type.SelectedIndex + 1;

                List<int> command_allowed_users_id_array = new List<int> { };
                foreach (AllowedIDs id in command_allowed_users_id_add_list.Items)
                {
                    command_allowed_users_id_array.Add(id.ID);
                }
                command.command_allowed_users_id = command_allowed_users_id_array;

                command.command_show_in_get_commands_list = (bool)command_show_in_get_commands_list.IsChecked;

                string command_serialized = JsonConvert.SerializeObject(command);
                File.WriteAllText(saveFileDialog.FileName, command_serialized);
                this.Title = "infrabot - Plugin Editor - " + saveFileDialog.FileName;
            }
        }

        private void MainMenuNewPlugin_Click(object sender, RoutedEventArgs e)
        {
            if (NewFileCreating)
            {
                if (MessageBox.Show("Are you sure that you want to close this file? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    NewFileCreating = false;
                }
                else
                {
                    return;
                }
            }

            if (NewPluginCreating)
            {
                if (MessageBox.Show("Are you sure that you want to close this plugin? All changes will not be saved!", "Attention", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                }
                else
                {
                    return;
                }
            }

            NewPluginCreating = true;
            MainPanelData.IsEnabled = true;
            MainPluginJsonData.IsEnabled = true;
            MainMenuSavePlugin.IsEnabled = true;
            MainMenuClosePlugin.IsEnabled = true;
            MainPluginFilesData.IsEnabled = true;
            MainMenuSaveFile.IsEnabled = false;
            MainMenuCloseFile.IsEnabled = false;
            MainMenuNewPlugin.IsEnabled = true;
            MainMenuOpenPlugin.IsEnabled = true;
            MainMenuNewFile.IsEnabled = false;
            MainMenuOpenFile.IsEnabled = false;

            try
            {
                if (Directory.Exists(TempPluginPath))
                {
                    Directory.Delete(TempPluginPath, true);
                    WaitForDeletion(TempPluginPath);
                }

                Directory.CreateDirectory(TempPluginPath);
                StreamWriter sw = File.CreateText(TempPluginPath + @"\plugin.json");
                sw.WriteLine(PluginJsonTemplate);
                sw.Close();

                command = null;
                command = JsonConvert.DeserializeObject<Command>(PluginJsonTemplate);
                InitFormWithCommand(command);
                this.Title = "infrabot - Plugin Editor * - Not Saved - New Plugin";
                MainPanelData.IsEnabled = true;
                MainPluginJsonData.IsEnabled = true;
                MainPluginFilesData.IsEnabled = true;
                NewPluginCreating = true;
                NewFileCreating = false;
                CurrentFileViewerFolder = TempPluginPath;
                FileViewerFolderHistory.Add(TempPluginPath);
                LoadNewPluginFolder(TempPluginPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeCheckItemClick(object sender, RoutedEventArgs e)
        {
            // not implemented
        }

        protected void PluginFilesListBoxItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (PluginFilesListBox.SelectedItems.Count == 1)
                {
                    TViewBinding tvb = (PluginFilesListBox.SelectedItem as TViewBinding);
                    if (!tvb.ItemIsFile)
                    {
                        if (tvb.ItemName.ToLower().Equals("..."))
                        {
                            if (FileViewerFolderHistory.Count > 1)
                            {
                                FileViewerFolderHistory.RemoveAt(FileViewerFolderHistory.Count - 1);
                            }
                            LoadNewPluginFolder(FileViewerFolderHistory[FileViewerFolderHistory.Count - 1]);
                        }
                        else
                        {
                            FileViewerFolderHistory.Add(tvb.ItemPath);
                            LoadNewPluginFolder(tvb.ItemPath);
                        }
                    }
                }
            }
            catch { }
        }

        private void PluginFilesListBoxDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    foreach (var file in files)
                    {
                        var f = new FileInfo(file);
                        File.Copy(f.FullName, CurrentFileViewerFolder + @"\" + f.Name, true);
                        LoadNewPluginFolder(CurrentFileViewerFolder);
                    }
                }
            }
            catch { }
        }

        private void PluginFilesListBoxDropKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteItemsFromPluginFolder();
            }
            if (e.Key == Key.F5)
            {
                LoadNewPluginFolder(CurrentFileViewerFolder);
            }
        }

        private void AddItemToList(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                if (button.Name == "command_data_id_add_button")
                    command_data_id_add_list.Items.Add(new AllowedIDs { ID = Convert.ToInt32(command_data_id_add_text.Text) });
                if (button.Name == "command_allowed_users_id_add_button")
                    command_allowed_users_id_add_list.Items.Add(new AllowedIDs { ID = Convert.ToInt32(command_allowed_users_id_add_text.Text) });
            }
            catch { }
        }

        private void PluginFilesListBoxRefresh(object sender, RoutedEventArgs e)
        {
            LoadNewPluginFolder(CurrentFileViewerFolder);
        }

        private void PluginFilesListBoxDeleteItem(object sender, RoutedEventArgs e)
        {
            DeleteItemsFromPluginFolder();
        }

        private void PluginFilesListBoxOpenFolder(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", TempPluginPath);
        }

        private void DeleteItemList(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                if (btn.Name == "command_data_id_delete_button")
                    command_data_id_add_list.Items.Remove((AllowedIDs)btn.DataContext);
                if (btn.Name == "command_allowed_users_id_delete_button")
                    command_allowed_users_id_add_list.Items.Remove((AllowedIDs)btn.DataContext);
            }
            catch { }
        }

        private void InitFormWithCommand(Command command)
        {
            command_starts_with.Text = command.command_starts_with;
            command_data_id_add_list.Items.Clear();
            foreach (int DataID in command.command_data_id)
            {
                command_data_id_add_list.Items.Add(new AllowedIDs { ID = Convert.ToInt32(DataID) });
            }

            command_execute_file.Text = command.command_execute_file;
            command_help_manual.Text = command.command_help_manual;
            command_help_short.Text = command.command_help_short;
            command_author.Text = command.command_author;
            command_version.Text = command.command_version;
            command_website.Text = command.command_website;
            command_default_error.Text = command.command_default_error;
            command_execute_type.SelectedIndex = command.command_execute_type - 1;

            command_allowed_users_id_add_list.Items.Clear();
            foreach (int AllowedUser in command.command_allowed_users_id)
            {
                command_allowed_users_id_add_list.Items.Add(new AllowedIDs { ID = Convert.ToInt32(AllowedUser) });
            }

            command_show_in_get_commands_list.IsChecked = command.command_show_in_get_commands_list;
            if (command.command_show_in_get_commands_list)
            {
                command_show_in_get_commands_list.Content = "True";
            }
            else
            {
                command_show_in_get_commands_list.Content = "False";
            }
        }

        private void CleanFormData()
        {
            command_starts_with.Text = "";
            command_data_id_add_list.Items.Clear();
            command_execute_file.Text = "";
            command_help_manual.Text = "";
            command_help_short.Text = "";
            command_author.Text = "";
            command_version.Text = "";
            command_website.Text = "";
            command_default_error.Text = "";
            command_default_error.Text = "";
            command_execute_type.SelectedIndex = 0;
            command_allowed_users_id_add_list.Items.Clear();
            command_show_in_get_commands_list.IsChecked = false;
            command_show_in_get_commands_list.Content = "False";
            MainTabControl.SelectedItem = MainPluginJsonData;
            PluginFilesListBox.Items.Clear();
        }

        private static void WaitForDeletion(string directoryName)
        {
            bool deleted = false;
            do
            {
                deleted = !System.IO.Directory.Exists(directoryName);
                DateTime now = DateTime.Now;
                System.Threading.Thread.Sleep(100);
            } while (!deleted);
        }

        private void LoadNewPluginFolder(string path)
        {
            PluginFilesListBox.Items.Clear();
            PreviousFileViewerFolder = CurrentFileViewerFolder;
            CurrentFileViewerFolder = path;
            PluginFilesListBox.Items.Add(
                new TViewBinding
                {
                    Icon = "",
                    ItemName = "...",
                    ItemExt = "",
                    ItemPath = PreviousFileViewerFolder,
                    ItemIsFile = false
                }
            );

            foreach (var d in Directory.GetDirectories(path))
            {
                var dir = new DirectoryInfo(d);
                var dirName = dir.Name;

                PluginFilesListBox.Items.Add(
                    new TViewBinding
                    {
                        Icon = "images/filesviewer/folder.png",
                        ItemName = dirName,
                        ItemExt = "",
                        ItemPath = dir.FullName,
                        ItemIsFile = false
                    }
                );
            }

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] Files = dirInfo.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
                string fileIcon = "";
                if (file.Extension.ToLower() == ".crt" || file.Extension.ToLower() == ".p12" || file.Extension.ToLower() == ".cer" || file.Extension.ToLower() == ".pkcs12" || file.Extension.ToLower() == ".jks" || file.Extension.ToLower() == ".keystore")
                {
                    fileIcon = "images/filesviewer/file.png";
                }
                else if (file.Extension.ToLower() == ".exe" || file.Extension.ToLower() == ".bat" || file.Extension.ToLower() == ".com")
                {
                    fileIcon = "images/filesviewer/executable.png";
                }
                else if (file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpeg" || file.Extension.ToLower() == ".gif")
                {
                    fileIcon = "images/filesviewer/image.png";
                }
                else if (file.Extension.ToLower() == ".ps1" || file.Extension.ToLower() == ".psm1" || file.Extension.ToLower() == ".psd1")
                {
                    fileIcon = "images/filesviewer/ps1.png";
                }
                else if (file.Name.ToLower() == "plugin.json")
                {
                    fileIcon = "images/filesviewer/config.png";
                }
                else
                {
                    fileIcon = "images/filesviewer/file.png";
                }

                PluginFilesListBox.Items.Add(
                    new TViewBinding
                    {
                        Icon = fileIcon,
                        ItemName = file.Name,
                        ItemExt = file.Extension,
                        ItemPath = file.DirectoryName,
                        ItemIsFile = true
                    }
                );
            }
        }

        private void DeleteItemsFromPluginFolder()
        {
            try
            {
                foreach (TViewBinding item in PluginFilesListBox.SelectedItems)
                {
                    TViewBinding tvb = (item as TViewBinding);
                    if (tvb.ItemName.ToLower().Equals("plugin.json"))
                    {
                        MessageBox.Show("You can not delete main plugin.json file, because it is main entry point of plugin. Please read manual on website!");
                        continue;
                    }
                    if (tvb.ItemName.ToLower().Equals("..."))
                    {
                        continue;
                    }
                    if (tvb.ItemIsFile)
                    {
                        File.Delete(tvb.ItemPath + @"\" + tvb.ItemName);
                    }
                    else
                    {
                        Directory.Delete(tvb.ItemPath, true);
                    }
                }
            }
            catch { }
            LoadNewPluginFolder(CurrentFileViewerFolder);
        }
    }
}
