
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GraphBuilder.Graphing
{
    public interface Cloneable
    {
        void clone();
    }

    
    public interface GraphIF
    {

    }


    public interface GraphComponentIF
    {
        void draw(Panel p);
    }

    public class Graph : GraphComponentIF, GraphIF
    {
        private Rectangle frame;
        private List<GraphComponentIF> components = new List<GraphComponentIF>(); 


        // Draw all subcompoents onto graphic 
        public void draw(Panel p)
        {
            foreach (GraphComponentIF c in components)
                c.draw(p);
        }

        // Add component to subcompoents 
        public void addComponent(GraphComponentIF c)
        {
            components.Add(c);
        }

        // Remove component from subcomponents
        public void removeComponent(GraphComponentIF c)
        {
            components.Remove(c);
        }

    }


}
