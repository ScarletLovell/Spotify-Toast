using System;
using System.Windows.Forms;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Local;
using System.Configuration;
using System.Drawing;

namespace ToastTest
{
    public partial class Options : Form
    {
        private static SpotifyLocalAPI _spotify;
        public SpotifyLocalAPI spotify { get { return _spotify; } set { _spotify = value; } }

        private static Form1 form1;
        public Form1 _form1 { get { return form1; } set { form1 = value; } }

        /// <summary>The way to make the app force ontop of all apps</summary>
        protected override CreateParams CreateParams
        { // https://stackoverflow.com/a/26607006 , I couldn't figure this out on my own.
            get
            {
                var cp = base.CreateParams;
                Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection confFile = configManager.AppSettings.Settings;
                if(confFile["alwaysOnTop"] != null && confFile["alwaysOnTop"].Value.ToLower().Equals("false")) {
                    return cp;
                }
                cp.ExStyle |= 8;
                return cp;
            }
        }
        public Options() {
            InitializeComponent();
        }

        static Configuration configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        KeyValueConfigurationCollection confFile = configManager.AppSettings.Settings;

        private void Options_Load(object sender, EventArgs e) {
            if(confFile["alwaysOnTop"] != null && confFile["alwaysOnTop"].Value.ToLower().Equals("false")) {
                updatingCheckBox_alwaysOnTop = true;
                checkBox_alwaysOnTop.Checked = false;
            }
            if(confFile["changeColorWithSong"] != null && confFile["changeColorWithSong"].Value.ToLower().Equals("false")) {
                updatingCheckBox_changeColorWithSong = true;
                checkBox_changeColorWithSong.Checked = false;
            } else {
                button_selectColor.Enabled = false;
                button_selectColor.Visible = false;
                button_selectTextColor.Enabled = false;
                button_selectTextColor.Visible = false;
            }
            if(confFile["toastMode"] != null && confFile["toastMode"].Value.ToLower().Equals("true")) {
                updatingCheckBox_toastMode = true;
                checkBox_toastMode.Checked = true;
            }
            Console.WriteLine("Options form loaded");
        }
        private bool toastMode = false;
        private void resetWindows() {
            Form1 oldForm1 = form1;
            oldForm1.Hide();
            form1 = new Form1();
            form1.isToast = toastMode;
            form1.newLocation = oldForm1.Location;
            this.Close();
            form1.ShowDialog();
            oldForm1.Close();
        }
        private bool updatingCheckBox_alwaysOnTop = false;
        private bool updatingCheckBox_changeColorWithSong = false;
        private bool updatingCheckBox_toastMode = false;
        private void checkBox_alwaysOnTop_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_alwaysOnTop) {
                updatingCheckBox_alwaysOnTop = false;
                return;
            }
            Console.WriteLine("Updated alwaysOnTop");
            if(confFile["alwaysOnTop"] != null)
                configManager.AppSettings.Settings.Remove("alwaysOnTop");
            configManager.AppSettings.Settings.Add("alwaysOnTop", checkBox_alwaysOnTop.Checked ? "true" : "false");
            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            resetWindows();
        }

        private void checkBox_changeColorWithSong_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_changeColorWithSong) {
                updatingCheckBox_changeColorWithSong = false;
                return;
            }
            Console.WriteLine("Updated changeColorWithSong");
            if(confFile["changeColorWithSong"] != null)
                configManager.AppSettings.Settings.Remove("changeColorWithSong");
            configManager.AppSettings.Settings.Add("changeColorWithSong", checkBox_changeColorWithSong.Checked ? "true" : "false");
            configManager.Save(ConfigurationSaveMode.Modified);
            resetWindows();
        }

        private void checkBox_toastMode_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_toastMode) {
                updatingCheckBox_toastMode = false;
                return;
            }
            if(confFile["toastMode"] != null)
                configManager.AppSettings.Settings.Remove("toastMode");
            configManager.AppSettings.Settings.Add("toastMode", checkBox_toastMode.Checked ? "true" : "false");
            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
            this.toastMode = checkBox_toastMode.Checked;
            resetWindows();
        }

        private void button_selectColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            if(confFile["defaultColor"] != null)
                configManager.AppSettings.Settings.Remove("defaultColor");
            Color colors = colorDialog1.Color;
            configManager.AppSettings.Settings.Add("defaultColor", colors.R + " " + colors.G + " " + colors.B);
            configManager.Save(ConfigurationSaveMode.Modified);
            resetWindows();
        }

        private void button_selectTextColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            if(confFile["defaultTextColor"] != null)
                configManager.AppSettings.Settings.Remove("defaultTextColor");
            Color colors = colorDialog1.Color;
            configManager.AppSettings.Settings.Add("defaultTextColor", colors.R + " " + colors.G + " " + colors.B);
            configManager.Save(ConfigurationSaveMode.Modified);
            resetWindows();
        }

        private void button2_Click_1(object sender, EventArgs e) {
            confFile.Clear();
            configManager.Save(ConfigurationSaveMode.Modified);
            resetWindows();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
