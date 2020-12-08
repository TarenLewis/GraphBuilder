
﻿using GraphBuilder.Graphing;
using GraphBuilder.Observer;
using GraphBuilder.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        public static double S_PADDING = 0.85;
        public static double W_PADDING = 0.15;

        public Graph graph = new Graph();
        
        // Used for rendering the graph with Future pattern
        private GraphRenderRequester graphRenderRequester;

        // Observer Pattern
        private Notifier notifier;



        private XAxis haxis = new XAxis();
        private XAxisGridLines hgridlines = new XAxisGridLines();
        private XAxisTickMarks htickmarks = new XAxisTickMarks();


        private YAxis vaxis = new YAxis();
        private YAxisGridLines vgridlines = new YAxisGridLines();
        private YAxisTickMarks vtickmarks = new YAxisTickMarks();

        private Data data = new Data();
        private Line line = new Line();

        public GraphManager()
        {      

        }

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
        

        public void openData(string path)
        {
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
                // MODIFIED TO TEST wrapper
                AbstractPointWrapper pointWrapper;
                // END MODIFICATION

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
                    // MODIFIED FOR TESTING
                    pointWrapper = new PointWithCoordinates(p);

                    //What I want to do:
                    //data.addComponent(pointWrapper);
                    //line.addPoint(pointWrapper);

                    data.addComponent(p);
                    line.addPoint(p);
                    //data.addComponent(p);
                    //line.addPoint(p);

                    // END MODIFICATION

                    GraphManager.Y_MAX = y_max;
                    GraphManager.X_MAX = x_max;
                }

            }
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
