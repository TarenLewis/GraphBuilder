using GraphBuilder.Graphing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphBuilder.Rendering
{
    // This class is for handling the separate thread that creates the graph
    public class GraphRenderRequester
    {
        private Bitmap bmp;
        RenderFuture renderFuture;

        public GraphRenderRequester(Panel p)
        {
            bmp = new Bitmap(p.Width, p.Height);
        }

        public RenderFuture getFuture(Graph graph)
        {
            return new RenderFuture(bmp, graph);
        }
    }

    public class RenderFuture
    {
        public Bitmap bitmap;
        private Graph graph;
        private bool resultready;

        public RenderFuture(Bitmap b, Graph gr)
        {
            this.bitmap = b;
            this.graph = gr;
            RenderThread renderThread = new RenderThread(bitmap, graph, resultready);
            Thread t = new Thread(new ThreadStart(renderThread.start));
        }

        public bool checkIfReady()
        {
            return resultready;
        }

        private class RenderThread
        {
            Bitmap bitmap;
            Graph graph;
            bool status;
            public RenderThread(Bitmap bm, Graph gr, bool s) {
                bitmap = bm;
                graph = gr;
                status = s;
            }


            public void start()
            {

                //Either create new panel with same size as bmp 
                Panel mypanel = new Panel();
                mypanel.Size = new Size(bitmap.Width, bitmap.Height);
                graph.draw(mypanel);
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

                mypanel.DrawToBitmap(bitmap, rect);
                status = true;

                // or change draw to accept graphics shouldnt be to bad besides line draw function 
                // might be a pain

            }
        }
    }
}
