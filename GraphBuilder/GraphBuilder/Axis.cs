using System;
using System.Collections.Generic;
using System.Drawing;
using GraphBuilder.Manager;

namespace GraphBuilder.Graphing
{

    // Marker interface to control the composition of Y - Axis
    public interface YAxisIF : GraphComponentIF
    {

    }

    // Marker interface to control the composition of X - Axis
    public interface XAxisIF : GraphComponentIF
    {

    }


    // Controls the Line and title for Y - Axis
    public class YAxis : YAxisIF
    {
        private List<YAxisIF> components = new List<YAxisIF>();
        private string title = "Y - Axis";
        public bool title_on = true;

        // Controls incrementing for axes 
        public static int incr = 25;


        // Add objects to the composition 
        public void addComponent(YAxisIF vaif)
        {
            components.Add(vaif);
        }


        // Remove objects from the composition 
        public void removeComponent(YAxisIF vaif)
        {
            components.Remove(vaif);
        }

        // set axis title 
        public void setTitle(string title)
        {
            this.title = title;
        }

        // Draw the horizontal line, title, and all sub component objects
        public void draw(Bitmap bmp)
        {
            // Draw line from bottom point (x1, y1) to top point (x2, y2)
            double x1 = bmp.Width * GraphManager.W_PADDING;
            double y1 = bmp.Height * GraphManager.S_PADDING;
            double x2 = x1;
            double y2 = bmp.Height * GraphManager.N_PADDING;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = Graphics.FromImage(bmp);
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Draw all subcomponents 
            foreach (YAxisIF yaif in components)
                yaif.draw(bmp);

            // Draw Title
            if (title_on)
            {
                Font f = new Font("Times New Roman", 14);
                Font f2 = new Font("Times New Roman", 9);
                

                // Determine size of title and center it in top
                SizeF title_dimensions = g.MeasureString(title, f);
                SizeF data_label_dimensions = g.MeasureString(((int)GraphManager.Y_MAX).ToString(), f2);
                double title_x_location = (bmp.Width * GraphManager.W_PADDING - data_label_dimensions.Width - title_dimensions.Height) /2;
                double title_y_location = bmp.Height/2 + title_dimensions.Width/2;

                g.TranslateTransform((float) title_x_location, (float) title_y_location);
                g.RotateTransform(270F);

                g.DrawString(title, f, Brushes.Black, 0, 0);
                f.Dispose();
                f2.Dispose();
            }

            g.Dispose();
            pen.Dispose();
        }

        public object Clone()
        {
            YAxis tempYAxis = (YAxis)this.MemberwiseClone();

            List<YAxisIF> tempComponentsList = new List<YAxisIF>();

            foreach(YAxisIF yaif in this.components)
            {
                tempComponentsList.Add((YAxisIF)yaif.Clone());
            }

            tempYAxis.components = tempComponentsList;

            return tempYAxis;
        }
    }

    // Controls line and title for X - Axis 
    public class XAxis : XAxisIF
    {
        private List<XAxisIF> components = new List<XAxisIF>();
        private string title = "X - Axis";
        public bool title_on = true;

        // Controls incrementing for axes 
        public static int incr = 25;

        // Add subcomponent 
        public void addComponent(XAxisIF haif)
        {
            components.Add(haif);
        }


        // Remove subcomponent 
        public void removeComponent(XAxisIF haif)
        {
            components.Remove(haif);
        }

        // Set axis title 
        public void setTitle(string title)
        {
            this.title = title;
        }

        // Draw horizontal line, title, and all subcomponents
        public void draw(Bitmap bmp)
        {
            // Draw line from left point (x1, y1) to right (x2, y2)
            double x1 = bmp.Width * GraphManager.W_PADDING;
            double y1 = bmp.Height * GraphManager.S_PADDING;
            double x2 = bmp.Width * GraphManager.E_PADDING;
            double y2 = y1;

            Pen pen = new Pen(Color.Black, 2);

            Graphics g = Graphics.FromImage(bmp);
            g.DrawLine(pen, (float) x1, (float) y1, (float) x2, (float) y2);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            // Draw Title
            if (title_on)
            {
                Font f = new Font("Times New Roman", 14);
                Font f2 = new Font("Times New Roman", 9);

                // Determine size of title and center it in top
                SizeF title_dimensions = g.MeasureString(title, f);
                SizeF data_label_dimensions = g.MeasureString(GraphManager.X_MAX.ToString("#.###"), f2);
                double title_x_location = bmp.Width / 2 - title_dimensions.Width / 2;
                double title_y_location = bmp.Height * GraphManager.S_PADDING + data_label_dimensions.Width * Math.Sin(45F) + 15;


                g.DrawString(title, f, Brushes.Black, (float) title_x_location, (float) title_y_location);
                f.Dispose();
                f2.Dispose();
            }

            // Draw all subcomponents 
            foreach (XAxisIF xaif in components)
                xaif.draw(bmp);

            g.Dispose();
            pen.Dispose();
            
        }

        public object Clone()
        {
            XAxis tempXAxis = (XAxis)this.MemberwiseClone();

            List<XAxisIF> tempComponentsList = new List<XAxisIF>();

            foreach (XAxisIF xaif in this.components)
            {
                tempComponentsList.Add((XAxisIF)xaif.Clone());
            }

            tempXAxis.components = tempComponentsList;

            return tempXAxis;
        }
    }

    // Class to control horizontal gridlines for the Y-axis
    public class YAxisGridLines : YAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;

        public object Clone()
        {
            YAxisGridLines tempYGrid = (YAxisGridLines)this.MemberwiseClone();

            return tempYGrid;
        }

        public void draw(Bitmap bmp)
        {
            // Draw horizontal lines from left point (x1, newy) to right point (x2, newy)
            double x1 = bmp.Width * GraphManager.W_PADDING;
            double x2 = bmp.Width * GraphManager.E_PADDING;

            double y1 = bmp.Height * GraphManager.N_PADDING;
            double y2 = bmp.Height * GraphManager.S_PADDING;
            double dy = (y2 - y1) / GraphManager.Y_AXIS_INCREMENT;

            Pen pen = new Pen(c, thickness);
            Graphics g = Graphics.FromImage(bmp);

            // Loop through drawing each line for each axis increment
            for (int i = 1; i < GraphManager.Y_AXIS_INCREMENT + 1; i++)
            {
                double newy = y2 - dy * i;
                g.DrawLine(pen, (float) x1, (float) newy, (float) x2, (float) newy);
            }


            g.Dispose();
            pen.Dispose();
        }

    }

    // Class to control vertical gridlines for the X-axis
    public class XAxisGridLines : XAxisIF
    {
        private float thickness = 0.75F;
        private Color c = Color.Gray;

        public object Clone()
        {
            XAxisGridLines tempXGrid = (XAxisGridLines)this.MemberwiseClone();

            return tempXGrid;
        }

        public void draw(Bitmap bmp)
        {
            // Draw vertical lines from bottom point (newx, y1) to top point (newy, y2)
            double x1 = bmp.Width * GraphManager.W_PADDING;
            double x2 = bmp.Width * GraphManager.E_PADDING;
            double dx = (x2 - x1) / GraphManager.X_AXIS_INCREMENT;

            double y1 = bmp.Height * GraphManager.S_PADDING;
            double y2 = bmp.Height * GraphManager.N_PADDING;

            Pen pen = new Pen(c, thickness);
            Graphics g = Graphics.FromImage(bmp);

            // Loop through drawing each line for each axis increment
            for (int i = 1; i < GraphManager.X_AXIS_INCREMENT + 1; i++)
            {
                double newx = x1 + dx * i;
                g.DrawLine(pen, (float) newx, (float) y1, (float) newx, (float) y2);
            }

            g.Dispose();
            pen.Dispose();
        }

    }


    // Controls major tick marks for the Y-axis
   public class YAxisTickMarks : YAxisIF
    {
        private Color c = Color.Black;
        private float thickness = 1;

        public object Clone()
        {
            YAxisTickMarks tempYTicks = (YAxisTickMarks)this.MemberwiseClone();

            return tempYTicks;
        }

        public void draw(Bitmap bmp)
        {
            double y_max = GraphManager.Y_MAX;
            double y_min = GraphManager.Y_MIN;

            double x1 = bmp.Width * (GraphManager.W_PADDING - 0.01);
            double x2 = bmp.Width * GraphManager.W_PADDING;

            double y1 = bmp.Height * GraphManager.N_PADDING;
            double y2 = bmp.Height * GraphManager.S_PADDING;
            double dy = (y2 - y1) / GraphManager.Y_AXIS_INCREMENT;

            Pen pen = new Pen(c, thickness);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            double y_incr = (y_max - y_min) / GraphManager.Y_AXIS_INCREMENT;
            string str;
            Font f = new Font("Times New Roman", 9);

            // Loop through drawing the the tickmark lines and adding data labels
            for (int i = 1; i < GraphManager.Y_AXIS_INCREMENT + 1; i++)
            {
                double newy = y2 - dy * i;
                g.DrawLine(pen, (float) x1, (float) newy, (float) x2, (float) newy);

                double value = (y_min + y_incr * i);

                if(((int) value).ToString().Length > 3)
                {
                   str = ((int) value).ToString(); 
                }
                else
                {
                    str = value.ToString("#.##");
                }
                
                g.DrawString(str, f, Brushes.Black, (float) x1 - 35, (float) newy - 10);

            }

            g.Dispose();
            pen.Dispose();
            f.Dispose();
        }

    }

    // Controls major tick marks for the X-axis
    public class XAxisTickMarks : XAxisIF
    {
        private Color c = Color.Black;
        private float thickness = 1;

        public object Clone()
        {
            XAxisTickMarks tempXTicks = (XAxisTickMarks)this.MemberwiseClone();

            return tempXTicks;
        }

        public void draw(Bitmap bmp)
        {
            double x_max = GraphManager.X_MAX;
            double x_min = GraphManager.X_MIN;


            double x1 = bmp.Width * GraphManager.W_PADDING;
            double x2 = bmp.Width * GraphManager.E_PADDING;
            double dx = (x2 - x1) / GraphManager.X_AXIS_INCREMENT;

            double y1 = bmp.Height * (GraphManager.S_PADDING + 0.01);
            double y2 = bmp.Height * GraphManager.S_PADDING;

            Pen pen = new Pen(c, thickness);
            Graphics g = Graphics.FromImage(bmp);

            double x_incr = (x_max - x_min) / GraphManager.X_AXIS_INCREMENT;
            string str;
            Font f = new Font("Times New Roman", 9);

            // Loop through drawing tick mark lines and adding data labels
            for (int i = 1; i < GraphManager.X_AXIS_INCREMENT + 1; i++)
            {
                g = Graphics.FromImage(bmp);

                double newx = x1 + dx * i;
                g.DrawLine(pen, (float) newx, (float) y1, (float) newx, (float) y2);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                str = ((x_min + x_incr * i)).ToString("#.###");

                g.TranslateTransform((float) newx + 5, (float) y1);
                g.RotateTransform(45F);
                g.DrawString(str, f, Brushes.Black, 0, 0);
            }

            g.Dispose();
            pen.Dispose();
            f.Dispose();
        }

    }
    
}
