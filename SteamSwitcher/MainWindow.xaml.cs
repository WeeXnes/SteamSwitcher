using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace SteamSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadAccountsList();
        }

        public void addAccount(string _account)
        {
            if (!String.IsNullOrEmpty(_account))
            {
                if (!AccoutListView.Items.Contains(_account))
                {
                    AccoutListView.Items.Add(_account);
                    saveAccounts();
                }
            }
        }
        public void switchAccount(string _account)
        {
            if (!String.IsNullOrEmpty(_account))
            {
                foreach (var process in Process.GetProcessesByName("steam"))
                {
                    process.Kill();
                }
                Registry.SetValue(
                    Variables.keyName, 
                    "AutoLoginUser", 
                    _account,
                    RegistryValueKind.ExpandString);
                Process.Start("steam://open/main");
            }
        }

        public void saveAccounts()
        {
            using (TextWriter TW = new StreamWriter(Variables.AppDataPath + "\\" + Variables.AccountFile))
            {
                foreach (string itemText in AccoutListView.Items)
                {
                    TW.WriteLine(itemText);
                }
                TW.Close();
            }
        }
        public void LoadAccountsList()
        {
            if (!Directory.Exists(Variables.AppDataPath))
            {
                Directory.CreateDirectory(Variables.AppDataPath);
            }
            
            if (File.Exists(Variables.AppDataPath + "\\" + Variables.AccountFile))
            {
                foreach (string str in IOfuncs.readFile(Variables.AppDataPath + "\\" + Variables.AccountFile))
                {
                    AccoutListView.Items.Add(str);
                }
            }
        }
        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            /*
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
            */
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_AddAccount_OnClick(object sender, RoutedEventArgs e)
        {
            addAccount(tb_AccountName.Text);
            tb_AccountName.Clear();
        }

        private void Btn_SwitchAccount_OnClick(object sender, RoutedEventArgs e)
        {
            if (AccoutListView.SelectedItem != null)
            {
                string selectedAccount = (string)AccoutListView.SelectedItem;
                switchAccount(selectedAccount);
            }
        }

        private void AccoutListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (AccoutListView.SelectedItem != null)
            {
                AccoutListView.Items.Remove(AccoutListView.SelectedItem);
                saveAccounts();
            }
        }

        private void Tb_AccountName_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                addAccount(tb_AccountName.Text);
                tb_AccountName.Clear();
            }
        }
    }

    public static class IOfuncs
    {
        public static string[] readFile(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            var listOfStrings = new List<string>();
            foreach (string line in lines)
            {
                listOfStrings.Add(line);
            }
            string[] arrayOfStrings = listOfStrings.ToArray();
            return arrayOfStrings;
        }
        public static void writeFile(string[] stringArray, string filepath)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                Console.WriteLine(stringArray[i]);
            }
            File.WriteAllLines(filepath, stringArray, Encoding.UTF8);
        }
    }

    public static class Variables
    {
        public static string AppDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeeXnes");
        public static string AccountFile = "accounts.wx";
        public const string userRoot = "HKEY_CURRENT_USER";
        public const string subkey = "Software\\Valve\\Steam";
        public const string keyName = userRoot + "\\" + subkey;
    }
}