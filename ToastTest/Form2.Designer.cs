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
            this.SuspendLayout();
            // 
            // checkBox_alwaysOnTop
            // 
            this.checkBox_alwaysOnTop.AutoSize = true;
            this.checkBox_alwaysOnTop.Checked = true;
            this.checkBox_alwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_alwaysOnTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_alwaysOnTop.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox_alwaysOnTop.Location = new System.Drawing.Point(13, 35);
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
            this.checkBox_changeColorWithSong.Location = new System.Drawing.Point(13, 12);
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
            this.button_selectColor.Location = new System.Drawing.Point(188, 8);
            this.button_selectColor.Name = "button_selectColor";
            this.button_selectColor.Size = new System.Drawing.Size(100, 23);
            this.button_selectColor.TabIndex = 6;
            this.button_selectColor.Text = "Select Color";
            this.button_selectColor.UseVisualStyleBackColor = false;
            this.button_selectColor.Click += new System.EventHandler(this.button_selectColor_Click);
            // 
            // button_selectTextColor
            // 
            this.button_selectTextColor.BackColor = System.Drawing.Color.White;
            this.button_selectTextColor.ForeColor = System.Drawing.Color.Black;
            this.button_selectTextColor.Location = new System.Drawing.Point(188, 37);
            this.button_selectTextColor.Name = "button_selectTextColor";
            this.button_selectTextColor.Size = new System.Drawing.Size(100, 23);
            this.button_selectTextColor.TabIndex = 7;
            this.button_selectTextColor.Text = "Select Text Color";
            this.button_selectTextColor.UseVisualStyleBackColor = false;
            this.button_selectTextColor.Click += new System.EventHandler(this.button2_Click);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.ClientSize = new System.Drawing.Size(292, 269);
            this.Controls.Add(this.button_selectTextColor);
            this.Controls.Add(this.button_selectColor);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox_changeColorWithSong);
            this.Controls.Add(this.checkBox_alwaysOnTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox_alwaysOnTop;
        private System.Windows.Forms.CheckBox checkBox_changeColorWithSong;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button_selectColor;
        private System.Windows.Forms.Button button_selectTextColor;
    }
}