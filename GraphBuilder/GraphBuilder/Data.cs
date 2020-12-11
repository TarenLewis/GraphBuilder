using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphBuilder.Manager;
using GraphBuilder.Observer;

namespace GraphBuilder.Graphing
{
    
    // Marker interface to control data composite object
    public interface DataIF : GraphComponentIF
    {
        
    }


    // Data composite class 
    [Serializable()]
    public class Data : DataIF
    {
        private List<DataIF> components = new List<DataIF>();


        // Draw all subcomponents
        public void draw(Bitmap bmp)
        {
            foreach (DataIF dif in components)
                dif.draw(bmp);
        }

        // Add subcomponent
        public void addComponent(DataIF dif)
        {
            components.Add(dif);
        }

        // remove subcomponent
        public void removeComponent(DataIF dif)
        {
            components.Remove(dif);
        }

        // remove all subcompoents
        public void clear()
        {
            components.Clear();
        }

        public object Clone()
        {
            // Memberwise clone value types
            Data tempData = (Data)this.MemberwiseClone();

            // Create new temporary list reference.
            List<DataIF> tempDataList = new List<DataIF>();

            // Clone each reference of dif components
            foreach(DataIF dif in this.components)
            {
                tempDataList.Add((DataIF)dif.Clone());
            }

            // Copy reference of temp list.
            tempData.components = tempDataList;

            return tempData;
        }
    }

    // Class to represent a point on the graph
    [Serializable()]
    public class Point : DataIF, ObserverIF
    {
        // x and y data point, not GUI location
        private double radius, x, y;
        private Color c = Color.Blue;

        // Constructor
        public Point(double x, double y, double radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;

        }

        // Setter for color
        public void setColor(Color c)
        {
            this.c = c;
        }

        // Getter for x data point
        public double getX()
        {
            return this.x;
        }

        // getter for y data point
        public double getY()
        {
            return this.y;
        }

        // Getter for radius 
        public double getR()
        {
            return this.radius;
        }

        public void draw(Bitmap bmp)
        {
            // Locations on the GUI
            double location_x_min = bmp.Width * GraphManager.W_PADDING;
            double location_x_max = bmp.Width * GraphManager.E_PADDING;
            double location_y_min = bmp.Height * GraphManager.S_PADDING;
            double location_y_max = bmp.Height * GraphManager.N_PADDING;

            // Calculate GUI location using ratios
            double location_x = (x / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min;
            double location_y = (y / (GraphManager.Y_MAX - GraphManager.Y_MIN)) * (location_y_max - location_y_min) + location_y_min;

            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(c);
            Brush brush = new SolidBrush(c);

            g.DrawEllipse(pen, (float) (location_x - radius), (float) (location_y -  radius), (float) (2*radius), (float) (2*radius));
            g.FillEllipse(brush, (float) (location_x - radius), (float) (location_y - radius), (float) (2*radius), (float) (2*radius));

            g.Dispose();
            brush.Dispose();
            pen.Dispose();
        }

        public object Clone()
        {
            // Shallow copy since Color is value type (struct), as well as the ints.
            Point temporaryPoint = (Point)this.MemberwiseClone();

            return temporaryPoint;
        }

        public void notify(Panel p)
        {
            double y1 = p.Height * GraphManager.S_PADDING;
            double y2 = p.Height * GraphManager.N_PADDING;

            double location_x_min = p.Width * GraphManager.W_PADDING;
            double location_x_max = p.Width * GraphManager.E_PADDING;

            double x1 = (x / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min;
            double x2 = x1;

            Graphics g = p.CreateGraphics();
            Pen pen = new Pen(Brushes.Red, 0.75F);
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);

            g.Dispose();
            pen.Dispose();
        }
    }

    [Serializable()]
    public abstract class AbstractPointWrapper : DataIF
    {
        internal Point point;

        public object Clone()
        {
            // Shallow copy since Color is value type (struct), as well as the ints.
            AbstractPointWrapper temporaryPoint = (AbstractPointWrapper)this.MemberwiseClone();

            return temporaryPoint;
        }


        public abstract void draw(Bitmap bmp);
    }

    [Serializable()]
    public class PointWithCoordinates : AbstractPointWrapper
    {
        public PointWithCoordinates(Point p)
        {
            point = new Point(p.getX(), p.getY(), p.getR());
        }

        public override void draw(Bitmap bmp)
        {
            string x_str = this.point.getX().ToString("#.##");
            string y_str = this.point.getY().ToString("#.##");

            string coord = "(" + x_str + ", " + y_str + ")";

            this.point.draw(bmp);

            // Locations on the GUI
            double location_x_min = bmp.Width * GraphManager.W_PADDING;
            double location_x_max = bmp.Width * GraphManager.E_PADDING;
            double location_y_min = bmp.Height * GraphManager.S_PADDING;
            double location_y_max = bmp.Height * GraphManager.N_PADDING;

            // Calculate GUI location using ratios
            double location_x = (point.getX() / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min;
            double location_y = (point.getY() / (GraphManager.Y_MAX - GraphManager.Y_MIN)) * (location_y_max - location_y_min) + location_y_min;

            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Font f = new Font("Times New Roman", 9);
            g.DrawString(coord, f, Brushes.Black, (float) location_x, (float) location_y);

            g.Dispose();
            f.Dispose();
        }
    }


    // Class to add lines to the graph 
    [Serializable()]
    public class Line : DataIF
    {
        List<Point> points = new List<Point>();
        Color c = Color.Red;

        // Add point: all plotted points should be in the list
        public void addPoint(Point p)
        {
            points.Add(p);
        }

        // Setter for color 
        public void setColor(Color c)
        {
            this.c = c;
        }

        // Clear all points
        public void clear()
        {
            points.Clear();
        }


        //Connect a line between all points
        public void draw(Bitmap bmp)
        {
            if (points.Count == 0)
                return;

            // locations on the GUI
            double location_x_min = bmp.Width * GraphManager.W_PADDING;
            double location_x_max = bmp.Width * GraphManager.E_PADDING;
            double location_y_min = bmp.Height * GraphManager.S_PADDING;
            double location_y_max = bmp.Height * GraphManager.N_PADDING;


            double x1, y1, x2, y2;
            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(c, 0.75F);

            x1 = (points[0].getX() / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min;
            y1 = (points[0].getY() / (GraphManager.Y_MAX - GraphManager.Y_MIN)) * (location_y_max - location_y_min) + location_y_min;

            // Loop thorugh connecting line between each point
            for (int i = 1; i < points.Count; i++)
            {
                x2 = (points[i].getX() / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min;
                y2 = (points[i].getY() / (GraphManager.Y_MAX - GraphManager.Y_MIN)) * (location_y_max - location_y_min) + location_y_min;

                g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);

                x1 = x2;
                y1 = y2;
            }

            g.Dispose();
            pen.Dispose();
        }

        public object Clone()
        {
            Line temporaryLine = (Line)this.MemberwiseClone();
            // includes shallow copy for Color c;

            // Create new object reference and set equal to this.points
            List<Point> tempPoints2 = new List<Point>();
            foreach (Point p in this.points)
            {
                tempPoints2.Add((Point)p.Clone());
            }

            // assign temporaryLine.points to newly created object reference.
            temporaryLine.points = tempPoints2;

            // Return deep copy
            return temporaryLine;
        }
    }
    
}
