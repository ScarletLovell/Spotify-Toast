namespace ToastTest
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.text_songName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.text_artistName = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.text_amountOfPlays = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // text_songName
            // 
            this.text_songName.AutoSize = true;
            this.text_songName.Font = new System.Drawing.Font("Nirmala UI Semilight", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_songName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.text_songName.Location = new System.Drawing.Point(75, 9);
            this.text_songName.Name = "text_songName";
            this.text_songName.Size = new System.Drawing.Size(112, 28);
            this.text_songName.TabIndex = 0;
            this.text_songName.Text = "Song Name";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 64);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // text_artistName
            // 
            this.text_artistName.AutoSize = true;
            this.text_artistName.Font = new System.Drawing.Font("Nirmala UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_artistName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.text_artistName.Location = new System.Drawing.Point(78, 38);
            this.text_artistName.Name = "text_artistName";
            this.text_artistName.Size = new System.Drawing.Size(55, 12);
            this.text_artistName.TabIndex = 2;
            this.text_artistName.Text = "Artist Name";
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Font = new System.Drawing.Font("Nirmala UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_version.ForeColor = System.Drawing.Color.DimGray;
            this.label_version.Location = new System.Drawing.Point(246, 55);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(45, 13);
            this.label_version.TabIndex = 4;
            this.label_version.Text = "version";
            this.label_version.Click += new System.EventHandler(this.label3_click);
            this.label_version.MouseEnter += new System.EventHandler(this.label3_mouseEnter);
            this.label_version.MouseLeave += new System.EventHandler(this.label3_mouseLeave);
            // 
            // text_amountOfPlays
            // 
            this.text_amountOfPlays.AutoSize = true;
            this.text_amountOfPlays.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.text_amountOfPlays.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.text_amountOfPlays.Location = new System.Drawing.Point(270, 2);
            this.text_amountOfPlays.Name = "text_amountOfPlays";
            this.text_amountOfPlays.Size = new System.Drawing.Size(10, 12);
            this.text_amountOfPlays.TabIndex = 5;
            this.text_amountOfPlays.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Location = new System.Drawing.Point(239, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Plays:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(292, 72);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.text_amountOfPlays);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.text_artistName);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.text_songName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label text_songName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label text_artistName;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Label text_amountOfPlays;
        private System.Windows.Forms.Label label1;
    }
}

