
﻿using GraphBuilder.Graphing;
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


        // Static variables for 
        public static int X_AXIS_INCREMENT = 25;
        public static int Y_AXIS_INCREMENT = 25;

        public static double Y_MAX = 100;
        public static double Y_MIN = 0;
        public static double X_MAX = 100;
        public static double X_MIN = 0;

        public static double N_PADDING = 0.10;
        public static double E_PADDING = 0.95;
        public static double S_PADDING = 0.85;
        public static double W_PADDING = 0.15;

        // Graph variables
        public List<Graph> graphList = new List<Graph>();

        public Graph graph = new Graph();



        private HAxis haxis = new HAxis();
        private HAxisGridLines hgridlines = new HAxisGridLines();
        private HAxisTickMarks htickmarks = new HAxisTickMarks();


        private VAxis vaxis = new VAxis();
        private VAxisGridLines vgridlines = new VAxisGridLines();
        private VAxisTickMarks vtickmarks = new VAxisTickMarks();

        private Data data = new Data();

        public GraphManager()
        {
            vaxis.addComponent(vgridlines);
            vaxis.addComponent(vtickmarks);

            haxis.addComponent(hgridlines);
            haxis.addComponent(htickmarks);

            graph.addComponent(haxis);
            graph.addComponent(vaxis);

            graph.addComponent(data);
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

                    GraphManager.Y_MAX = y_max;
                    GraphManager.X_MAX = x_max;
                }

            }
        }

        public void saveGraphObject<Graph>(Graph serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

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
                //Log exception here
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
                //Log exception here
            }

            return objectOut;
        }

    }
}
