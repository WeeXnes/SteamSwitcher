using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using Nocksoft.IO.ConfigFiles;


namespace SteamSwitcher
{
    public partial class Form1 : Form
    {
        string username = "";
        string autosave = "";
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SteamSwitcher");
        string filePath = "accounts.txt";
        const string userRoot = "HKEY_CURRENT_USER";
        const string subkey = "Software\\Valve\\Steam";
        const string keyName = userRoot + "\\" + subkey;


        public Form1()
        {
            InitializeComponent();
            PopulateList();
            InitializeConfig();
            listBox1.Click += new EventHandler(ListBox1_Click);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                using (TextWriter TW = new StreamWriter(path + "\\accounts.txt"))
                {
                    foreach (string itemText in listBox1.Items)
                    {
                        TW.WriteLine(itemText);
                    }
                    TW.Close();
                }


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();
            }
            username = textBox1.Text;
            Registry.SetValue(keyName, "AutoLoginUser", username,
            RegistryValueKind.ExpandString);
            Process.Start("steam://open/main");
            string thelist = listBox1.Text;
            string text = textBox1.Text;
            if (listBox1.Items.Contains(username))
            {

            }
            else
            {
                listBox1.Items.Add(username);
                if (checkBox1.Checked == true)
                {
                    if (listBox1.Items.Count > 0)
                    {
                        using (TextWriter TW = new StreamWriter(path + "\\accounts.txt"))
                        {
                            foreach (string itemText in listBox1.Items)
                            {
                                TW.WriteLine(itemText);
                            }
                            TW.Close();
                        }
                    }
                }

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void PopulateList()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string line;
            if (File.Exists(path + "\\accounts.txt"))
            {
                var file = new System.IO.StreamReader(path + "\\accounts.txt");
                while ((line = file.ReadLine()) != null)
                {
                    listBox1.Items.Add(line);
                }
                file.Close();
            }
            
        }
        public void InitializeConfig()
        {
            if (!File.Exists(path + "\\settings.ini"))
            {
                using (StreamWriter sw = File.CreateText(path + "\\settings.ini"))
                {
                    sw.WriteLine("[Settings]");
                    sw.WriteLine("auto=0");
                }
            }
            INIFile iniFile = new INIFile(path + "\\settings.ini");
            autosave = iniFile.GetValue("Settings", "auto");
            int autovalue = Convert.ToInt32(autosave);
            if(autovalue == 1)
            {
                checkBox1.Checked = true;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void ListBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Clear();
                var selectionIndex = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text.Insert(selectionIndex, listBox1.SelectedItem.ToString());
            }
        }
        public void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Clear();
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                textBox1.Clear();
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            if (listBox1.Items.Count > 0)
            {
                using (TextWriter TW = new StreamWriter(path + "\\accounts.txt"))
                {
                    foreach (string itemText in listBox1.Items)
                    {
                        TW.WriteLine(itemText);
                    }
                    TW.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            username = textBox1.Text;
            if (listBox1.Items.Contains(username))
            {

            }
            else
            {
                listBox1.Items.Add(username);
                if (checkBox1.Checked == true)
                {
                    if (listBox1.Items.Count > 0)
                    {
                        using (TextWriter TW = new StreamWriter(path + "\\accounts.txt"))
                        {
                            foreach (string itemText in listBox1.Items)
                            {
                                TW.WriteLine(itemText);
                            }
                            TW.Close();
                        }
                    }
                }

            }
        }

        private void checkBox1_CheckedChanged1(object sender, EventArgs e)
        {
            INIFile iniFile = new INIFile(path + "\\settings.ini");
            if (checkBox1.Checked == true)
            {
                iniFile.SetValue("Settings", "auto", "1");
            }
            else if (checkBox1.Checked == false)
            {
                iniFile.SetValue("Settings", "auto", "0");
            }
        }
    }
}
