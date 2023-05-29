using System;
using System.Drawing;
using System.Windows.Forms;

namespace lab10
{
    public partial class Form1 : Form
    {
        const int step = 10;

        const int width = 1200;
        const int height = 800;

        const int d = 100;
        const int x1 = d, y1 = 2 * height - d;
        const int x2 = 2 * width - d, y2 = d;
        const int r = 25;

        const int valuesNumber = 5;

        Pen pen = new Pen(Color.Red, 6);
        Brush brush = new SolidBrush(Color.Blue);

        public Form1()
        {
            InitializeComponent();

            ClientSize = new Size(width, height);
            Location = new Point(150, 150);

            formsPlot1.Visible = false;

            Draw(this, new PaintEventArgs(CreateGraphics(), Bounds), x1, y1);
        }

        int Factorial(int x)
        {
            return x == 0 ? 1 : x * Factorial(x - 1);
        }

        public double Sin(double x, int n)
        {
            double x_ = x % (Math.PI / 2);
            x_ = ((int)(2 * x / Math.PI)) % 2 == 0 ? x_ : (x_ / Math.Abs(x_)) * Math.PI / 2 - x_;

            double sum = 0;
            for (int i = 0; i < n; i++)
            {
                sum += (i % 2 == 0 ? 1 : -1) * Math.Pow(x_, 2 * i + 1) / Factorial(2 * i + 1);
            }
            return ((int)(x / Math.PI) % 2 == 0 ? 1 : -1) * sum;
        }

        public double Cos(double x, int n)
        {
            return Sin(x + Math.PI / 2, n);
        }

        public double Atan(double x, int n)
        {
            return Cos(x, n) / Sin(x, n);
        }

        private void Draw(object sender, PaintEventArgs e, double x, double y)
        {
            Graphics g = e.Graphics;

            g.ScaleTransform(0.5f, 0.5f);
            g.Clear(BackColor);

            g.DrawEllipse(pen, x1 - r, y1 - r, 2 * r, 2 * r);
            g.DrawEllipse(pen, x2 - r, y2 - r, 2 * r, 2 * r);

            g.FillEllipse(brush, (int)(x - r + 2), (int)(y - r + 2), 2 * r - 4, 2 * r - 4);
        }

        private double PointMove(int n)
        {
            double angle = Math.Atan((double)(y2 - y1) / (x2 - x1));
            double x = x1;
            double y = y1;

            double distance = Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
            double value = distance;

            while (distance <= value)
            {
                Draw(this, new PaintEventArgs(CreateGraphics(), Bounds), x, y);

                System.Threading.Thread.Sleep(5 * step);

                x += step * Cos(angle, n);
                y += step * Sin(angle, n);

                distance = Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
                if (value > distance)
                {
                    value = distance;
                }
            }

            return value;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            double[] dataX = new double[valuesNumber];
            double[] dataY = new double[valuesNumber];

            for (int n = 1; n <= valuesNumber; n++)
            {
                label1.Text = "n = " + Convert.ToString(n);
                label1.Refresh();
                dataX[n - 1] = n;
                dataY[n - 1] = PointMove(n);
            }

            formsPlot1.Visible = true;
            formsPlot1.Plot.AddScatter(dataX, dataY);
            formsPlot1.Refresh();
        }
    }
}
