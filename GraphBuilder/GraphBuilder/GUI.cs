using System;
using System.Windows.Forms;
using GraphBuilder.Graphing;

namespace GraphBuilder
{
    public partial class GUI : Form
    {
        Graph graph = new Graph();

        HAxis haxis = new HAxis();
        HAxisGridLines hgridlines = new HAxisGridLines();
        HAxisTickMarks htickmarks = new HAxisTickMarks();
        

        VAxis vaxis = new VAxis();
        VAxisGridLines vgridlines = new VAxisGridLines();
        VAxisTickMarks vtickmarks = new VAxisTickMarks();

        Data data = new Data();

        public GUI()
        {
            InitializeComponent();

            Data.setLimits(0, 100, 0, 100);

            vaxis.addComponent(vgridlines);
            vaxis.addComponent(vtickmarks);

            haxis.addComponent(hgridlines);
            haxis.addComponent(htickmarks);

            graph.addComponent(haxis);
            graph.addComponent(vaxis);

            Point p1 = new Point(4, 4, 5);
            Point p2 = new Point(8, 8, 5);
            Point p3 = new Point(50, 50, 5);

            data.addComponent(p1);
            data.addComponent(p2);
            data.addComponent(p3);

            graph.addComponent(data);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            graph.draw(display);
        }
    }
}
