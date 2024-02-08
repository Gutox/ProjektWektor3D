using System;
using System.Windows.Forms;

namespace ProjektWektor3D.Forms
{
    public partial class SqlSettings : Form
    {
        public SqlSettings()
        {
            InitializeComponent();
        }

        private void SqlSettings_Load(object sender, EventArgs e)
        {
            try
            {
                Settings.Load();

                if (Settings.SQLSettings.Server != null)
                    Host_textBox.Text = Settings.SQLSettings.Server;

                if (Settings.SQLSettings.Database != null)
                    Database_textBox.Text = Settings.SQLSettings.Database;

                if (Settings.SQLSettings.Login != null)
                    Username_textBox.Text = Settings.SQLSettings.Login;

                if (Settings.SQLSettings.Password != null)
                    Password_textBox.Text = Settings.SQLSettings.Password;
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Exception while loading settings! Message: {exception.Message}");
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Host_textBox.Text))
            {
                MessageBox.Show(@"Host cannot be empty!");
                return;
            }

            if (string.IsNullOrEmpty(Database_textBox.Text))
            {
                MessageBox.Show(@"Database cannot be empty!");
                return;
            }

            if (string.IsNullOrEmpty(Username_textBox.Text))
            {
                MessageBox.Show(@"Username cannot be empty!");
                return;
            }


            try
            {
                Settings.SQLSettings.Server = Host_textBox.Text;
                Settings.SQLSettings.Database = Database_textBox.Text;
                Settings.SQLSettings.Login = Username_textBox.Text;
                Settings.SQLSettings.Password = Password_textBox.Text;

                Settings.Save();
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Exception while saving settings! Message: {exception.Message}");
            }

            Close();
        }


        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
        }

        private void Host_textBox_TextChanged(object sender, EventArgs e)
        {
        }
    }
}