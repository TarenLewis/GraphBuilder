using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphBuilder.Observer
{
    interface ObserverIF
    {
        void notify(double x);
    }

    class Notifier
    {
        private List<ObserverIF> observers;

        public void notify(double x)
        {
            throw new NotImplementedException();
        }

        public void addObserver(ObserverIF oif)
        {
            observers.Add(oif);
        }

        public void removeObserver(ObserverIF oif)
        {
            if (observers.Contains(oif))
            {
                observers.Remove(oif);
            }
        }
    }

    class GraphObserver
    {
    }
}
