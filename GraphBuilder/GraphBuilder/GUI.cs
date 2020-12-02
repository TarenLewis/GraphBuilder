using System;
using System.IO;
using System.Windows.Forms;
using GraphBuilder.Manager;
using GraphBuilder.Graphing;

namespace GraphBuilder
{
    public partial class GUI : Form
    {

        

        GraphManager graphmanager = new GraphManager();

        public GUI()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            graphmanager.graph.draw(display);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog od = new OpenFileDialog())
            {
                string directory = Directory.GetCurrentDirectory();
                directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder\\GraphBuilder"));
                directory += "\\SavedGraphs";
                od.InitialDirectory = directory;

                od.DefaultExt = "xml";
                od.Filter = "XML Files|*.xml";

                if (od.ShowDialog() == DialogResult.OK)
                {
                    string path = od.FileName;
                    graphmanager.graph = graphmanager.openGraphObject<Graph>(path);
                }


            }
        }

        private void loadDatasetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog od = new OpenFileDialog())
            {
                string directory = Directory.GetCurrentDirectory();
                directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder\\GraphBuilder\\bin\\Debug"));
                directory += "\\Data";
                od.InitialDirectory = directory;

                od.Filter = "CSV files (*.csv)|*.csv";

                if (od.ShowDialog() == DialogResult.OK)
                {
                    string path = od.FileName;
                    graphmanager.openData(path);
                }


            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save current graph
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save As
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string directory = Directory.GetCurrentDirectory();
            directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder\\GraphBuilder\\bin\\Debug"));
            directory += "\\Data";
            saveFileDialog.InitialDirectory = directory;

            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    graphmanager.saveGraphObject(graphmanager.graph, path);
                    myStream.Close();
                }
            }


        }

        // Clear Graph
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
