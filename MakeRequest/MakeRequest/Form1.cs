using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace MakeRequest
{
    public partial class Form1 : Form
    {
        private Queue<string> fileQueue = new Queue<string>();
        public string Folder;
        public string newtext1;
        public string newtext2;
        public string sourceFilePath;
        private int fileCounter = 1;
        private int fileCounter2 = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Construct the file name with the counter
            string fileName = $"request_{fileCounter.ToString("D2")}.txt";

            StreamWriter A = new StreamWriter(Path.Combine(Application.StartupPath, "request", fileName));
            A.WriteLine(label1.Text + " " + textBox1.Text);
            A.WriteLine(label2.Text + " " + textBox2.Text);
            A.WriteLine(label3.Text);
            A.Close();

            // Increment the counter for the next file
            fileCounter++;
            MessageBox.Show("บันทึกเรียบร้อย");
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                // Construct the file name with the counter
                string fileName = $"request_{fileCounter.ToString("D2")}.txt";

                StreamWriter A = new StreamWriter(Path.Combine(Application.StartupPath, "request", fileName));
                A.WriteLine(label1.Text + " " + textBox1.Text);
                A.WriteLine(label2.Text + " " + textBox2.Text);
                A.WriteLine(label3.Text);
                A.Close();

                // Increment the counter for the next file
                fileCounter++;
                MessageBox.Show("บันทึกเรียบร้อย");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }

        private string SanitizeFileName(string fileName)
        {
            // Remove any illegal characters from the file name
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return new string(fileName.Where(c => !invalidChars.Contains(c)).ToArray());
        }
        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                Folder = folderBrowserDialog1.SelectedPath;
                textBox9.Text = folderBrowserDialog1.SelectedPath;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string folderPath = Folder;
            
            // Check if the folder exists
            if (Directory.Exists(folderPath))
            {
                // Get all files in the folder
                string[] files = Directory.GetFiles(folderPath);

                // Add files to the queue
                foreach (string file in files)
                {
                    fileQueue.Enqueue(file);

                    if (File.Exists(file))
                    {
                        string textContent = File.ReadAllText(file);
                        textBox4.Text = textContent;

                        string[] line = File.ReadAllLines(file);
                        foreach (string l in line)
                        {
                            if (l.Contains("Tray"))
                            {
                                newtext1 = l.Replace("Tray : ", "");
                                textBox5.Text = newtext1;
                            }
                            if (l.Contains("Pos"))
                            {
                                newtext2 = l.Replace("Pos  : ", "");
                                textBox6.Text = newtext2;
                            }
                            if (l.Contains("Return"))
                            {
                                textBox7.Text = l;
                            }

                        }
                    }

                    if (!string.IsNullOrEmpty(newtext1) && !string.IsNullOrEmpty(newtext2))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(@"D:\", "HolyOCR")); // Replace with your actual directory path

                        if (directoryInfo.Exists)
                        {
                            // Search for files containing both newtext1 and newtext2
                            FileInfo[] fils = directoryInfo.GetFiles($"*{newtext1}*{newtext2}*");

                            if (fils.Length > 0)
                            {
                                StringBuilder fileNames = new StringBuilder();

                                foreach (FileInfo fileInfo in fils)
                                {
                                    fileNames.AppendLine(fileInfo.FullName);
                                }
                                sourceFilePath = fileNames.ToString();
                                textBox10.Text = fileNames.ToString();
                            }
                            else
                            {
                                textBox10.Text = $"No files found in {directoryInfo.FullName} containing both {newtext1} and {newtext2}.";
                            }
                        }
                        else
                        {
                            textBox10.Text = "Directory does not exist.";
                        }
                    }
                    else
                    {
                        textBox10.Text = "Specify both search terms first.";
                    }
                    string fileName = $"nissan_{fileCounter2.ToString("D2")}.txt";

                    StreamWriter A = new StreamWriter(Path.Combine(Application.StartupPath, "nissan", fileName));
                    A.WriteLine(label1.Text + " " + textBox5.Text);
                    A.WriteLine(label2.Text + " " + textBox6.Text);
                    A.WriteLine(label3.Text);
                    A.Close();

                    // Increment the counter for the next file
                    fileCounter2++;
                    string filedel = file;
                    File.Delete(filedel);
                    try 
                    {
                        string destinationFolderPath = @"C:\Users\Fronk1221\source\repos\MakeRequest\MakeRequest\bin\Debug\targetPic\" + textBox5.Text +"_"+ textBox6.Text + ".jpg";
                        File.Copy(sourceFilePath, destinationFolderPath);
                    }
                    catch 
                    {
                        return;
                    }

                    DisplayNextFile();
                }

                // Display the files in the TextBox
            }
            else
            {
                MessageBox.Show("The specified folder does not exist.");
                timer1.Stop();
            }
        }

        private void DisplayNextFile()
        {
            if (fileQueue.Count > 0)
            {
                // Dequeue the next file from the queue
                string nextFile = fileQueue.Dequeue();

                // Display the file in the TextBox
                textBox1.AppendText(nextFile + Environment.NewLine);

                // Optionally, you can call this method recursively to display all files
                // DisplayNextFile();
            }
            else
            {
                MessageBox.Show("No more files in the queue.");
            }
        }
        private void folderBrowserDialog1_HelpRequest_1(object sender, EventArgs e)
        {

        }
    }
}
