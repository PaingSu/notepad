using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;

namespace notepad
{
    public partial class Notepad : Form
    {
        public string Path = "";
        String currentFile;

        public Notepad()
        {
            InitializeComponent();
            this.Text = "Notpad";
            currentFile = null;

            this.richTextBox1.AllowDrop = true;
            this.richTextBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.richTextBox1_DragDrop);
        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileNames != null)
            {
                richTextBox1.Clear();
                foreach (string name in fileNames)
                {
                    try
                    {
                        this.richTextBox1.AppendText(File.ReadAllText(name));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void openCtrlOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void openFile()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog1.FileName;
                System.IO.StreamReader sr = new System.IO.StreamReader(currentFile);

                richTextBox1.Text = sr.ReadToEnd();
                this.Text = currentFile + " Notepad";
                sr.Close();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void newFile()
        {
            DialogResult result = MessageBox.Show("Do you want to save changes to the current file?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (currentFile != null)
                    saveFile();
                else
                    saveFileAs();
            }
            else if (result == DialogResult.Cancel)
            {
                //code for Cancel
                return;
            }
            richTextBox1.Text = "";
            currentFile = null;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveFile()
        {
            SaveFileDialog SFD = new SaveFileDialog();
            if (Path != "")
            {
                richTextBox1.SaveFile(Path);
            }

            else
            {
                SFD.Filter = "Text files (*.Txt)| *.txt";
                SFD.FileName = "";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.SaveFile(SFD.FileName, RichTextBoxStreamType.UnicodePlainText);
                    Path = SFD.FileName;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileAs();
        }

        private void saveFileAs()
        {
            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "Text files (*.Txt)| *.txt";
            SFD.FileName = "";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(SFD.FileName, RichTextBoxStreamType.UnicodePlainText);
                Path = SFD.FileName;
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                Application.Exit();
            }
            DialogResult result = MessageBox.Show("Do you really want to exit?\n All the current changes will be lost", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            else if (result == DialogResult.No)
            {
                //return;
                Application.Exit();
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printFile();
        }

        private void printFile()
        {
            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = currentFile;
            printDlg.Document = printDoc;
            printDlg.AllowSelection = true;
            printDlg.AllowSomePages = true;
            //Call ShowDialog
            if (printDlg.ShowDialog() == DialogResult.OK)
                printDoc.Print();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }


        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " " + DateTime.Now;
        }

        private void wordWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWToolStripMenuItem.Checked == true)
            {
                richTextBox1.WordWrap = true;
            }
            else
                richTextBox1.WordWrap = false;

        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog FD = new FontDialog();
            FD.Font = richTextBox1.SelectionFont;
            if (FD.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = FD.Font;
            }
        }

        private void pageColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = CD.Color;
            }
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = CD.Color;
            }
        }

        private void selectedFontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog CD = new ColorDialog();
            if (CD.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = CD.Color;
            }
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length > 0)
            {
                cutToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
            }
        }

        private void aboutNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.Show();
        }



    }
}
