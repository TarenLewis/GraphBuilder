
﻿using GraphBuilder.Graphing;
using GraphBuilder.Observer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace GraphBuilder.Manager
{
    public class GraphManager
    {

        // Static variables for axis spacing
        public static int X_AXIS_INCREMENT = 25;
        public static int Y_AXIS_INCREMENT = 25;

        // Static variables for controlling limits on axis
        public static double Y_MAX = 100;
        public static double Y_MIN = 0;
        public static double X_MAX = 100;
        public static double X_MIN = 0;

        // Controls the spacing between the edge of the panel and the graph
        public static double N_PADDING = 0.10;
        public static double E_PADDING = 0.95;
        public static double S_PADDING = 0.85;
        public static double W_PADDING = 0.15;

        public Graph graph = new Graph();
        public Graph graphCopy = new Graph();

        // Observer Pattern
        private Notifier notifier;

        // Varibables to build composite object
        private XAxis xaxis = new XAxis();
        private XAxisGridLines hgridlines = new XAxisGridLines();
        private XAxisTickMarks htickmarks = new XAxisTickMarks();


        private YAxis yaxis = new YAxis();
        private YAxisGridLines vgridlines = new YAxisGridLines();
        private YAxisTickMarks vtickmarks = new YAxisTickMarks();

        private Data data = new Data();
        private Line line = new Line();

        // Default constructor build basic default graph
        public GraphManager(System.Drawing.Bitmap bmp)
        {

            // Build basic graph as default can edit this code to test
            // Don't need to load data set just hit button to display graph
            Point p1 = new Point(10, 25, 1.5);
            Point p2 = new Point(50, 50, 1.5);
            Point p3 = new Point(75, 75, 1.5);

            p1.setColor(System.Drawing.Color.Red);
            p2.setColor(System.Drawing.Color.Red);
            p3.setColor(System.Drawing.Color.Red);

            PointWithCoordinates wrapper1 = new PointWithCoordinates(p1);
            PointWithCoordinates wrapper2 = new PointWithCoordinates(p2);
            PointWithCoordinates wrapper3 = new PointWithCoordinates(p3);

            data.addComponent(p1);
            data.addComponent(p2);
            data.addComponent(p3);

            line.addPoint(p1);
            line.addPoint(p2);
            line.addPoint(p3);

            data.addComponent(wrapper1);
            data.addComponent(wrapper2);
            data.addComponent(wrapper3);

            notifier = new Notifier(bmp);
            notifier.addObserver(p1);
            notifier.addObserver(p2);
            notifier.addObserver(p3);
        }





        // Handlers to add/remove components from graph
        public void addLine()
        {
            data.addComponent(line);
            graph.addComponent(data);
        }

        public void removeLine()
        {
            data.removeComponent(line);
            graph.addComponent(data);
        }

        public void removeXAxis()
        {
            graph.removeComponent(xaxis);
        }

        public void addXAxis()
        {
            graph.addComponent(xaxis);
        }

        public void addYAxis()
        {
            graph.addComponent(yaxis);
        }

        public void removeYAxis()
        {
            graph.removeComponent(yaxis);
        }

        public void removeGridLines()
        {
            yaxis.removeComponent(vgridlines);
            xaxis.removeComponent(hgridlines);
        }

        public void addGridLines()
        {
            yaxis.addComponent(vgridlines);
            xaxis.addComponent(hgridlines);
        }

        public void addTickMarks()
        {
            yaxis.addComponent(vtickmarks);
            xaxis.addComponent(htickmarks);
        }

        public void removeTickMarks()
        {
            yaxis.removeComponent(vtickmarks);
            xaxis.removeComponent(htickmarks);
        }

        public void handleMouseMove(Panel p, int x)
        {
            notifier.notify(p, x);
        }

        public void resetNotifier()
        {
            notifier.clearObservers();

            foreach(DataIF dif in graph.getData().components)
            {
                if (typeof(Point).IsInstanceOfType(dif))
                {
                    notifier.addObserver((Point)dif);
                }
            }
        }

        public void resetLimits()
        {
            double max_x = 0;
            double max_y = 0;
            foreach(DataIF dif in graph.getData().components)
            {
                if (typeof(Point).IsInstanceOfType(dif))
                {
                    Point pt = (Point)dif;
                    if (pt.getX() > max_x)
                        max_x = pt.getX();
                    if (pt.getY() > max_y)
                        max_y = pt.getY();

                }
            }

            GraphManager.X_MAX = max_x;
            GraphManager.Y_MAX = max_y;
        }


        // Tool menu handlers
        public void openData(string path)
        {
            data.clear();
            line.clear();
            notifier.clearObservers();

            using (StreamReader reader = new StreamReader(path))
            {
                string title = reader.ReadLine();
                title = title.Substring(0, title.Length - 1);
                string[] axis_titles = reader.ReadLine().Split(',');
                string x_axis_title = axis_titles[0];
                string y_axis_title = axis_titles[1];

                xaxis.setTitle(x_axis_title);
                yaxis.setTitle(y_axis_title);
                graph.setTitle(title);
                
                string[] values;
                double x, y, x_max = 0, y_max = 0;
                Point p;

                List<Point> points = new List<Point>();
                while (!reader.EndOfStream)
                {
                    values = reader.ReadLine().Split(',');
                    x = double.Parse(values[0], CultureInfo.InvariantCulture);
                    y = double.Parse(values[1], CultureInfo.InvariantCulture);

                    if (x > x_max)
                        x_max = x;
                    if (y > y_max)
                        y_max = y;

                    p = new Point(x, y, 1.5);
                    
                    data.addComponent(p);
                    line.addPoint(p);
                    points.Add(p);
                    
                }

                GraphManager.Y_MAX = y_max;
                GraphManager.X_MAX = x_max;

                foreach (Point pt in points)
                    notifier.addObserver(pt);
                


            }

            data.addComponent(line);
            graph.addComponent(data);
        }

        public void saveObjectAsBin(Graph graph, string path) {

            Stream SaveFileStream = File.Create(path);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, graph);
            SaveFileStream.Close();
        }

        public Graph openObjectAsBin(string filename)
        {
            Graph graph2 = new Graph();
            if (File.Exists(filename))
            {
                Console.WriteLine("Reading saved file");
                Stream openFileStream = File.OpenRead(filename);
                BinaryFormatter deserializer = new BinaryFormatter();
                graph2 = (Graph)deserializer.Deserialize(openFileStream);
                openFileStream.Close();
               
            }
            
            
            return graph2;
        }


    }
}
