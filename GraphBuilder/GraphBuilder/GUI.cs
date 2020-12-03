using System;
using System.IO;
using System.Windows.Forms;
using GraphBuilder.Manager;
using GraphBuilder.Graphing;
using System.Drawing;

namespace GraphBuilder
{
    public partial class GUI : Form
    {

        GraphManager graphmanager = new GraphManager();

        public GUI()
        {
            InitializeComponent();
        }

        // OK button 
        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Graph g in graphmanager.graphList)
            {
                graphmanager.graph.draw(display);
            }
        }


        // Open Graph
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

        // Load Dataset
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
            directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder"));
            directory += "\\SavedGraphs";
            saveFileDialog.InitialDirectory = directory;

            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.Filter = "XML Files|*.xml";

            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;

                // Set this graph's file name.
                graphmanager.graph.setFileName(saveFileDialog.FileName);

                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    if(graphmanager.graph != null)
                    {
                        graphmanager.saveGraphObject(graphmanager.graph, path);
                    }
                    myStream.Close();
                }
            }


        }

        // Clear Graph
        private void clearGraphButton_click(object sender, EventArgs e)
        {
            Graphics g = display.CreateGraphics();
            g.FillRectangle(Brushes.White, 0, 0, display.Width, display.Height);
        }

        // On Graph type 'selected index change'
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            foreach (Graph graph in graphmanager.graphList)
            {
                foreach(GraphComponentIF graphComponentIF in graph.getComponentList())
                {
                    // If graphComponentIF.type != selected combobox, remove it. Then add a graphComponentIF of type "selected combobox"

                    if (comboBox.SelectedItem.ToString() != graphComponentIF.getComponentType())
                    {
                        // remove the component
                        graph.removeComponent(graphComponentIF);
                    }

                    // GraphComponentIF gcif = new graphComponent("typeHere");
                    // Add correct component type
                    // graph.addComponent(gcif)
                    
                }
            }
        }

        // X-axis 'selected index change'
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Y-axis 'selected index change'
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Ticks 'selected index change'
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Grid Lines 'selected index change'
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
