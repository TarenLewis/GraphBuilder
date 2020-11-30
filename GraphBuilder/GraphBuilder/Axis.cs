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
            double x1 = p.Width * 0.10;
            double y1 = p.Height - 0.10 * p.Height;
            double x2 = x1;
            double y2 = p.Height * 0.10;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = p.CreateGraphics();
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);

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
            double x1 = p.Width * 0.10;
            double y1 = p.Height - 0.10 * p.Height;
            double x2 = p.Width - p.Width * 0.10;
            double y2 = y1;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = p.CreateGraphics();
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);

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
            double x1 = p.Width * 0.10;
            double x2 = p.Width - p.Width * 0.10;

            double y1 = p.Height * 0.10;
            double y2 = p.Height - p.Height * 0.10;
            double dy = (y2 - y1) / VAxis.incr;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            for (int i = 1; i < VAxis.incr + 1; i++)
            {
                double newy = y2 - dy * i;
                g.DrawLine(pen, (float) x1, (float) newy, (float) x2, (float) newy);
            }
        }
    }

    public class HAxisGridLines : HAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;

        public void draw(Panel p)
        {
            double x1 = p.Width * 0.10;
            double x2 = p.Width - p.Width * 0.10;
            double dx = (x2 - x1) / HAxis.incr;

            double y1 = p.Height - p.Height * 0.10;
            double y2 = p.Height * 0.10;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            for (int i = 1; i < HAxis.incr + 1; i++)
            {
                double newx = x1 + dx * i;
                g.DrawLine(pen, (float) newx, (float) y1, (float) newx, (float) y2);
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


            double x1 = p.Width * 0.09;
            double x2 = p.Width * 0.10;

            double y1 = p.Height * 0.10;
            double y2 = p.Height - p.Height * 0.10;
            double dy = (y2 - y1) / VAxis.incr;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            double y_incr = (y_max - y_min) / VAxis.incr;
            string str;
            Font f = new Font("Times New Roman", 9);

            for (int i = 1; i < VAxis.incr + 1; i++)
            {
                double newy = y2 - dy * i;
                g.DrawLine(pen, (float) x1, (float) newy, (float) x2, (float) newy);

                str = (y_min + y_incr * i).ToString();
                g.DrawString(str, f, Brushes.Black, (float) x1 - 25, (float) newy - 10);

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


            double x1 = p.Width * 0.10;
            double x2 = p.Width - p.Width * 0.10;
            double dx = (x2 - x1) / HAxis.incr;

            double y1 = p.Height - p.Height * 0.09;
            double y2 = p.Height - p.Height * 0.10;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            double x_incr = (x_max - x_min) / HAxis.incr;
            string str;
            Font f = new Font("Times New Roman", 9);

            for (int i = 1; i < HAxis.incr + 1; i++)
            {
                g = p.CreateGraphics();

                double newx = x1 + dx * i;
                g.DrawLine(pen, (float) newx, (float) y1, (float) newx, (float) y2);

                str = (x_min + x_incr * i).ToString();

                g.TranslateTransform((float) newx + 5, (float) y1);
                g.RotateTransform(45F);
                g.DrawString(str, f, Brushes.Black, 0, 0);
            }

            
        }


    }
    
}
