using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using ProjektWektor3D.FileHelpers;
using ProjektWektor3D.SQL;

namespace ProjektWektor3D.Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Dictionary<string, Vector3D> Vectors { get; set; } = new Dictionary<string, Vector3D>();

        public Commands Commands { get; set; } = new Commands();


        private void Form1_Load(object sender, EventArgs e)
        {
            Settings.Filename = "settings.json";
            Settings.Load();
        }

        private void RefreshDataBetweenVectorsAndGrid()
        {
            dataGridView1.EndEdit();
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            foreach (var vector in Vectors)
                dataGridView1.Rows.Add(vector.Key, vector.Value.X, vector.Value.Y, vector.Value.Z);
            dataGridView1.Refresh();
        }

        private void SaveVectorsFromGrid()
        {
            Vectors = new Dictionary<string, Vector3D>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null || string.IsNullOrEmpty(row.Cells[0].Value.ToString()))
                    continue;

                if (row.Cells[1].Value == null || row.Cells[2].Value == null || row.Cells[3].Value == null)
                    continue;

                Vectors[row.Cells[0].Value.ToString()] = new Vector3D((double)row.Cells[1].Value,
                    (double)row.Cells[2].Value, (double)row.Cells[3].Value);
            }
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex != 0)
            {
                if (e.Value == null)
                {
                    e.Value = 0.0;
                    e.ParsingApplied = true;
                }
                else
                {
                    try
                    {
                        var value = e.Value.ToString().Replace(',', '.');
                        e.Value = double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
                        e.ParsingApplied = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(@"Invalid type. This row should have only floating point type");
                        e.Value = 0.0;
                        e.ParsingApplied = true;
                    }
                }
            }
        }

        private void DeleteRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dataGridView1.RowCount)
                return;

            // Can't delete
            if (dataGridView1.Rows[rowIndex].Cells[0].Value == null)
                dataGridView1.Rows[rowIndex].Cells[0].Value = "";

            if (dataGridView1.Rows[rowIndex].Cells[1].Value == null)
                dataGridView1.Rows[rowIndex].Cells[1].Value = 0.0;

            if (dataGridView1.Rows[rowIndex].Cells[2].Value == null)
                dataGridView1.Rows[rowIndex].Cells[2].Value = 0.0;

            if (dataGridView1.Rows[rowIndex].Cells[3].Value == null)
                dataGridView1.Rows[rowIndex].Cells[3].Value = 0.0;
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value == null ||
                string.IsNullOrEmpty(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()))
            {
                DeleteRow(e.RowIndex);
                return;
            }

            for (var i = 1; i < dataGridView1.ColumnCount; i++)
                if (dataGridView1.Rows[e.RowIndex].Cells[i].Value == null)
                {
                    MessageBox.Show(@"Invalid data or cell is empty!");
                    DeleteRow(e.RowIndex);
                    return;
                }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SaveVectorsFromGrid();
        }

        private void Commands_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                var command = Commands_textBox.Text;
                Commands_textBox.Text = "";

                if (command == "!h")
                {
                    var helpString = "Commands:\n";
                    helpString += "!h -> show help\n";
                    helpString += "!c -> clear console\n";

                    helpString += "v3 = v1 + v2 -> addition\n";
                    helpString += "v3 = v1 - v2 -> subtraction\n";
                    helpString += "v3 = v1 * scalar -> multiplication\n";
                    helpString += "v3 = v1 / scalar -> division\n";
                    helpString += "res = v1 . v2 -> dot product\n";
                    helpString += "v3 = v1 ^ v2 -> cross product\n";
                    helpString += "res = |v1| -> magnitude\n";
                    helpString += "v3 = Normalize(v1) -> normalize\n";
                    helpString += "res = Distance(v1, v2) -> distance\n";
                    helpString += "res = Angle(v1, v2) -> angle\n";
                    helpString += "v3 = Negation(v1) -> negation\n";


                    MessageBox.Show(helpString);
                }
                else if (command == "!c")
                {
                    History_listBox.Items.Clear();
                }
                else
                {
                    try
                    {
                        SaveVectorsFromGrid();

                        var result = Commands.ParseCommand(command, Vectors);
                        if (result == null)
                            throw new Exception("Result is null");

                        History_listBox.Items.Add($"{Commands.RemoveWhitespace(command)} ->\n {result}");

                        RefreshDataBetweenVectorsAndGrid();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($@"Exception while parsing command! Message: {exception.Message}");
                    }
                }
            }
        }

        private void sQLSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sqlSettings = new SqlSettings();
            sqlSettings.ShowDialog(this);
        }

        private void fromSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.SQLSettings.Server) ||
                string.IsNullOrEmpty(Settings.SQLSettings.Database) ||
                string.IsNullOrEmpty(Settings.SQLSettings.Login) ||
                string.IsNullOrEmpty(Settings.SQLSettings.Password))
            {
                MessageBox.Show(@"SQL settings are not set!");
                return;
            }


            try
            {
                var mssqlHelper = new MSSQLHelper(Settings.SQLSettings.Server);
                mssqlHelper.UseDatabase(Settings.SQLSettings.Database);

                mssqlHelper.Connect(Settings.SQLSettings.Login, Settings.SQLSettings.Password);

                var vectorHelper = new VectorsFromSQLHelper();
                if (!vectorHelper.CheckIfTableExists(mssqlHelper, "Vectors"))
                {
                    MessageBox.Show(@"Table Vectors does not exist!");
                    return;
                }

                Vectors = vectorHelper.ImportFromSQL(mssqlHelper, "Vectors");
                RefreshDataBetweenVectorsAndGrid();

                MessageBox.Show(@"Imported vectors from SQL!");
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Exception while importing vectors! Message: {exception.Message}");
            }
        }

        private void fromFileToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = @"CSV Files (*.csv)|*.csv|Excel Files|*.xlsx|JSON Files|*.json";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;

                    IFileHelper fileHelper = null;
                    if (filePath.EndsWith("csv"))
                        fileHelper = new CSVHelper();
                    else if (filePath.EndsWith("xlsx"))
                        fileHelper = new XLSXHelper();
                    else if (filePath.EndsWith("json"))
                        fileHelper = new JSONHelper();

                    if (fileHelper == null)
                    {
                        MessageBox.Show(@"Invalid file extension!");
                        return;
                    }

                    try
                    {
                        Vectors = fileHelper.ImportFromFile(filePath);
                        RefreshDataBetweenVectorsAndGrid();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($@"Exception while importing vectors! Message: {exception.Message}");
                    }
                }
            }
        }

        private void toSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.SQLSettings.Server) ||
                string.IsNullOrEmpty(Settings.SQLSettings.Database) ||
                string.IsNullOrEmpty(Settings.SQLSettings.Login) ||
                string.IsNullOrEmpty(Settings.SQLSettings.Password))
            {
                MessageBox.Show(@"SQL settings are not set!");
                return;
            }


            try
            {
                var mssqlHelper = new MSSQLHelper(Settings.SQLSettings.Server);
                mssqlHelper.UseDatabase(Settings.SQLSettings.Database);

                mssqlHelper.Connect(Settings.SQLSettings.Login, Settings.SQLSettings.Password);

                var vectorHelper = new VectorsFromSQLHelper();
                if (!vectorHelper.CheckIfTableExists(mssqlHelper, "Vectors"))
                    if (!vectorHelper.CreateSQLTable(mssqlHelper, "Vectors"))
                    {
                        MessageBox.Show(@"Failed to create table Vectors");
                        return;
                    }

                vectorHelper.ExportToSQL(mssqlHelper, "Vectors", Vectors);
                MessageBox.Show(@"Exported vectors to SQL!");
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"Exception while exporting vectors! Message: {exception.Message}");
            }
        }

        private void toFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = @"CSV Files (*.csv)|*.csv|Excel Files|*.xlsx|JSON Files|*.json";
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var filePath = saveFileDialog.FileName;

                    IFileHelper fileHelper = null;
                    if (filePath.EndsWith("csv"))
                        fileHelper = new CSVHelper();
                    else if (filePath.EndsWith("xlsx"))
                        fileHelper = new XLSXHelper();
                    else if (filePath.EndsWith("json"))
                        fileHelper = new JSONHelper();

                    if (fileHelper == null)
                    {
                        MessageBox.Show(@"Invalid file extension!");
                        return;
                    }

                    try
                    {
                        fileHelper.ExportToFile(filePath, Vectors);
                        MessageBox.Show($@"Exported vectors to file {filePath}!");
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show($@"Exception while exporting vectors! Message: {exception.Message}");
                    }
                }
            }
        }
    }
}