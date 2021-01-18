using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace TelegramBotPluginEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Command command = null;
        string JsonPluginFile = "";

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
            Process.Start("https://infrabot.io");
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ManualViewer.ShowInfo(button);
        }

        private void MainMenuDocumentation_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://infrabot.io/documentation");
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

        }

        private void MainMenuSaveFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainMenuOpenPlugin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainMenuSavePlugin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MainMenuClosePlugin_Click(object sender, RoutedEventArgs e)
        {

        }


        private void MainMenuOpenFile_Click(object sender, RoutedEventArgs e)
        {
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
                    this.Title = "InfraBot.IO configurator * - Not Saved - " + dlg.FileName;
                    MainPanelData.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot read file Or READ permission error, or json file constitency is wrong, or this is not a JSON file! \n\nAdditional info: " + ex.Message);
                }
            }
        }

        private void MainMenuNewPlugin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeCheckItemClick(object sender, RoutedEventArgs e)
        {
            // not implemented
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
            command_default_error.Text = command.command_default_error;
            command_execute_type.SelectedIndex = command.command_execute_type - 1;

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
    }
}
