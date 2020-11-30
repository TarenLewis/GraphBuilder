using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GraphBuilder.Graphing
{

    public interface VAxisIF : GraphComponentIF
    {

    }

    public interface HAxisIF : GraphComponentIF
    {

    }



    public class VAxis : VAxisIF
    {
        private List<VAxisIF> components = new List<VAxisIF>();

        // Controls incrementing for axes 
        public static int incr = 25;

        public void addComponent(VAxisIF vaif)
        {
            components.Add(vaif);
        }

        public void removeComponent(VAxisIF vaif)
        {
            components.Remove(vaif);
        }

        public void draw(Panel p)
        {
            int x1 = (int)(p.Width * 0.10);
            int y1 = (int)(p.Height - 0.10 * p.Height);
            int x2 = x1;
            int y2 = (int)(p.Height * 0.10);

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = p.CreateGraphics();
            g.DrawLine(pen, x1, y1, x2, y2);

            foreach (VAxisIF vaif in components)
                vaif.draw(p);
        }


    }

    public class HAxis : HAxisIF
    {
        private List<HAxisIF> components = new List<HAxisIF>();

        // Controls incrementing for axes 
        public static int incr = 25;

        public void addComponent(HAxisIF haif)
        {
            components.Add(haif);
        }

        public void removeComponent(HAxisIF haif)
        {
            components.Remove(haif);
        }

        public void draw(Panel p)
        {
            int x1 = (int)(p.Width * 0.10);
            int y1 = (int)(p.Height - 0.10 * p.Height);
            int x2 = (int)(p.Width - p.Width * 0.10);
            int y2 = y1;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = p.CreateGraphics();
            g.DrawLine(pen, x1, y1, x2, y2);

            foreach (HAxisIF haif in components)
                haif.draw(p);
        }
    }

    public class VAxisGridLines : VAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;

        public void draw(Panel p)
        {
            int x1 = (int)(p.Width * 0.10);
            int x2 = (int)(p.Width - p.Width * 0.10);

            int y1 = (int)(p.Height * 0.10);
            int y2 = (int)(p.Height - p.Height * 0.10);
            int dy = (int)((y2 - y1) / VAxis.incr);

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            for (int i = 1; i < VAxis.incr + 1; i++)
            {
                int newy = y2 - dy * i;
                g.DrawLine(pen, x1, newy, x2, newy);
            }
        }
    }

    public class HAxisGridLines : HAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;

        public void draw(Panel p)
        {
            int x1 = (int)(p.Width * 0.10);
            int x2 = (int)(p.Width - p.Width * 0.10);
            int dx = (int)((x2 - x1) / HAxis.incr);

            int y1 = (int)(p.Height - p.Height * 0.10);
            int y2 = (int)(p.Height * 0.10);

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            for (int i = 1; i < HAxis.incr + 1; i++)
            {
                int newx = x1 + dx * i;
                g.DrawLine(pen, newx, y1, newx, y2);
            }
        }
    }


    
   public class VAxisTickMarks : VAxisIF
    {
        private int y_max, y_min;
        private Color c = Color.Black;
        private float thickness = 1;

        public void draw(Panel p)
        {
            this.y_max = Data.y_max;
            this.y_min = Data.y_min;


            int x1 = (int)(p.Width * 0.09);
            int x2 = (int)(p.Width - p.Width * 0.10);

            int y1 = (int)(p.Height * 0.10);
            int y2 = (int)(p.Height - p.Height * 0.10);
            int dy = (int)((y2 - y1) / VAxis.incr);

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            int y_incr = (y_max - y_min) / VAxis.incr;
            string str;
            Font f = new Font("Times New Roman", 9);

            for (int i = 1; i < VAxis.incr + 1; i++)
            {
                int newy = y2 - dy * i;
                g.DrawLine(pen, x1, newy, x2, newy);

                str = (y_min + y_incr * i).ToString();
                g.DrawString(str, f, Brushes.Black, x1 - 25, newy - 10);

            }
        }

    }


    public class HAxisTickMarks : HAxisIF
    {
        private int  x_max, x_min;
        private Color c = Color.Black;
        private float thickness = 1;

        public void draw(Panel p)
        {
            this.x_max = Data.x_max;
            this.x_min = Data.x_min;


            int x1 = (int)(p.Width * 0.10);
            int x2 = (int)(p.Width - p.Width * 0.10);
            int dx = (int)((x2 - x1) / HAxis.incr);

            int y1 = (int)(p.Height - p.Height * 0.09);
            int y2 = (int)(p.Height * 0.10);

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            int x_incr = (x_max - x_min) / HAxis.incr;
            string str;
            Font f = new Font("Times New Roman", 9);

            for (int i = 1; i < HAxis.incr + 1; i++)
            {
                g = p.CreateGraphics();

                int newx = x1 + dx * i;
                g.DrawLine(pen, newx, y1, newx, y2);

                str = (x_min + x_incr * i).ToString();

                g.TranslateTransform(newx + 5, y1);
                g.RotateTransform(45F);
                g.DrawString(str, f, Brushes.Black, 0, 0);
            }

            
        }


    }
    
}
