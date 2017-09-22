using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace rbfNeuro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            generator = new Generator();
        }

        private const int Count = 20;
        private static Generator generator;

        private static double[] genY, genYNotSort,
                                chartVal = new double[Count],
                                chartX = new double[Count];
        double deltaX;

        private void GetXArray(double[] generated, out double[] masX, out double[] masY)
        {
            int i = 0;

            masX = new double[Count];
            masY = new double[Count];

            deltaX = (generated.Max() - generated.Min()) / Count;
            for (double val1 = generated[0], val2 = val1 + deltaX; i < Count; masX[i] = val1 + deltaX / 2, val1 = val2, val2 += deltaX, i++)
                for (int k = 0; k < generated.Length; k++)
                    if (generated[k] >= val1 && generated[k] < val2)
                        masY[i]++;

            double minY = masY.Min();
            double dif = masY.Max() - minY;

            for (int k = 0; k < masY.Length; k++)
            {
                //masY[k] = masY[k] / masY.Length;
                masY[k] = (masY[k] - minY) / dif;
            }
        }

        Layer layer;
        private void button3_Click(object sender, EventArgs e)
        {
            //layer.Neurons.ForEach(o => o.Impulse = 0);
            //foreach (var o in layer.Neurons)
            //{
            //    o.Impulse;
            //}
            //layer.Output = 0;

            double[] masX;
            double[] masY;

            GetXArray(generator.Generate(5000, 1).OrderBy(o => o).ToArray(), out masX, out masY);


            double[] neuroOutput = new double[20];

            for (int i = 0; i < masX.Length; i++)
            {
                layer.LinearSum(masX[i]);
                neuroOutput[i] = layer.Output;

            }

            ToChart(2, masX, neuroOutput);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            layer = new Layer(20, chartX, deltaX / Math.Sqrt(40));
            double n = 0.0015;///////////////////////////////////////////////


            int Iter = 5000;
            double[] err = new double[Iter * Count];
            double[] indexes = new double[Iter * Count];
            for (int i = 0; i < Iter * Count; i++)
                indexes[i] = i;

            Thread t = new Thread(() =>
            {
                for (int i = 0; i < Iter; i++)
                {
                    double[] m = new double[20];
                    for (int k = 0; k < 20; k++)
                    {
                        layer.GoLearn(n, chartVal[k], chartX[k]);
                        layer.CalcE(chartVal[k], 0.00001);
                        err[k * i] = layer.E;
                        m[k] = layer.Output;
                    }
                    chart1.Invoke((MethodInvoker)delegate () { ToChart(1, chartX, m); });
                }
                label1.Invoke((MethodInvoker)delegate () { label1.Text = "готово"; });
            });
            t.Start();

        }

        private void ToChart(int ind, double[] x, double[] y)
        {
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Points.DataBindXY(x, y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            genYNotSort = generator.Generate(5000, Generator.rand.NextDouble() * 3);
            genY = genYNotSort.OrderBy(o => o).ToArray();
            GetXArray(genY, out chartX, out chartVal);
            ToChart(0, chartX, chartVal);
        }
    }
}
