using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbfNeuro
{
    class Layer
    {
        public static int Count { get; set; }
        public List<Neuron> Neurons;
        public double E { get; set; }
        public double Output { get; set; }

        public Layer(int count, double[] c, double sigma)
        {
            Count = count;
            Neurons = new List<Neuron>();
            for (int i = 0; i < Count; i++)
                Neurons.Add(new Neuron(c[i] ,sigma));
        }

        public void GoLearn(double n, double d, double x)
        {
            LinearSum(x);
            CorrectWeight(n, d, x);
        }

        private void CorrectWeight(double n, double d, double x)
        {
            double val = n * (Output - d);
            Neurons.ForEach(o => o.CorrectWeight(val, x, d, Output, n));
        }

        public void LinearSum(double x)
        {
            Output = 0;
            for (int i = 0; i < Neurons.Count; i++)
            {
                Output += Neurons[i].CalcImpulse(x) * Neurons[i].Weight;
            }

           // Output = Neurons.Select(o => o.CalcImpulse(x) * o.Weight).Sum();
        }


        public bool CalcE(double y, double border)
        {
            E = Math.Pow(Output - y, 2) / 2;
            return E <= border;
        }
    }
}
