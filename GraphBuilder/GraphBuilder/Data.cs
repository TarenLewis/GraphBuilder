
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphBuilder.Manager;

namespace GraphBuilder.Graphing
{
    
    // Marker interface to control data composite object
    public interface DataIF : GraphComponentIF
    {

    }

    
    // Data composite class 
    public class Data : DataIF
    {
        private List<DataIF> components = new List<DataIF>();


        // Draw all subcomponents
        public void draw(Panel p)
        {
            foreach (DataIF dif in components)
                dif.draw(p);
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

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }

    // Class to represent a point on the graph
    public class Point : DataIF
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

        public void draw(Panel p)
        {
            // Locations on the GUI
            double location_x_min = p.Width * GraphManager.W_PADDING;
            double location_x_max = p.Width * GraphManager.E_PADDING;
            double location_y_min = p.Height * GraphManager.S_PADDING;
            double location_y_max = p.Height * GraphManager.N_PADDING;

            // Calculate GUI location using ratios
            double location_x = (x / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min;
            double location_y = (y / (GraphManager.Y_MAX - GraphManager.Y_MIN)) * (location_y_max - location_y_min) + location_y_min;

            Graphics g = p.CreateGraphics();
            Pen pen = new Pen(c);
            Brush brush = new SolidBrush(c);

            g.DrawEllipse(pen, (float) (location_x - radius), (float) (location_y -  radius), (float) (2*radius), (float) (2*radius));
            g.FillEllipse(brush, (float) (location_x - radius), (float) (location_y - radius), (float) (2*radius), (float) (2*radius));

        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }

    // Class to add lines to the graph 
    public class Line : DataIF
    {
        private List<Point> points = new List<Point>();
        private Color c = Color.Red;

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


        //Connect a line between all points
        public void draw(Panel p)
        {
            if (points.Count == 0)
                return;

            // locations on the GUI
            double location_x_min = p.Width * GraphManager.W_PADDING;
            double location_x_max = p.Width * GraphManager.E_PADDING;
            double location_y_min = p.Height * GraphManager.S_PADDING;
            double location_y_max = p.Height * GraphManager.N_PADDING;


            double x1, y1, x2, y2;
            Graphics g = p.CreateGraphics();
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
            
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
