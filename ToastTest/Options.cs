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
            if(confFile["showAmountOfPlays"] != null && confFile["showAmountOfPlays"].Value.ToLower().Equals("true")) {
                updatingCheckBox_showAmount = true;
                checkBox_amountPlayed.Checked = true;
            }
            if(confFile["textTicks"] != null && confFile["textTicks"].Value != null) {
                int ticks = 250;
                if(!int.TryParse(confFile["textTicks"].Value, out ticks))
                    ticks = 250;
                textBox_textTicks.Text = ticks.ToString();
            }
            Console.WriteLine("Options form loaded");
        }
        private bool toastMode = false;
        private bool fadeIn = false;
        private void ApplyOptions() {
            Application.Restart();
        }

        private void SetSetting(string setting, string result) {
            if(confFile[setting] != null)
                configManager.AppSettings.Settings.Remove(setting);
            configManager.AppSettings.Settings.Add(setting, result);
            configManager.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configManager.AppSettings.SectionInformation.Name);
        }
        private bool updatingCheckBox_alwaysOnTop = false;
        private bool updatingCheckBox_changeColorWithSong = false;
        private bool updatingCheckBox_toastMode = false;
        private bool updatingCheckBox_fadeIn = false;
        private bool updatingCheckBox_showAmount = false;
        private void checkBox_alwaysOnTop_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_alwaysOnTop) {
                updatingCheckBox_alwaysOnTop = false;
                return;
            }
            SetSetting("alwaysOnTop", checkBox_alwaysOnTop.Checked ? "true" : "false");
            ApplyOptions();
        }

        private void checkBox_changeColorWithSong_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_changeColorWithSong) {
                updatingCheckBox_changeColorWithSong = false;
                return;
            }
            SetSetting("changeColorWithSong", checkBox_changeColorWithSong.Checked ? "true" : "false");
            ApplyOptions();
        }

        private void checkBox_toastMode_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_toastMode) {
                updatingCheckBox_toastMode = false;
                return;
            }
            SetSetting("toastMode", checkBox_toastMode.Checked ? "true" : "false");
            ApplyOptions();
        }

        private void checkBox_fadeIn_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_fadeIn) {
                updatingCheckBox_fadeIn = false;
                return;
            }
            SetSetting("toastMode_fadeIn", checkBox_toastMode.Checked ? "true" : "false");
            ApplyOptions();
        }

        private void checkBox_amountPlayed_CheckedChanged(object sender, EventArgs e) {
            if(updatingCheckBox_showAmount) {
                updatingCheckBox_showAmount = false;
                return;
            }
            SetSetting("showAmountOfPlays", checkBox_amountPlayed.Checked ? "true" : "false");
            ApplyOptions();
        }

        private void button_selectColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            SetSetting("defaultColor", colors.R + " " + colors.G + " " + colors.B);
            ApplyOptions();
        }

        private void button_selectTextColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            configManager.AppSettings.Settings.Add("defaultTextColor", colors.R + " " + colors.G + " " + colors.B);
            ApplyOptions();
        }

        private void button_forceExit_Click(object sender, EventArgs e) {
            form1.close();
        }

        private void button2_Click_1(object sender, EventArgs e) {
            confFile.Clear();
            configManager.Save(ConfigurationSaveMode.Modified);
            ApplyOptions();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void textBox_textTicks_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == (char) Keys.Enter) {
                //e.Handled = true;
                string t = textBox_textTicks.Text;
                if(confFile["textTicks"] != null && confFile["textTicks"].Value != null && t == confFile["textTicks"].Value) {
                    return;
                }
                int ticks = 250;
                if(!int.TryParse(t, out ticks)) {
                    ticks = 250;
                    textBox_textTicks.Text = ticks.ToString();
                    return;
                }
                configManager.AppSettings.Settings.Add("textTicks", ticks.ToString());
                ApplyOptions();
            }
        }
    }
}
