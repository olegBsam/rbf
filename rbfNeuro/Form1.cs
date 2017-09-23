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

           // generated = generated.Select(o => o / 10).ToArray();

            deltaX = (generated.Max() - generated.Min()) / Count;
            for (double val1 = generated[0], val2 = val1 + deltaX; i < Count; maasss[i] = val1, masX[i] = val1 + deltaX / 2, val1 = val2, val2 += deltaX, i++)
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


        double[] maasss = new double[Count];
        Layer layer;

        List<double> errors = new List<double>();
        private void button3_Click(object sender, EventArgs e)
        {

          
            double error = 0;

            layer.Neurons.ForEach(o => o.Impulse = 0);
            layer.Output = 0;

            double[] masX;
            double[] masY;

            GetXArray(generator.Generate(5000, Generator.rand.NextDouble() * 3).OrderBy(o => o).Select(o => o / 1.1).ToArray(), out masX, out masY);


            double[] neuroOutput = new double[Count];

            for (int i = 0; i < masX.Length; i++)
            {
                layer.LinearSum(masX[i]);
                neuroOutput[i] = layer.Output;
                error += (Math.Pow(masY[i] - layer.Output, 2));
            }
            errors.Add(Math.Sqrt(error / (Count - 1)));


            // ToChart(2, masX, masY);
            ToChart(3, masX, neuroOutput);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            layer = new Layer(Count, chartX, deltaX / Math.Sqrt(45));
            double n = 0.0012;///////////////////////////////////////////////

            for (int p = 0; p < 80; p++)
            {
                genYNotSort = generator.Generate(5000, 1 /*Generator.rand.NextDouble() * 3*/);
                genY = genYNotSort.OrderBy(o => o).ToArray();
                GetXArray(genY, out chartX, out chartVal);

                int Iter = 8000;
                double[] err = new double[Iter * Count];
                double[] indexes = new double[Iter * Count];
                for (int i = 0; i < Iter * Count; i++)
                    indexes[i] = i;
                label1.Text = "....";
                ////Thread t = new Thread(() =>
                ////{
                    for (int i = 0; i < Iter; i++)
                    {
                        double[] m = new double[Count];
                        for (int k = 0; k < Count; k++)
                        {
                            layer.GoLearn(n, chartVal[k],/* chartX*/ maasss[k]);

                            layer.CalcE(chartVal[k], 0.00001);
                        /// { label1.Text = "готово"; return; }
                        err[k * i] = layer.E;
                            m[k] = layer.Output;
                        }
                       // chart1.Invoke((MethodInvoker)delegate ()
                        //{
                            ToChart(1, chartX, m);
                       // });
                    }
                    //label1.Invoke((MethodInvoker)delegate ()
                    //{
                        label1.Text = "готово";
                    //});
               // });
                //t.Start();

            }

        }

        private void ToChart(int ind, double[] x, double[] y)
        {
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[0].Points.DataBindXY(x, y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            genYNotSort = generator.Generate(5000, 1/* Generator.rand.NextDouble() * 3*/);
            genY = genYNotSort.OrderBy(o => o).ToArray();
            GetXArray(genY, out chartX, out chartVal);
            ToChart(0, chartX, chartVal);
        }
    }
}
