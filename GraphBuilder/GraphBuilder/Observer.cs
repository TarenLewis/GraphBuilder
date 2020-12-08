using System.Windows.Forms;

namespace GraphBuilder.Graphing
{
    public interface ObserverIF
    {

        void notify(Panel p);
    }


    public class Notifier
    {
        int[] x_values;
        Point[] points;

        public Notifier(int x)
        {
            x_values = new int[x];
            points = new Point[x];
        }


        public void notify(Panel p, int x)
        {

        }
    }
}
