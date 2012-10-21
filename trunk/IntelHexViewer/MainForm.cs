using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IntelHexViewer2
{   
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileLineLoader.LoadFile("C:\\Blaze.hex", this.hex_grid_left);
        }

        private void loadLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenLeftFile();
        }

        private void loadRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenRightFile();
        }

        private void OpenLeftFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = Application.StartupPath;
            dlg.Filter = "Intel Hex Files (*.hex)|*.hex|" +
                            "All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLeftFile(dlg.FileName);
            }
        }

        private void OpenRightFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.InitialDirectory = Application.StartupPath;
            dlg.Filter = "Intel Hex Files (*.hex)|*.hex|" +
                            "All files (*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadRightFile(dlg.FileName);
            }
        }

        private void LoadLeftFile(string filename)
        {
            this.txtLeftFileName.Text = filename;
            FileLineLoader.LoadFile(filename, this.hex_grid_left);
        }

        private void LoadRightFile(string filename)
        {
            //this.txtRightFileName.Text = filename;
            //FileLineLoader.LoadFile(filename, this.hex_grid_right);
        }

        private void hex_grid_left_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void hex_grid_left_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string file = files[0];

            LoadLeftFile(file);
        }

        private void hex_grid_right_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void hex_grid_right_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string file = files[0];
            LoadRightFile(file);
        }

        private void compareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CompareManager cmp = new CompareManager();
            //cmp.Compare(this.hex_grid_left, this.hex_grid_right);
            //this.hex_grid_left.Refresh();
            //this.hex_grid_right.Refresh();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
