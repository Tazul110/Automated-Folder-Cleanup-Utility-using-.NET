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
        List<string> lstDirectory = new List<string>();

        public Opendirectory()
        {
            InitializeComponent();
        }

        private void Opendirectory_Load(object sender, EventArgs e)
        {
            // Initialization logic if needed
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
                    listBox1.Items.Add(selectedFolderPath); // Add the selected path to the ListBox
                    textBox1.Text = selectedFolderPath;
                    checkedListBox1.Items.Add(selectedFolderPath); // Add the selected path to the CheckedListBox
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
                        if (File.GetLastWriteTime(file) < thresholdDate &&  (checkedListBox1.CheckedItems.Contains(folder) && File.Exists(file)))
                        {
                            cnt++;
                            File.Delete(file);
                        }
                        
                    }
                    // Remove the folder from the CheckedListBox if it was checked and files were deleted
                    /*if (checkedListBox1.CheckedItems.Contains(folder))
                    {
                        checkedListBox1.Items.Remove(folder);      
                    }*/
                }

                MessageBox.Show($"{cnt} Files older than the specified number of days have been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                checkedListBox1.Items.Clear();
                listBox1.Items.Clear(); // Clear all items in the ListBox
                lstDirectory.Clear(); // Clear the list of directories
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
            // Handle dateTimePicker1 value changed event
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            // Handle dateTimePicker2 value changed event
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
                        if ((fileDate.Date >= startDate.Date && fileDate.Date <= endDate.Date) && (checkedListBox1.CheckedItems.Contains(folder) && File.Exists(file)))
                        {
                            cnt++;
                            File.Delete(file);
                        }
                    }
                }

                MessageBox.Show($"{cnt} Files between the specified dates have been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Clear();
                textBox2.Clear();
                checkedListBox1.Items.Clear();
                listBox1.Items.Clear(); // Clear all items in the ListBox
                lstDirectory.Clear(); // Clear the list of directories
                selectedFolderPath = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedPath = listBox1.SelectedItem.ToString();
                MessageBox.Show(selectedPath, "Selected Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                string selectedPaths = "Selected Paths:\n";
                foreach (var item in checkedListBox1.CheckedItems)
                {
                    selectedPaths += item.ToString() + "\n";
                }
                MessageBox.Show(selectedPaths, "Selected Paths", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
