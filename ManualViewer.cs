using System.Windows;
using System.Windows.Controls;

namespace TelegramBotPluginEditor
{
    public partial class ManualViewer
    {
        public static void ShowInfo(Button button)
        {
            if (button.Name == "command_starts_with_i")
                MessageBox.Show("Parameter in plugin.json file is: command_starts_with \n\nSpecify beginning of the commands, to which Bot should react. It is recommended to start all commands with '/'. \n\nExample: /restart \n\nIn this example if command begins with /restart then bot will react to it according to the rules. Text is case sensitive. '/Restart' is not the same as '/restart'");
            else if (button.Name == "command_data_id_i")
                MessageBox.Show("Parameter in plugin.json file is: command_data_id \n\nSpecifiy which parts of the command separated with space(' ') contains data which should be sent as argument to your script or executable. Count starts from 1 (not 0). \n\nExample: 2 \n\nSpecified example means that if your intended functionality for example is '/restart NAME_OF_YOUR_SERVER', then second part 'NAME_OF_YOUR_SERVER' is a string which will be sent as argument to your executable or script. \n\nExample 2: 2,3\n\nSpecified example means that if your intended command is '/restart YOUR_SERVER_NAME CLUSTER_NUMBER', then second part 'YOUR_SERVER_NAME' and third part 'CLUSTER_NUMBER' will be sent as 2 arguments to your script or executable");
            else if (button.Name == "command_execute_file_i")
                MessageBox.Show("Parameter in plugin.json file is: command_execute_file \n\nSpecify exact path to your PS1 script file (if Execute type is 'PowerShell with custom result answers') or EXECUTABLE file (if Execute type is 'My Custom Application with custom result answers')\n\nExample: C:\\Program Files\\infrabot.io\\tools\\my_super_script.ps1");
            else if (button.Name == "command_help_manual_i")
                MessageBox.Show("Parameter in plugin.json file is: command_help_manual \n\nSpecify long help manual if you send ? argument to bot\n\nExample: /restart ?\n\nIn the specified example Bot will show you text provided here");
            else if (button.Name == "command_help_short_i")
                MessageBox.Show("Parameter in plugin.json file is: command_help_short \n\nSpecify brief info about what your command does. It will be shown in /getcommands result\n\nExample: /getcommands\n\nIn the specified example if you send /getcommands, then Bot will show you text provided here near command itself\n\nOutput in /getcommands will be like: /your_command - Restarts fridge in the kitchen");
            else if (button.Name == "command_author_i")
                MessageBox.Show("Parameter in plugin.json file is: command_author \n\nSpecify author of this plugin");
            else if (button.Name == "command_version_i")
                MessageBox.Show("Parameter in plugin.json file is: command_version \n\nSpecify version of this plugin");
            else if (button.Name == "command_website_i")
                MessageBox.Show("Parameter in plugin.json file is: command_website \n\nSpecify website of the plugin author");
            else if (button.Name == "command_default_error_i")
                MessageBox.Show("Parameter in plugin.json file is: command_default_error \n\nSpecify default error if no command_execute_results will be matched with your script result output\n\nWarning: {DATA} in the text will be replaced with the arguments which you specified to command. {RESULT} in the text will be replaced with the output from your script or executable. \n\nExmaple: If you send lets say send command `/restart fridge01`, and something unexpected happens and your text here is like 'Unexpected error when restarting `{DATA}` fridge', then this message will be shown: 'Unexpected error when restarting `fridge01` fridge'\n\nThe same is true with {RESULT}");
            else if (button.Name == "command_execute_type_i")
                MessageBox.Show("Parameter in plugin.json file is: command_execute_type \n\nSpecify how this command will be executed. If you specify 'My Custom Application with ignoring all output' or 'PowerShell with ignoring all output', specifying command_execute_results is useless. In these specific cases all the output from script or executable will be sent to you ignoring what you specified in command_execute_results");
            else if (button.Name == "command_allowed_users_id_i")
                MessageBox.Show("Parameter in plugin.json file is: command_allowed_users_id \n\nSpecify id of the Users, who can send use this command via direct chat (not group). \n\nWarning: the same principle as in telegram_allowed_chats_id and telegram_allowed_users_id");
            else if (button.Name == "command_allowed_chats_id_i")
                MessageBox.Show("Parameter in plugin.json file is: command_allowed_chats_id \n\nSpecify id of the chat Groups which have access to execute this command (not direct chat). \n\nWarning: the same principle as in telegram_allowed_chats_id and telegram_allowed_users_id");
            else if (button.Name == "command_show_in_get_commands_list_i")
                MessageBox.Show("Parameter in plugin.json file is: command_show_in_get_commands_list \n\nSpecify if this command will be shown in /getcommands list. You can create your own secret commands which no one will know");
            else if (button.Name == "command_execute_results_i")
                MessageBox.Show("Parameter in plugin.json file is: command_execute_results \n\nSpecify which your custom message will be shown to you, based on your script or executable answer. Actual only if 'My Custom Application with custom result answers' or 'PowerShell with custom result answers' have been selected\n\nExample: If your script or executable returns `0` or `1`, then there you can specify what to show to user if result was `0` or `1`");
            else if (button.Name == "result_checktype_i")
                MessageBox.Show("Parameter in plugin.json file in command_execute_results is: result_checktype \n\nSpecify how to check returned answer from your Script or Executable");
            else if (button.Name == "result_output_i")
                MessageBox.Show("Parameter in plugin.json file in command_execute_results is: result_output \n\nSpecify what to show to you if result check succeeded. \n\nWarning: {DATA} in the text will be replaced with the arguments which you specified to command. {RESULT} in the text will be replaced with the output from your script or executable.");
            else if (button.Name == "result_value_i")
                MessageBox.Show("Parameter in plugin.json file in command_execute_results is: result_value \n\nSpecify with what to check returned answer from your script or executable.");
        }
    }
}