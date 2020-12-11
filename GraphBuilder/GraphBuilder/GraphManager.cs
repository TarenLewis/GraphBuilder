
﻿using GraphBuilder.Graphing;
using GraphBuilder.Observer;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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
        public static double S_PADDING = 0.90;
        public static double W_PADDING = 0.10;

        public Graph graph = new Graph();
        

        // Observer Pattern
        private Notifier notifier;

        // Varibables to build composite object
        private XAxis haxis = new XAxis();
        private XAxisGridLines hgridlines = new XAxisGridLines();
        private XAxisTickMarks htickmarks = new XAxisTickMarks();


        private YAxis vaxis = new YAxis();
        private YAxisGridLines vgridlines = new YAxisGridLines();
        private YAxisTickMarks vtickmarks = new YAxisTickMarks();

        private Data data = new Data();
        private Line line = new Line();

        // Default constructor build basic default graph
        public GraphManager(Panel p)
        {

            // Build basic graph as default can edit this code to test
            // Don't need to load data set just hit button to display graph
            Point p1 = new Point(25, 25, 1.5);
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

            notifier = new Notifier(p);
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
            graph.removeComponent(haxis);
        }

        public void addXAxis()
        {
            graph.addComponent(haxis);
        }

        public void addYAxis()
        {
            graph.addComponent(vaxis);
        }

        public void removeYAxis()
        {
            graph.removeComponent(vaxis);
        }

        public void removeGridLines()
        {
            vaxis.removeComponent(vgridlines);
            haxis.removeComponent(hgridlines);
        }

        public void addGridLines()
        {
            vaxis.addComponent(vgridlines);
            haxis.addComponent(hgridlines);
        }

        public void addTickMarks()
        {
            vaxis.addComponent(vtickmarks);
            haxis.addComponent(htickmarks);
        }

        public void removeTickMarks()
        {
            vaxis.removeComponent(vtickmarks);
            haxis.removeComponent(htickmarks);
        }

        public void handleMouseMove(Panel p, int x)
        {
            notifier.notify(p, x);
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

                haxis.setTitle(x_axis_title);
                vaxis.setTitle(y_axis_title);
                graph.setTitle(title);
                
                string[] values;
                double x, y, x_max = 0, y_max = 0;
                Point p;

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
                    
                }

                GraphManager.Y_MAX = y_max;
                GraphManager.X_MAX = x_max;
                


            }

            data.addComponent(line);
        }

        public void saveGraphObject<Graph>(Graph serializableObject, string fileName)
        {
            Console.WriteLine("Inside saveGraphObject");
            if (serializableObject == null) {
                Console.WriteLine("Object is null inside saveGraphObject function");
                return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere was an isue with WRITING the object.\n{0}", ex.Message);
            }
        }

        public Graph openGraphObject<Graph>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(Graph); }

            Graph objectOut = default(Graph);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(Graph);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (Graph)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThere was an isue with READING the object.\n{0}", ex.Message);
            }

            return objectOut;
        }

    }
}
