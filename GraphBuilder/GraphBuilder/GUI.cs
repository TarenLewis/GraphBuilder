using System;
using System.Windows.Forms;
using GraphBuilder.Graphing;

namespace GraphBuilder
{
    public partial class GUI : Form
    {
        GraphManager graphManager = new GraphManager();
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

            Point p1 = new Point(16, 16, 2);
            Point p2 = new Point(50, 50, 2);
            Point p3 = new Point(75, 30, 2);
            Point p4 = new Point(90, 65, 2);

            data.addComponent(p1);
            data.addComponent(p2);
            data.addComponent(p3);
            data.addComponent(p4);

            graph.addComponent(data);

            Line line = new Line();
            line.addPoint(p1);
            line.addPoint(p2);
            line.addPoint(p3);
            line.addPoint(p4);

            graph.addComponent(line);


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
