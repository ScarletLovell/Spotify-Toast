namespace ToastTest
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBox_alwaysOnTop = new System.Windows.Forms.CheckBox();
            this.checkBox_changeColorWithSong = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.button_selectColor = new System.Windows.Forms.Button();
            this.button_selectTextColor = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox_toastMode = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_main = new System.Windows.Forms.TabPage();
            this.checkBox_amountPlayed = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_textTicks = new System.Windows.Forms.TextBox();
            this.tabPage_toast = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_ticks = new System.Windows.Forms.TextBox();
            this.checkBox_fadeIn = new System.Windows.Forms.CheckBox();
            this.tabPage_debug = new System.Windows.Forms.TabPage();
            this.button_forceExit = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage_main.SuspendLayout();
            this.tabPage_toast.SuspendLayout();
            this.tabPage_debug.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_alwaysOnTop
            // 
            this.checkBox_alwaysOnTop.AutoSize = true;
            this.checkBox_alwaysOnTop.Checked = true;
            this.checkBox_alwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_alwaysOnTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_alwaysOnTop.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox_alwaysOnTop.Location = new System.Drawing.Point(6, 6);
            this.checkBox_alwaysOnTop.Name = "checkBox_alwaysOnTop";
            this.checkBox_alwaysOnTop.Size = new System.Drawing.Size(98, 17);
            this.checkBox_alwaysOnTop.TabIndex = 3;
            this.checkBox_alwaysOnTop.Text = "Always On Top";
            this.checkBox_alwaysOnTop.UseVisualStyleBackColor = true;
            this.checkBox_alwaysOnTop.CheckedChanged += new System.EventHandler(this.checkBox_alwaysOnTop_CheckedChanged);
            // 
            // checkBox_changeColorWithSong
            // 
            this.checkBox_changeColorWithSong.AutoSize = true;
            this.checkBox_changeColorWithSong.Checked = true;
            this.checkBox_changeColorWithSong.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_changeColorWithSong.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_changeColorWithSong.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox_changeColorWithSong.Location = new System.Drawing.Point(6, 128);
            this.checkBox_changeColorWithSong.Name = "checkBox_changeColorWithSong";
            this.checkBox_changeColorWithSong.Size = new System.Drawing.Size(169, 17);
            this.checkBox_changeColorWithSong.TabIndex = 4;
            this.checkBox_changeColorWithSong.Text = "Change color on song change";
            this.checkBox_changeColorWithSong.UseVisualStyleBackColor = true;
            this.checkBox_changeColorWithSong.CheckedChanged += new System.EventHandler(this.checkBox_changeColorWithSong_CheckedChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(0, 241);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(292, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "Close Window";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_selectColor
            // 
            this.button_selectColor.BackColor = System.Drawing.Color.White;
            this.button_selectColor.ForeColor = System.Drawing.Color.Black;
            this.button_selectColor.Location = new System.Drawing.Point(110, 151);
            this.button_selectColor.Name = "button_selectColor";
            this.button_selectColor.Size = new System.Drawing.Size(98, 23);
            this.button_selectColor.TabIndex = 6;
            this.button_selectColor.Text = "Select Main Color";
            this.button_selectColor.UseVisualStyleBackColor = false;
            this.button_selectColor.Click += new System.EventHandler(this.button_selectColor_Click);
            // 
            // button_selectTextColor
            // 
            this.button_selectTextColor.BackColor = System.Drawing.Color.White;
            this.button_selectTextColor.ForeColor = System.Drawing.Color.Black;
            this.button_selectTextColor.Location = new System.Drawing.Point(6, 151);
            this.button_selectTextColor.Name = "button_selectTextColor";
            this.button_selectTextColor.Size = new System.Drawing.Size(98, 23);
            this.button_selectTextColor.TabIndex = 7;
            this.button_selectTextColor.Text = "Select Text Color";
            this.button_selectTextColor.UseVisualStyleBackColor = false;
            this.button_selectTextColor.Click += new System.EventHandler(this.button_selectTextColor_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(9, 214);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(276, 21);
            this.button2.TabIndex = 8;
            this.button2.Text = "Reset All";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // checkBox_toastMode
            // 
            this.checkBox_toastMode.AutoSize = true;
            this.checkBox_toastMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_toastMode.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox_toastMode.Location = new System.Drawing.Point(6, 6);
            this.checkBox_toastMode.Name = "checkBox_toastMode";
            this.checkBox_toastMode.Size = new System.Drawing.Size(95, 17);
            this.checkBox_toastMode.TabIndex = 10;
            this.checkBox_toastMode.Text = "Toast Enabled";
            this.checkBox_toastMode.UseVisualStyleBackColor = true;
            this.checkBox_toastMode.CheckedChanged += new System.EventHandler(this.checkBox_toastMode_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage_main);
            this.tabControl1.Controls.Add(this.tabPage_toast);
            this.tabControl1.Controls.Add(this.tabPage_debug);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(292, 209);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage_main
            // 
            this.tabPage_main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.tabPage_main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPage_main.Controls.Add(this.checkBox_amountPlayed);
            this.tabPage_main.Controls.Add(this.label2);
            this.tabPage_main.Controls.Add(this.textBox_textTicks);
            this.tabPage_main.Controls.Add(this.checkBox_changeColorWithSong);
            this.tabPage_main.Controls.Add(this.checkBox_alwaysOnTop);
            this.tabPage_main.Controls.Add(this.button_selectColor);
            this.tabPage_main.Controls.Add(this.button_selectTextColor);
            this.tabPage_main.Location = new System.Drawing.Point(4, 25);
            this.tabPage_main.Name = "tabPage_main";
            this.tabPage_main.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_main.Size = new System.Drawing.Size(284, 180);
            this.tabPage_main.TabIndex = 0;
            this.tabPage_main.Text = "Main";
            // 
            // checkBox_amountPlayed
            // 
            this.checkBox_amountPlayed.AutoSize = true;
            this.checkBox_amountPlayed.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_amountPlayed.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox_amountPlayed.Location = new System.Drawing.Point(6, 29);
            this.checkBox_amountPlayed.Name = "checkBox_amountPlayed";
            this.checkBox_amountPlayed.Size = new System.Drawing.Size(175, 17);
            this.checkBox_amountPlayed.TabIndex = 16;
            this.checkBox_amountPlayed.Text = "Show amount played on a song";
            this.checkBox_amountPlayed.UseVisualStyleBackColor = true;
            this.checkBox_amountPlayed.CheckedChanged += new System.EventHandler(this.checkBox_amountPlayed_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(177, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Text Ticks:";
            // 
            // textBox_textTicks
            // 
            this.textBox_textTicks.Location = new System.Drawing.Point(243, 6);
            this.textBox_textTicks.MaxLength = 5;
            this.textBox_textTicks.Name = "textBox_textTicks";
            this.textBox_textTicks.Size = new System.Drawing.Size(35, 20);
            this.textBox_textTicks.TabIndex = 14;
            this.textBox_textTicks.Text = "250";
            this.textBox_textTicks.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_textTicks_KeyPress);
            // 
            // tabPage_toast
            // 
            this.tabPage_toast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.tabPage_toast.Controls.Add(this.label1);
            this.tabPage_toast.Controls.Add(this.textBox_ticks);
            this.tabPage_toast.Controls.Add(this.checkBox_fadeIn);
            this.tabPage_toast.Controls.Add(this.checkBox_toastMode);
            this.tabPage_toast.Location = new System.Drawing.Point(4, 25);
            this.tabPage_toast.Name = "tabPage_toast";
            this.tabPage_toast.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_toast.Size = new System.Drawing.Size(284, 180);
            this.tabPage_toast.TabIndex = 1;
            this.tabPage_toast.Text = "Toast Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(201, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Ticks:";
            // 
            // textBox_ticks
            // 
            this.textBox_ticks.Location = new System.Drawing.Point(243, 6);
            this.textBox_ticks.MaxLength = 5;
            this.textBox_ticks.Name = "textBox_ticks";
            this.textBox_ticks.Size = new System.Drawing.Size(35, 20);
            this.textBox_ticks.TabIndex = 12;
            this.textBox_ticks.Text = "65";
            // 
            // checkBox_fadeIn
            // 
            this.checkBox_fadeIn.AutoSize = true;
            this.checkBox_fadeIn.ForeColor = System.Drawing.Color.White;
            this.checkBox_fadeIn.Location = new System.Drawing.Point(6, 29);
            this.checkBox_fadeIn.Name = "checkBox_fadeIn";
            this.checkBox_fadeIn.Size = new System.Drawing.Size(62, 17);
            this.checkBox_fadeIn.TabIndex = 11;
            this.checkBox_fadeIn.Text = "Fade In";
            this.checkBox_fadeIn.UseVisualStyleBackColor = true;
            this.checkBox_fadeIn.CheckedChanged += new System.EventHandler(this.checkBox_fadeIn_CheckedChanged);
            // 
            // tabPage_debug
            // 
            this.tabPage_debug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.tabPage_debug.Controls.Add(this.button_forceExit);
            this.tabPage_debug.Location = new System.Drawing.Point(4, 25);
            this.tabPage_debug.Name = "tabPage_debug";
            this.tabPage_debug.Size = new System.Drawing.Size(284, 180);
            this.tabPage_debug.TabIndex = 2;
            this.tabPage_debug.Text = "Debug";
            // 
            // button_forceExit
            // 
            this.button_forceExit.BackColor = System.Drawing.Color.White;
            this.button_forceExit.ForeColor = System.Drawing.Color.Black;
            this.button_forceExit.Location = new System.Drawing.Point(105, 154);
            this.button_forceExit.Name = "button_forceExit";
            this.button_forceExit.Size = new System.Drawing.Size(75, 23);
            this.button_forceExit.TabIndex = 0;
            this.button_forceExit.Text = "Force Exit";
            this.button_forceExit.UseVisualStyleBackColor = false;
            this.button_forceExit.Click += new System.EventHandler(this.button_forceExit_Click);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.ClientSize = new System.Drawing.Size(292, 269);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_main.ResumeLayout(false);
            this.tabPage_main.PerformLayout();
            this.tabPage_toast.ResumeLayout(false);
            this.tabPage_toast.PerformLayout();
            this.tabPage_debug.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox_alwaysOnTop;
        private System.Windows.Forms.CheckBox checkBox_changeColorWithSong;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button_selectColor;
        private System.Windows.Forms.Button button_selectTextColor;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox_toastMode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_main;
        private System.Windows.Forms.TabPage tabPage_toast;
        private System.Windows.Forms.CheckBox checkBox_fadeIn;
        private System.Windows.Forms.TabPage tabPage_debug;
        private System.Windows.Forms.Button button_forceExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_ticks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_textTicks;
        private System.Windows.Forms.CheckBox checkBox_amountPlayed;
    }
}