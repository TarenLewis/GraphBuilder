using System;
using System.Collections.Generic;
using System.Drawing;
using GraphBuilder.Manager;

namespace GraphBuilder.Graphing
{
    public interface Cloneable
    {
        Cloneable clone();
    }

    

    // GraphComponentIF to handle composite graph object
    public interface GraphComponentIF : ICloneable
    {
        void draw(Bitmap bmp);
    }


    // Parent composite object
    [Serializable()]
    public class Graph : GraphComponentIF
    {
        private List<GraphComponentIF> components = new List<GraphComponentIF>();
        private string title = "Y vs X";
        public bool title_on = true;

        private string fileName;


        // Add component to subcomponents 
        public void addComponent(GraphComponentIF c)
        {
            components.Add(c);
        }

        // Remove component from subcomponents
        public void removeComponent(GraphComponentIF c)
        {
            components.Remove(c);
        }

        public List<GraphComponentIF> getComponentList()
        {
            return components;
        }

        public void setFileName(string name)
        {
            this.fileName = name;
        }

        public string getFileName()
        {
            return this.fileName;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        // Draw all subcompoents onto graphic 
        public void draw(Bitmap bmp)
        {

            // Draw graph title if needed
            if (title_on)
            {
                Graphics g = Graphics.FromImage(bmp);
                g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);

                Font f = new Font("Times new roman", 16);

                // Determine size of title and center it in top
                SizeF title_dimensions = g.MeasureString(title, f);
                double title_x_location = bmp.Width / 2 - title_dimensions.Width / 2;
                double title_y_location = bmp.Height * GraphManager.N_PADDING / 2 - title_dimensions.Height / 2;

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(title, f, Brushes.Black, (float) title_x_location, (float) title_y_location );

                g.Dispose();
            }

            foreach (GraphComponentIF c in components)
                c.draw(bmp);

        }

        public object Clone()
        {
            // Copy for value types
            Graph tempGraph = (Graph)this.MemberwiseClone();

            // Copy String Reference types
            tempGraph.title = String.Copy(this.title);
            tempGraph.fileName = String.Copy(this.fileName);

            // Copy component references
            List<GraphComponentIF> tempComponentList = new List<GraphComponentIF>();

            foreach(GraphComponentIF gcif in this.components)
            {
                tempComponentList.Add((GraphComponentIF)gcif.Clone());
            }

            // Set equal to new reference
            tempGraph.components = tempComponentList;

            return tempGraph;
        }
    }


}
