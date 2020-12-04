using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphBuilder.Manager;

namespace GraphBuilder.Graphing
{

    // Marker interface to control the composition of Y - Axis
    public interface VAxisIF : GraphComponentIF
    {

    }

    // Marker interface to control the composition of X - Axis
    public interface HAxisIF : GraphComponentIF
    {

    }


    // Controls the Line and title for Y - Axis
    public class VAxis : VAxisIF
    {
        private List<VAxisIF> components = new List<VAxisIF>();
        private string title = "Y - Axis";
        public bool title_on = true;

        // Controls incrementing for axes 
        public static int incr = 25;


        // Add objects to the composition 
        public void addComponent(VAxisIF vaif)
        {
            components.Add(vaif);
        }


        // Remove objects from the composition 
        public void removeComponent(VAxisIF vaif)
        {
            components.Remove(vaif);
        }

        // set axis title 
        public void setTitle(string title)
        {
            this.title = title;
        }

        // Draw the horizontal line, title, and all sub component objects
        public void draw(Panel p)
        {
            // Draw line from bottom point (x1, y1) to top point (x2, y2)
            double x1 = p.Width * GraphManager.W_PADDING;
            double y1 = p.Height * GraphManager.S_PADDING;
            double x2 = x1;
            double y2 = p.Height * GraphManager.N_PADDING;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = p.CreateGraphics();
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);


            // Draw all subcomponents 
            foreach (VAxisIF vaif in components)
                vaif.draw(p);

            // Draw Title
            if (title_on)
            {
                Font f = new Font("Times New Roman", 14);
                g.TranslateTransform(5, p.Height / 2);
                g.RotateTransform(270F);

                g.DrawString(title, f, Brushes.Black, 0, 0);

            }
        }

        public string getComponentType()
        {
            return componentType;
        }
    }

    // Controls line and title for X - Axis 
    public class HAxis : HAxisIF
    {
        private List<HAxisIF> components = new List<HAxisIF>();
        private string title = "X - Axis";
        public bool title_on = true;

        // Controls incrementing for axes 
        public static int incr = 25;

        // Add subcomponent 
        public void addComponent(HAxisIF haif)
        {
            components.Add(haif);
        }


        // Remove subcomponent 
        public void removeComponent(HAxisIF haif)
        {
            components.Remove(haif);
        }

        // Set axis title 
        public void setTitle(string title)
        {
            this.title = title;
        }

        // Draw horizontal line, title, and all subcomponents
        public void draw(Panel p)
        {
            // Draw line from left point (x1, y1) to right (x2, y2)
            double x1 = p.Width * GraphManager.W_PADDING;
            double y1 = p.Height * GraphManager.S_PADDING;
            double x2 = p.Width * GraphManager.E_PADDING;
            double y2 = y1;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = p.CreateGraphics();
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);

            // Draw Title
            if (title_on)
            {
                Font f = new Font("Times New Roman", 14);
                g.DrawString(title, f, Brushes.Black, p.Width / 2, (float)(p.Height - (p.Height - p.Height * GraphManager.S_PADDING) / 2));
            }

            // Draw all subcomponents 
            foreach (HAxisIF haif in components)
                haif.draw(p);
        }

        public string getComponentType()
        {
            return componentType;
        }
    }

    // Class to control horizontal gridlines for the Y-axis
    public class VAxisGridLines : VAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;


        public void draw(Panel p)
        {
            // Draw horizontal lines from left point (x1, newy) to right point (x2, newy)
            double x1 = p.Width * GraphManager.W_PADDING;
            double x2 = p.Width * GraphManager.E_PADDING;

            double y1 = p.Height * GraphManager.N_PADDING;
            double y2 = p.Height * GraphManager.S_PADDING;
            double dy = (y2 - y1) / GraphManager.Y_AXIS_INCREMENT;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            // Loop through drawing each line for each axis increment
            for (int i = 1; i < GraphManager.Y_AXIS_INCREMENT + 1; i++)
            {
                double newy = y2 - dy * i;
                g.DrawLine(pen, (float) x1, (float) newy, (float) x2, (float) newy);
            }
        }

        public string getComponentType()
        {
            return "VAxisGridLines";
        }
    }

    // Class to control vertical gridlines for the X-axis
    public class HAxisGridLines : HAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;
        private string componentType = "HAxisGridLines";

        public void draw(Panel p)
        {
            // Draw vertical lines from bottom point (newx, y1) to top point (newy, y2)
            double x1 = p.Width * GraphManager.W_PADDING;
            double x2 = p.Width * GraphManager.E_PADDING;
            double dx = (x2 - x1) / GraphManager.X_AXIS_INCREMENT;

            double y1 = p.Height * GraphManager.S_PADDING;
            double y2 = p.Height * GraphManager.N_PADDING;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            // Loop through drawing each line for each axis increment
            for (int i = 1; i < GraphManager.X_AXIS_INCREMENT + 1; i++)
            {
                double newx = x1 + dx * i;
                g.DrawLine(pen, (float) newx, (float) y1, (float) newx, (float) y2);
            }
        }

        public string getComponentType()
        {
            return componentType;
        }
    }


    // Controls major tick marks for the Y-axis
   public class VAxisTickMarks : VAxisIF
    {
        private Color c = Color.Black;
        private float thickness = 1;
        private string componentType = "VAxisTickMarks";

        public void draw(Panel p)
        {
            double y_max = GraphManager.Y_MAX;
            double y_min = GraphManager.Y_MIN;

            double x1 = p.Width * (GraphManager.W_PADDING - 0.01);
            double x2 = p.Width * GraphManager.W_PADDING;

            double y1 = p.Height * GraphManager.N_PADDING;
            double y2 = p.Height * GraphManager.S_PADDING;
            double dy = (y2 - y1) / GraphManager.Y_AXIS_INCREMENT;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            double y_incr = (y_max - y_min) / GraphManager.Y_AXIS_INCREMENT;
            string str;
            Font f = new Font("Times New Roman", 9);

            // Loop through drawing the the tickmark lines and adding data labels
            for (int i = 1; i < GraphManager.Y_AXIS_INCREMENT + 1; i++)
            {
                double newy = y2 - dy * i;
                g.DrawLine(pen, (float) x1, (float) newy, (float) x2, (float) newy);

                str = ((int) (y_min + y_incr * i)).ToString();
                g.DrawString(str, f, Brushes.Black, (float) x1 - 35, (float) newy - 10);

            }
        }

        public string getComponentType()
        {
            return componentType;
        }
    }

    // Controls major tick marks for the X-axis
    public class HAxisTickMarks : HAxisIF
    {
        private Color c = Color.Black;
        private float thickness = 1;
        private string componentType = "HAxisTickMarks";

        public void draw(Panel p)
        {
            double x_max = GraphManager.X_MAX;
            double x_min = GraphManager.X_MIN;


            double x1 = p.Width * GraphManager.W_PADDING;
            double x2 = p.Width * GraphManager.E_PADDING;
            double dx = (x2 - x1) / GraphManager.X_AXIS_INCREMENT;

            double y1 = p.Height * (GraphManager.S_PADDING + 0.01);
            double y2 = p.Height * GraphManager.S_PADDING;

            Pen pen = new Pen(c, thickness);
            Graphics g = p.CreateGraphics();

            double x_incr = (x_max - x_min) / GraphManager.X_AXIS_INCREMENT;
            string str;
            Font f = new Font("Times New Roman", 9);

            // Loop through drawing tick mark lines and adding data labels
            for (int i = 1; i < GraphManager.X_AXIS_INCREMENT + 1; i++)
            {
                g = p.CreateGraphics();

                double newx = x1 + dx * i;
                g.DrawLine(pen, (float) newx, (float) y1, (float) newx, (float) y2);


                str = ((x_min + x_incr * i)).ToString("#.###");

                g.TranslateTransform((float) newx + 5, (float) y1);
                g.RotateTransform(45F);
                g.DrawString(str, f, Brushes.Black, 0, 0);
            }

            
        }

        public string getComponentType()
        {
            return componentType;
        }
    }
    
}
