
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GraphBuilder.Graphing
{
    
    public interface DataIF : GraphComponentIF
    {

    }


    public class Data : DataIF
    {
        private List<DataIF> components = new List<DataIF>();

        public static int x_min, x_max, y_min, y_max;

        public static void setLimits(int x_min, int x_max, int y_min, int y_max)
        {
            Data.x_min = x_min;
            Data.x_max = x_max;
            Data.y_min = y_min;
            Data.y_max = y_max;
        }


        public void draw(Panel p)
        {
            foreach (DataIF dif in components)
                dif.draw(p);
        }

        public void addComponent(DataIF dif)
        {
            components.Add(dif);
        }

        public void removeComponent(DataIF dif)
        {
            components.Remove(dif);
        }
    }


    public class Point : DataIF
    {
        private double radius, x, y;
        private Color c = Color.Blue;

        public Point(double x, double y, double radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;

        }

        public double getX()
        {
            return this.x;
        }

        public double getY()
        {
            return this.y;
        }

        public void draw(Panel p)
        {
            double location_x_min = p.Width * 0.1;
            double location_x_max = p.Width - p.Width * 0.1;
            double location_y_min = p.Height - p.Height * 0.1;
            double location_y_max = p.Height * 0.1;


            double location_x = (x / (Data.x_max - Data.x_min)) * (location_x_max - location_x_min) + location_x_min;
            double location_y = (y / (Data.y_max - Data.y_min)) * (location_y_max - location_y_min) + location_y_min;

            Graphics g = p.CreateGraphics();
            Pen pen = new Pen(c);
            Brush brush = new SolidBrush(c);
            g.DrawEllipse(pen, (float) (location_x - radius), (float) (location_y -  radius), (float) (2*radius), (float) (2*radius));
            g.FillEllipse(brush, (float) (location_x - radius), (float) (location_y - radius), (float) (2*radius), (float) (2*radius));

        }
    }


    public class Line : DataIF
    {
        List<Point> points = new List<Point>();
        Color c = Color.Red;

        public void addPoint(Point p)
        {
            points.Add(p);
        }

        public void draw(Panel p)
        {
            double location_x_min = p.Width * 0.1;
            double location_x_max = p.Width - p.Width * 0.1;
            double location_y_min = p.Height - p.Height * 0.1;
            double location_y_max = p.Height * 0.1;


            double x1, y1, x2, y2;
            Graphics g = p.CreateGraphics();
            Pen pen = new Pen(c, 0.75F);

            x1 = (points[0].getX() / (Data.x_max - Data.x_min)) * (location_x_max - location_x_min) + location_x_min;
            y1 = (points[0].getY() / (Data.y_max - Data.y_min)) * (location_y_max - location_y_min) + location_y_min;

            for (int i = 1; i < points.Count; i++)
            {
                x2 = (points[i].getX() / (Data.x_max - Data.x_min)) * (location_x_max - location_x_min) + location_x_min;
                y2 = (points[i].getY() / (Data.y_max - Data.y_min)) * (location_y_max - location_y_min) + location_y_min;

                g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);

                x1 = x2;
                y1 = y2;
            }
            
        }
    }
    
}
