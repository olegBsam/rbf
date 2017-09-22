using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rbfNeuro
{
    class Generator
    {
        public static Random rand = new Random();
        public Generator(){}
        public double[] Generate(int count, double lambda)
        {
            double[] array = new double[count];
            for (int i = 0; i < count; i++)
            {
                double y = rand.NextDouble();
                array[i] = (y == 0) ? 0 : -1 / lambda * Math.Log(1 - y);
            }
            return array;
        }
    }
}
