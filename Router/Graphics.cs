using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Router
{
    public partial class Graphics : Form
    {
        public List<double> drawValues = new List<double>();
        public Graphics()
        {
            InitializeComponent();
        }

        private void Graphics_Load(object sender, EventArgs e)
        {
            drawValues.Add(10);
            drawValues.Add(15);
            drawValues.Add(40);
            drawValues.Add(25);
            drawValues.Add(55);
            drawValues.Add(75);
            drawValues.Add(45);
            drawValues.Add(25);
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            System.Drawing.Graphics g = this.chart1.CreateGraphics();

            int valueSpacing = 20;

            int visibleValues = Math.Min(this.Width / valueSpacing, drawValues.Count);
            //"trick": initialize the first previous Point outside the bounds
            Point previousPoint = new Point(Width , Height);
            Point currentPoint = new Point();

            // Only draw average line when possible (visibleValues) 
            //and needed (style setting)
            // [...]
            Pen pen = new Pen(Color.BlueViolet);
            // Connect all visible values with lines
            for (int i = 0; i < visibleValues; i++)
            {
                currentPoint.X = previousPoint.X - valueSpacing;
                currentPoint.Y = previousPoint.Y + valueSpacing;//CalcVerticalPosition(drawValues[i]);

                // Actually draw the line
                g.DrawLine(pen, previousPoint,
                           currentPoint);

                previousPoint = currentPoint;
            }
            ControlPaint.DrawBorder3D(g, 0, 0, Width, Height);
        }

    }
}
