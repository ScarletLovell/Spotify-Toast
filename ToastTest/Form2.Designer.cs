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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkbox_showVersion = new System.Windows.Forms.CheckBox();
            this.checkBox_alwaysOnTop = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(205, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Accept";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(13, 234);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // checkbox_showVersion
            // 
            this.checkbox_showVersion.AutoSize = true;
            this.checkbox_showVersion.Checked = true;
            this.checkbox_showVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkbox_showVersion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkbox_showVersion.ForeColor = System.Drawing.SystemColors.Control;
            this.checkbox_showVersion.Location = new System.Drawing.Point(13, 35);
            this.checkbox_showVersion.Name = "checkbox_showVersion";
            this.checkbox_showVersion.Size = new System.Drawing.Size(90, 17);
            this.checkbox_showVersion.TabIndex = 2;
            this.checkbox_showVersion.Text = "Show version";
            this.checkbox_showVersion.UseVisualStyleBackColor = true;
            // 
            // checkBox_alwaysOnTop
            // 
            this.checkBox_alwaysOnTop.AutoSize = true;
            this.checkBox_alwaysOnTop.Checked = true;
            this.checkBox_alwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_alwaysOnTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBox_alwaysOnTop.ForeColor = System.Drawing.SystemColors.Control;
            this.checkBox_alwaysOnTop.Location = new System.Drawing.Point(12, 58);
            this.checkBox_alwaysOnTop.Name = "checkBox_alwaysOnTop";
            this.checkBox_alwaysOnTop.Size = new System.Drawing.Size(98, 17);
            this.checkBox_alwaysOnTop.TabIndex = 3;
            this.checkBox_alwaysOnTop.Text = "Always On Top";
            this.checkBox_alwaysOnTop.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.ClientSize = new System.Drawing.Size(292, 269);
            this.Controls.Add(this.checkBox_alwaysOnTop);
            this.Controls.Add(this.checkbox_showVersion);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.Options_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkbox_showVersion;
        private System.Windows.Forms.CheckBox checkBox_alwaysOnTop;
    }
}