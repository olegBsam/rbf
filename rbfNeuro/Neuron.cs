using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbfNeuro
{
    class Neuron
    {
        public static Random rand = new Random();
        public double Weight { get; set; }
        public double Sigma { get; set; }
        public double C { get; set; }
        public double Impulse { get; set; }

        public Neuron(double c, double sig)
        {
            Sigma = sig;
            C = c;
            Weight = rand.NextDouble();// - 0.5;
        }
        public double CalcImpulse(double x)
        {
            double different = -Math.Pow(x - C, 2);
            different = different / (2 * Math.Pow(Sigma, 2));
            Impulse = Math.Exp(different);
            return Impulse;
        }
        public void CorrectWeight(double corr, double x, double d, double y, double n)
        {
            double w = Weight, c = C, o = Sigma;

            double xDifc = x - c, xDifc2 = xDifc * xDifc, sigmPow2 = Math.Pow(o, 2),
                   yDifd = y - d;

            w = w - corr * Impulse;

            double buf = yDifd * w * Math.Exp(-xDifc2 / (2 * sigmPow2));
            double dE_dc = buf * (xDifc / sigmPow2);

            c = c - n * dE_dc;

            o = o - n * dE_dc * xDifc / o;

            Weight = w; C = c; Sigma = o;
        }
    }
}
