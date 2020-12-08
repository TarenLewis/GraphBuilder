﻿using System;
using System.IO;
using System.Windows.Forms;
using GraphBuilder.Manager;
using GraphBuilder.Graphing;
using System.Drawing;
using System.Threading.Tasks;

namespace GraphBuilder
{
    public partial class GUI : Form
    {

        GraphManager graphmanager = new GraphManager();

        public GUI()
        {
            InitializeComponent();
            graphTypeComboBox.SelectedIndex = 0;
            xaxisComboBox.SelectedIndex = 0;
            yaxisComboBox.SelectedIndex = 0;
            tickMarkComboBox.SelectedIndex = 0;
            gridLinesComboBox.SelectedIndex = 0;
        }

        // OK button 
        private async void button1_Click(object sender, EventArgs e)
        {
            Graphics g = display.CreateGraphics();
            g.FillRectangle(Brushes.White, 0, 0, display.Width, display.Height);

            // Check to make sure graph future is ready
            while (!graphmanager.checkIfFutureReady())
            {
                Console.WriteLine("Future is not ready...");
                await Task.Delay(25);
            }

            graphmanager.graph.draw(display);
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
                    clearGraph();
                    string path = od.FileName;
                    graphmanager.graph = new Graph();
                    graphmanager.graph = graphmanager.openGraphObject<Graph>(path);
                    
                    graphmanager.graph.draw(display);
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
                graphmanager.saveGraphObject(graphmanager.graph, path);
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
        }

        // X-axis 'selected index change'
        private void xaxisComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (xaxisComboBox.SelectedIndex == 0)
                graphmanager.addXAxis();
            else
                graphmanager.removeXAxis();
        }

        // Y-axis 'selected index change'
        private void yaxisComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (yaxisComboBox.SelectedIndex == 0)
                graphmanager.addYAxis();
            else
                graphmanager.removeYAxis();
        }

        // Ticks 'selected index change'
        private void tickMarksComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tickMarkComboBox.SelectedIndex == 0)
                graphmanager.addTickMarks();
            else
                graphmanager.removeTickMarks();

        }

        // Grid Lines 'selected index change'
        private void gridLinesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gridLinesComboBox.SelectedIndex == 0)
                graphmanager.addGridLines();
            else
                graphmanager.removeGridLines();
        }


    }
}
