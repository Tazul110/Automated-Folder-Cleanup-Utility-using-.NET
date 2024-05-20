using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OpenFolder
{
    public partial class Opendirectory : Form
    {
        private string selectedFolderPath;

        List<string> lstDirectory =new List<string>();
        public Opendirectory()
        {
            InitializeComponent();
        }

        private void Opendirectory_Load(object sender, EventArgs e)
        {
            // Set the TextBox text to the selected folder path if it's not null or empty
            /*if (!string.IsNullOrEmpty(selectedFolderPath))
            {
                textBox1.Text = selectedFolderPath;
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                DialogResult result = folder.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folder.SelectedPath))
                {
                    selectedFolderPath = folder.SelectedPath;
                    lstDirectory.Add(selectedFolderPath);
                    textBox1.Text = selectedFolderPath;
                }
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                MessageBox.Show("Please select a folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox2.Text, out int days))
            {
                MessageBox.Show("Please enter a valid number of days.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime thresholdDate = DateTime.Now.AddDays(-days);

            try
            {
                int cnt = 0;
                foreach (string folder in lstDirectory)
                {
                    var files = Directory.GetFiles(folder);
                   
                    foreach (var file in files)
                    {
                        if (File.GetLastWriteTime(file) < thresholdDate)
                        {
                            cnt++;
                            File.Delete(file);
                        }
                    }
                }
               

                MessageBox.Show($"{cnt} Files older than the specified number of days have been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("{cnt Files older than the specified number of days have been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                selectedFolderPath = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string text = textBox.Text;

            // Use regular expression to remove non-numeric characters
            text = Regex.Replace(text, "[^0-9]", "");

            // Update the text in the TextBox
            textBox.Text = text;

            // Move the caret to the end of the text
            textBox.SelectionStart = textBox.Text.Length;
        }

      
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFolderPath))
            {
                MessageBox.Show("Please select a folder first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;

            if (startDate.Date > endDate.Date)
            {
                MessageBox.Show("The start date must be earlier than the end date.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int cnt = 0;
                foreach (var folder in lstDirectory)
                {
                    var files = Directory.GetFiles(folder);
                    
                    foreach (var file in files)
                    {
                        DateTime fileDate = File.GetLastWriteTime(file);
                        if (fileDate.Date >= startDate.Date && fileDate.Date <= endDate.Date)
                        {
                            cnt++;
                            File.Delete(file);
                        }
                    }
                }
                

                MessageBox.Show($"{cnt} Files between the specified dates have been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Clear();
                textBox2.Clear();
                selectedFolderPath = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
