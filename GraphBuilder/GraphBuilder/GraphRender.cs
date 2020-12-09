using GraphBuilder.Graphing;
using System.Drawing;
using System.Threading;



namespace GraphBuilder.Rendering
{
    // This class handles the request for a RenderFuture object
    public class GraphRenderRequester
    {
        private Bitmap bmp;

        public GraphRenderRequester(Bitmap bmp)
        {
            this.bmp = bmp;
        }

        public RenderFuture getFuture(Graph graph)
        {
            return new RenderFuture(bmp, graph);
        }
    }


    // This class handles rendering the Graph object onto a Bitmap Image asyncnroulsy 
    public class RenderFuture
    {
        private Graph graph;
        private Bitmap result;
        private bool ready = false;
        private readonly object lock_object = new object();
        private Thread runner;

        public RenderFuture(Bitmap bmp, Graph graph)
        {
            this.graph = graph;
            this.result = (Bitmap) bmp.Clone();
            runner = new Thread(new ThreadStart(run));
            runner.Start();
        }


        public bool checkGraphStatus()
        {
            return ready;
        }

        public void waitForResult()
        {
            if (ready)
                return;
            else
                lock (lock_object) { }

            return;
        }

        public Bitmap getResult()
        {
            return result;
        }

        private void run()
        {
            lock (lock_object)
            {
                graph.draw(result);
            }
        }


    }




}
