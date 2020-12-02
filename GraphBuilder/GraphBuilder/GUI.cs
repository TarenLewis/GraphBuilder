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
                directory = directory.Substring(0, directory.LastIndexOf("\\GraphBuilder\\GraphBuilder\\bin\\Debug"));
                directory += "\\Data";
                od.InitialDirectory = directory;

                od.Filter = "CSV files (*.csv)|*.csv";

                if(od.ShowDialog() == DialogResult.OK)
                {
                    string path = od.FileName;
                    graphmanager.openData(path);
                }

                
            }

        }
    }
}
