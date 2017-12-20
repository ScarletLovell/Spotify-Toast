using System;
using System.Windows.Forms;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Local;
using System.Configuration;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace ToastTest
{
    public partial class Options : Form
    {
        private static SpotifyLocalAPI _spotify;
        public SpotifyLocalAPI spotify { get { return _spotify; } set { _spotify = value; } }

        public static Form1 form1;
        public Form1 _form1 { get { return form1; } set { form1 = value; } }

        /// <summary>The way to make the app force ontop of all apps</summary>
        protected override CreateParams CreateParams
        { // https://stackoverflow.com/a/26607006 , I couldn't figure this out on my own.
            get
            {
                var cp = base.CreateParams;
                if(JsonOptions.GetOption("alwaysOnTop") != "true") {
                    return cp;
                }
                cp.ExStyle |= 8;
                return cp;
            }
        }
        public Options() {
            InitializeComponent();
        }

        bool updatingChecks = true;
        private void Options_Load(object sender, EventArgs e) {
            if(JsonOptions.GetOption("alwaysOnTop") != "") {
                checkBox_alwaysOnTop.Checked = JsonOptions.GetOption("alwaysOnTop").Equals("true");
            }
            bool changeColorWithSong = false;
            if(JsonOptions.GetOption("changeColorWithSong") != "") {
                checkBox_changeColorWithSong.Checked = changeColorWithSong = JsonOptions.GetOption("changeColorWithSong").Equals("true");
            }
            if(changeColorWithSong) {
                button_selectColor.Visible = false;
                button_selectTextColor.Visible = false;
            } else {
                button_selectColor.Visible = true;
                button_selectTextColor.Visible = true;
            }
            bool toastMode = false;
            if(JsonOptions.GetOption("toastMode") != "") {
                checkBox_toastMode.Checked = toastMode = JsonOptions.GetOption("toastMode").Equals("true");
            }
            if(toastMode) {
                if(JsonOptions.GetOption("toastMode_fadeIn") != "") {
                    checkBox_toast_fadeIn.Visible = true;
                    checkBox_toast_fadeIn.Checked = JsonOptions.GetOption("toastMode_fadeIn").Equals("true");
                }
            } else {
                checkBox_toast_fadeIn.Visible = false;
            }
            if(JsonOptions.GetOption("showAmountOfPlays") != "") {
                checkBox_amountPlayed.Checked = JsonOptions.GetOption("showAmountOfPlays").Equals("true");
            }
            if(JsonOptions.GetOption("dontShowVersion") != "") {
                checkBox_showVersion.Checked = JsonOptions.GetOption("dontShowVersion").Equals("true");
            }
            if(JsonOptions.GetOption("textTicks") != "") {
                textBox_textTicks.Text = JsonOptions.GetOption("textTicks");
            }
            if(JsonOptions.GetOption("barHeight") != "") {
                int trackBar = int.Parse(JsonOptions.GetOption("barHeight"));
                trackBar_barHeight.Value = trackBar;
            }
            if(JsonOptions.GetOption("showSongBorder") != "") {
                checkBox_showSongBorder.Checked = JsonOptions.GetOption("showSongBorder").Equals("true");
            }
            if(JsonOptions.GetOption("disableBorder") != "") {
                checkBox_disableBorder.Checked = JsonOptions.GetOption("disableBorder").Equals("true");
            }
            textColor = JsonOptions.GetOption("defaultTextColor");
            color = JsonOptions.GetOption("defaultColor");
            barColor = JsonOptions.GetOption("barColor");
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
        private bool applyByRestart = false;
        private void ApplyOptions() {
            if(applyByRestart) {
                Application.Restart();
                return;
            }
            form1.load(true);
            this.Close();
        }

        private void SetSetting(string setting, string result) {
            JsonOptions.SetOption(setting, result);
            return;
        }
        private void checkBox_alwaysOnTop_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
            applyByRestart = true;
        }

        private void checkBox_changeColorWithSong_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_toastMode_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            bool check = checkBox_toastMode.Checked;
            if(check) {
                DialogResult result = MessageBox.Show("Are you sure? You will only be able to open the options by using the icon in the System Tray!", "Spotify Toast", MessageBoxButtons.YesNo);
                bool res = (result == DialogResult.Yes ? true : false);
                if(!res) {
                    checkBox_toastMode.Checked = false;
                    return;
                }
            }
            setCanUpdate(true);
        }

        private void checkBox_toast_fadeIn_CheckedChanged(object sender, EventArgs e) {
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
                DialogResult result = MessageBox.Show("Are you sure? You will only be able to open the options by using the icon in the System Tray!", "Spotify Toast", MessageBoxButtons.YesNo);
                bool res = (result == DialogResult.Yes ? true : false);
                if(!res) {
                    checkBox_showVersion.Checked = false;
                    return;
                }
            }
            setCanUpdate(true);
        }

        private void checkBox_disableBorder_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            bool check = checkBox_disableBorder.Checked;
            if(check) {
                DialogResult result = MessageBox.Show("Are you sure? You can only close the app by using the icon in the System Tray after!", "Spotify Toast", MessageBoxButtons.YesNo);
                bool res = (result == DialogResult.Yes ? true : false);
                if(!res) {
                    checkBox_disableBorder.Checked = false;
                    return;
                }
            }
            setCanUpdate(true);
        }

        private void checkBox_showSongBorder_CheckedChanged(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void checkBox_changeColorWithSong_CheckedChanged_1(object sender, EventArgs e) {
            if(updatingChecks)
                return;
            setCanUpdate(true);
        }

        private void trackBar_barHeight_Scroll(object sender, EventArgs e) {
            setCanUpdate(true);
        }

        string color = "";
        private void button_selectColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            if(colors.IsEmpty) {
                return;
            }
            color = colors.R + " " + colors.G + " " + colors.B;
            setCanUpdate(true);
        }

        string textColor = "";
        private void button_selectTextColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            if(colors.IsEmpty) {
                return;
            }
            textColor = colors.R + " " + colors.G + " " + colors.B;
            setCanUpdate(true);
        }

        string barColor = "";
        private void button_setBarColor_Click(object sender, EventArgs e) {
            colorDialog1.ShowDialog();
            Color colors = colorDialog1.Color;
            if(colors.IsEmpty) {
                return;
            }
            barColor = colors.R + " " + colors.G + " " + colors.B;
            setCanUpdate(true);
        }

        private void button_forceExit_Click(object sender, EventArgs e) {
            form1.close();
        }

        private void setCanUpdate(bool updateable) {
            button_applyAll.Enabled = updateable;
            button_applyAll.BackColor = (updateable ? Color.FromArgb(255, 255, 255, 255) : Color.FromArgb(255, 100, 100, 100));
        }

        private void button_applyAll_Click_1(object sender, EventArgs e) {
            JsonOptions.options.Clear();
            SetSetting("defaultColor", color);
            SetSetting("defaultTextColor", textColor);
            SetSetting("barColor", barColor);
            SetSetting("showSongBorder", checkBox_showSongBorder.Checked.ToString().ToLower());
            SetSetting("disableBorder", checkBox_disableBorder.Checked.ToString().ToLower());
            SetSetting("barHeight", trackBar_barHeight.Value.ToString());
            SetSetting("textTicks", textBox_textTicks.Text);
            SetSetting("showAmountOfPlays", checkBox_amountPlayed.Checked.ToString().ToLower());
            SetSetting("toastMode_fadeIn", checkBox_toastMode.Checked.ToString().ToLower());
            SetSetting("toastMode", checkBox_toastMode.Checked.ToString().ToLower());
            SetSetting("changeColorWithSong", checkBox_changeColorWithSong.Checked.ToString().ToLower());
            SetSetting("alwaysOnTop", checkBox_alwaysOnTop.Checked.ToString().ToLower());
            SetSetting("dontShowVersion", checkBox_showVersion.Checked.ToString().ToLower());
            JsonOptions.UpdateOptions();
            JsonOptions.options.Clear();
            ApplyOptions();
        }

        private void textBox_textTicks_KeyPress(object sender, KeyPressEventArgs e) {
            if(e.KeyChar == (char) Keys.Enter) {
                string t = textBox_textTicks.Text;
                string op = JsonOptions.GetOption("textTicks");
                if(op == "")
                    return;
                string val = op;
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
            JsonOptions.DeleteAll();
            //confFile.Clear();
            //configManager.Save(ConfigurationSaveMode.Modified);
            applyByRestart = true;
            ApplyOptions();
        }
    }
}
