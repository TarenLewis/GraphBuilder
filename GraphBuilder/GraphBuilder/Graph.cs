using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GraphBuilder.Manager;

namespace GraphBuilder.Graphing
{
    public interface Cloneable
    {
        void clone();
    }

    
    public interface GraphIF
    {

    }

    // GraphComponentIF to handle composite graph object
    public interface GraphComponentIF : ICloneable
    {
        void draw(Panel p);
    }


    // Parent composite object
    public class Graph : GraphComponentIF, GraphIF
    {
        private List<GraphComponentIF> components = new List<GraphComponentIF>();
        private string title = "Y vs X";
        public bool title_on = true;
        public string fileName;
        private string componentType = "Graph";

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
        public void draw(Panel p)
        {

            if (title_on)
            {
                Graphics g = p.CreateGraphics();
                Font f = new Font("Times new roman", 16);

                g.DrawString(title, f, Brushes.Black, p.Width / 2, 5);
            }

            foreach (GraphComponentIF c in components)
                c.draw(p);

        }

        public string getComponentType()
        {
            return componentType;
        }

        public object Clone()
        {
            return this.Clone();
        }
    }


}
