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

        bool updatingChecks = true;
        private void Options_Load(object sender, EventArgs e) {
            if(confFile["alwaysOnTop"] != null) {
                checkBox_alwaysOnTop.Checked = confFile["alwaysOnTop"].Value.ToLower().Equals("true");
            }
            if(confFile["changeColorWithSong"] != null) {
                checkBox_changeColorWithSong.Checked = confFile["changeColorWithSong"].Value.ToLower().Equals("true");
            } else {
                button_selectColor.Enabled = false;
                button_selectColor.Visible = false;
                button_selectTextColor.Enabled = false;
                button_selectTextColor.Visible = false;
            }
            if(confFile["toastMode"] != null) {
                checkBox_toastMode.Checked = confFile["toastMode"].Value.ToLower().Equals("true");
            }
            if(confFile["showAmountOfPlays"] != null) {
                checkBox_amountPlayed.Checked = confFile["showAmountOfPlays"].Value.ToLower().Equals("true");
            }
            if(confFile["showVersion"] != null) {
                checkBox_showVersion.Checked = confFile["showVersion"].Value.ToLower().Equals("true");
            }
            if(confFile["textTicks"] != null) {
                textBox_textTicks.Text = confFile["textTicks"].Value;
            }
            updatingChecks = false;
            //int i = 0;
            /*foreach(Form1.Song song in Form1.songHistory) {
                i += 1;
                //radioButton_history[i] = song.name;
            }*/
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
        private void checkBox_alwaysOnTop_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_changeColorWithSong_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_toastMode_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_fadeIn_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_amountPlayed_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_showVersion_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            bool check = checkBox_showVersion.Checked;
            if(check) {
                DialogResult result = MessageBox.Show("Are you sure? Disabling this will only let you see the options menu via the icon in the system tray", "Spotify Toast", MessageBoxButtons.YesNo);
                bool res = (result == DialogResult.Yes ? true : false);
                if(!res) {
                    checkBox_showVersion.Checked = false;
                    return;
                }
            }
            setCanUpdate(true);
        }

        private void button_selectColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            SetSetting("defaultColor", colors.R + " " + colors.G + " " + colors.B);
        }

        private void button_selectTextColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            SetSetting("defaultTextColor", colors.R + " " + colors.G + " " + colors.B);
        }

        private void button_forceExit_Click(object sender, EventArgs e) {
            form1.close();
        }

        private void setCanUpdate(bool updateable) {
            button_applyAll.Enabled = updateable;
            button_applyAll.BackColor = (updateable ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(255, 100, 100, 100));
        }

        private void button_applyAll_Click_1(object sender, EventArgs e) {
            SetSetting("textTicks", textBox_textTicks.Text);
            SetSetting("showAmountOfPlays", checkBox_amountPlayed.Checked.ToString().ToLower());
            SetSetting("toastMode_fadeIn", checkBox_toastMode.Checked.ToString().ToLower());
            SetSetting("toastMode", checkBox_toastMode.Checked.ToString().ToLower());
            SetSetting("changeColorWithSong", checkBox_changeColorWithSong.Checked.ToString().ToLower());
            SetSetting("alwaysOnTop", checkBox_alwaysOnTop.Checked.ToString().ToLower());
            SetSetting("showVersion", checkBox_showVersion.Checked.ToString().ToLower());
            ApplyOptions();
        }

        private void textBox_textTicks_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == (char) Keys.Enter) {
                string t = textBox_textTicks.Text;
                if(confFile["textTicks"] == null)
                    return;
                string val = confFile["textTicks"].Value;
                int ticks = 250;
                if(!int.TryParse(t, out ticks)) {
                    textBox_textTicks.Text = val;
                    return;
                }
                if(ticks < 10) {
                    ticks = 10;
                    textBox_textTicks.Text = val;
                }
                this.ActiveControl = null;
                e.Handled = true;
                setCanUpdate(true);
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void button_resetAll_Click(object sender, EventArgs e) {
            confFile.Clear();
            configManager.Save(ConfigurationSaveMode.Modified);
            ApplyOptions();
        }
    }
}
