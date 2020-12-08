using GraphBuilder.Graphing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder.Rendering
{
    // This class is for handling the separate thread that creates the graph
    class RenderFuture
    {
        private Graph graph;
        private Bitmap result;
        private bool ready;

        RenderFuture(Graph g)
        {
            this.graph = g;
        }
 

        public bool checkGraphStatus()
        {
            throw new NotImplementedException();
        }

        public void waitForResult()
        {

        }

        public Bitmap getResult()
        {
            return result;
        }
    }

    class GraphRenderRequester
    {
        private Graph graph;

        GraphRenderRequester(Graph g)
        {
            this.graph = g;
        }

        RenderFuture getFuture()
        {
            throw new NotImplementedException();
        }
    }
}
