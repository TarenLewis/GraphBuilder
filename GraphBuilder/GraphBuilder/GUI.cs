using System;
using System.IO;
using System.Windows.Forms;
using GraphBuilder.Manager;
using GraphBuilder.Graphing;
using System.Drawing;
using GraphBuilder.Rendering;

namespace GraphBuilder
{
    public partial class GUI : Form
    {

        GraphManager graphmanager;
        Bitmap background_image;
        RenderFuture future;
        GraphRenderRequester requester;

        // Constructor 
        public GUI()
        {
            // Build the GUI components
            InitializeComponent();


            Graphics g = display.CreateGraphics();
            g.FillRectangle(Brushes.White, 0, 0, display.Width, display.Height);

            background_image = new Bitmap(display.Width, display.Height, g);

            graphmanager = new GraphManager(background_image);
            requester = new GraphRenderRequester(background_image);
            g.Dispose();
            
            // Set default selections
            graphTypeComboBox.SelectedIndex = 0;
            xaxisComboBox.SelectedIndex = 0;
            yaxisComboBox.SelectedIndex = 0;
            tickMarkComboBox.SelectedIndex = 0;
            gridLinesComboBox.SelectedIndex = 0;

            // Clone original graph
            graphmanager.graphCopy = (Graph)graphmanager.graph.Clone();

            graphmanager.graph.draw(background_image);
            g = display.CreateGraphics();
            g.DrawImage(background_image, 0, 0);
            g.Dispose();
        }

        // OK button 
        private void okbutton_Click(object sender, EventArgs e)
        {
            Graphics g = display.CreateGraphics();
            g.FillRectangle(Brushes.White, 0, 0, display.Width, display.Height);

            if(!future.checkGraphStatus())
                future.waitForResult();

            background_image = future.getResult();
            g.DrawImage(background_image, 0, 0);
        }

        // Open Graph Object as bin
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog od = new OpenFileDialog())
            {
                string directory = Directory.GetCurrentDirectory();
                directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder\\GraphBuilder"));
                directory += "\\SavedGraphs";
                od.InitialDirectory = directory;

                od.DefaultExt = "bin";
                od.Filter = "BIN Files (*.bin)|*.bin";

                if (od.ShowDialog() == DialogResult.OK)
                {
                    clearGraph();
                    string path = od.FileName;
                    Graph tempGraph = graphmanager.openObjectAsBin(path);

                    graphmanager.graph = (Graph)tempGraph.Clone();
                    
                    graphmanager.graph.draw(background_image);
                    Graphics g = display.CreateGraphics();
                    g.DrawImage(background_image, 0, 0);
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

            graphmanager.graph.draw(background_image);
            Graphics g = display.CreateGraphics();
            g.DrawImage(background_image, 0, 0);

            future = requester.getFuture(graphmanager.graph);
            g.Dispose();

            // Set default selections
            graphTypeComboBox.SelectedIndex = 0;

        }

        // Saving as bin
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/walkthrough-persisting-an-object-in-visual-studio
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Save As
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            string directory = Directory.GetCurrentDirectory();
            directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder"));
            directory += "\\SavedGraphs";
            saveFileDialog.InitialDirectory = directory;

            saveFileDialog.DefaultExt = "bin";
            saveFileDialog.Filter = "BIN Files (*.bin)|*.bin";

            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;

                Console.WriteLine("Saving graph as bin... " + graphmanager.graph.getTitle());
                graphmanager.saveObjectAsBin(graphmanager.graph, path);
            }
        }

            // Clear Graph
        private void clearGraphButton_click(object sender, EventArgs e)
        {
            clearGraph();
        }

        private void clearGraph()
        {
            Graphics g = display.CreateGraphics();
            g.FillRectangle(Brushes.White, 0, 0, display.Width, display.Height);
            
        }


        // On Graph type 'selected index change'
        private void graphTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (graphTypeComboBox.SelectedIndex == 0)
                graphmanager.addLine();
            else
                graphmanager.removeLine();

            future = requester.getFuture(graphmanager.graph);
        }

        // X-axis 'selected index change'
        private void xaxisComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (xaxisComboBox.SelectedIndex == 0)
                graphmanager.addXAxis();
            else
                graphmanager.removeXAxis();

            future = requester.getFuture(graphmanager.graph);
        }

        // Y-axis 'selected index change'
        private void yaxisComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (yaxisComboBox.SelectedIndex == 0)
                graphmanager.addYAxis();
            else
                graphmanager.removeYAxis();

            future = requester.getFuture(graphmanager.graph);
        }

        // Ticks 'selected index change'
        private void tickMarksComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tickMarkComboBox.SelectedIndex == 0)
                graphmanager.addTickMarks();
            else
                graphmanager.removeTickMarks();

            future = requester.getFuture(graphmanager.graph);
        }

        // Grid Lines 'selected index change'
        private void gridLinesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gridLinesComboBox.SelectedIndex == 0)
                graphmanager.addGridLines();
            else
                graphmanager.removeGridLines();

            future = requester.getFuture(graphmanager.graph);
        }

        private void display_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = display.CreateGraphics();
            
            g.DrawImage(background_image, 0, 0);
            graphmanager.handleMouseMove(display, e.X);
        }


        // New Tool menu strip
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

            GraphManager.X_MAX = 100;
            GraphManager.X_MIN = 0;
            GraphManager.Y_MAX = 100;
            GraphManager.Y_MIN = 0;

            /*
            background_image = new Bitmap(display.Width, display.Height);
            graphmanager = new GraphManager(background_image);
            requester = new GraphRenderRequester(background_image
            */

            // Reset default selections
            graphTypeComboBox.SelectedIndex = 0;
            xaxisComboBox.SelectedIndex = 0;
            yaxisComboBox.SelectedIndex = 0;
            tickMarkComboBox.SelectedIndex = 0;
            gridLinesComboBox.SelectedIndex = 0;

            // Stuff is unnecessary since the graph is cloned
            /*
            graphmanager.addLine();
            graphmanager.addGridLines();
            graphmanager.addTickMarks();
            graphmanager.addXAxis();
            graphmanager.addYAxis();
            */ 

            graphmanager.graphCopy.draw(background_image);
            Graphics g = display.CreateGraphics();
            g.DrawImage(background_image, 0, 0);
            g.Dispose();

            future = requester.getFuture(graphmanager.graphCopy);
        }

    }
}
