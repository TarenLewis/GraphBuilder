using System;
using GraphBuilder.Graphing;
using System.Windows.Forms;
using GraphBuilder.Manager;

namespace GraphBuilder.Observer
{
    interface ObserverIF
    {
        void notify(Panel p);
        double getX();
    }

    class Notifier
    {
        private ObserverIF[] observer_points;
        private double location_x_max, location_x_min;

        public Notifier(Panel p)
        {

            location_x_max = p.Width * GraphManager.E_PADDING;
            location_x_min = p.Width * GraphManager.W_PADDING;

            observer_points = new ObserverIF[p.Width];   
        }

        public void notify(Panel p, int x)
        {
            if(observer_points[x] != null)
                observer_points[x].notify(p);
        }

        public void addObserver(ObserverIF oif)
        {
            int index = (int) ((oif.getX() / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min);
            observer_points[index] = oif;
        }

        public void removeObserver(ObserverIF oif)
        {
            int index = (int)((oif.getX() / (GraphManager.X_MAX - GraphManager.X_MIN)) * (location_x_max - location_x_min) + location_x_min);
            observer_points[index] = null;
        }

        public void clearObservers()
        {
            observer_points = new ObserverIF[observer_points.Length];
        }
    }

}
