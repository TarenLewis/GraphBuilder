
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
        private int radius, x, y;
        private Color c = Color.Blue;

        public Point(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;

        }

        public void draw(Panel p)
        {
            int location_x_min = (int) (p.Width * 0.1);
            int location_x_max = (int) (p.Width - p.Width * 0.1);
            int location_y_min = (int) (p.Height - p.Height * 0.1);
            int location_y_max = (int) (p.Height * 0.1);


            int location_x = (x / (Data.x_max - Data.x_min)) * (location_x_max - location_x_min);
            int location_y = (y / (Data.y_max - Data.y_min)) * (location_y_max - location_y_min);

            Graphics g = p.CreateGraphics();
            Pen pen = new Pen(c);
            Brush brush = new SolidBrush(c);
            g.DrawEllipse(pen, location_x, location_y, radius, radius);
            g.FillEllipse(brush, location_x, location_y, radius, radius);

        }
    }


    public class Line : DataIF
    {


        public void draw(Panel p)
        {

        }
    }
    
}
