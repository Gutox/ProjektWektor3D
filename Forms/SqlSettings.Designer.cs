namespace ProjektWektor3D.Forms
{
    partial class SqlSettings
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
            this.Host_textBox = new System.Windows.Forms.TextBox();
            this.Host_label = new System.Windows.Forms.Label();
            this.Database_label = new System.Windows.Forms.Label();
            this.Database_textBox = new System.Windows.Forms.TextBox();
            this.Username_label = new System.Windows.Forms.Label();
            this.Username_textBox = new System.Windows.Forms.TextBox();
            this.Password_label = new System.Windows.Forms.Label();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.Save_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Host_textBox
            // 
            this.Host_textBox.Location = new System.Drawing.Point(89, 7);
            this.Host_textBox.Name = "Host_textBox";
            this.Host_textBox.Size = new System.Drawing.Size(211, 20);
            this.Host_textBox.TabIndex = 0;
            this.Host_textBox.Text = "(local)";
            this.Host_textBox.TextChanged += new System.EventHandler(this.Host_textBox_TextChanged);
            // 
            // Host_label
            // 
            this.Host_label.AutoSize = true;
            this.Host_label.Location = new System.Drawing.Point(13, 10);
            this.Host_label.Name = "Host_label";
            this.Host_label.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.Host_label.Size = new System.Drawing.Size(32, 28);
            this.Host_label.TabIndex = 1;
            this.Host_label.Text = "Host:";
            // 
            // Database_label
            // 
            this.Database_label.AutoSize = true;
            this.Database_label.Location = new System.Drawing.Point(13, 38);
            this.Database_label.Name = "Database_label";
            this.Database_label.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.Database_label.Size = new System.Drawing.Size(56, 28);
            this.Database_label.TabIndex = 2;
            this.Database_label.Text = "Database:";
            // 
            // Database_textBox
            // 
            this.Database_textBox.Location = new System.Drawing.Point(89, 35);
            this.Database_textBox.Name = "Database_textBox";
            this.Database_textBox.Size = new System.Drawing.Size(211, 20);
            this.Database_textBox.TabIndex = 3;
            this.Database_textBox.Text = "master";
            // 
            // Username_label
            // 
            this.Username_label.AutoSize = true;
            this.Username_label.Location = new System.Drawing.Point(13, 69);
            this.Username_label.Name = "Username_label";
            this.Username_label.Padding = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.Username_label.Size = new System.Drawing.Size(58, 28);
            this.Username_label.TabIndex = 4;
            this.Username_label.Text = "Username:";
            this.Username_label.Click += new System.EventHandler(this.label1_Click);
            // 
            // Username_textBox
            // 
            this.Username_textBox.Location = new System.Drawing.Point(89, 66);
            this.Username_textBox.Name = "Username_textBox";
            this.Username_textBox.Size = new System.Drawing.Size(211, 20);
            this.Username_textBox.TabIndex = 5;
            this.Username_textBox.Text = "SA";
            // 
            // Password_label
            // 
            this.Password_label.AutoSize = true;
            this.Password_label.Location = new System.Drawing.Point(13, 97);
            this.Password_label.Name = "Password_label";
            this.Password_label.Size = new System.Drawing.Size(56, 13);
            this.Password_label.TabIndex = 6;
            this.Password_label.Text = "Password:";
            this.Password_label.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // Password_textBox
            // 
            this.Password_textBox.Location = new System.Drawing.Point(89, 94);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.PasswordChar = '*';
            this.Password_textBox.Size = new System.Drawing.Size(211, 20);
            this.Password_textBox.TabIndex = 7;
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(225, 125);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(75, 23);
            this.Save_button.TabIndex = 8;
            this.Save_button.Text = "Save";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // SqlSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 161);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Password_textBox);
            this.Controls.Add(this.Password_label);
            this.Controls.Add(this.Username_textBox);
            this.Controls.Add(this.Username_label);
            this.Controls.Add(this.Database_textBox);
            this.Controls.Add(this.Database_label);
            this.Controls.Add(this.Host_label);
            this.Controls.Add(this.Host_textBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlSettings";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.Text = "SQL Connection Settings";
            this.Load += new System.EventHandler(this.SqlSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Host_textBox;
        private System.Windows.Forms.Label Host_label;
        private System.Windows.Forms.Label Database_label;
        private System.Windows.Forms.TextBox Database_textBox;
        private System.Windows.Forms.Label Username_label;
        private System.Windows.Forms.TextBox Username_textBox;
        private System.Windows.Forms.Label Password_label;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Button Save_button;
    }
}