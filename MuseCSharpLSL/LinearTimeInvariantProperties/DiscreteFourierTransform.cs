using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearTimeInvariantProperties
{
    public class DiscreteFourierTransform
    {
        public double[] discreteFourierTransform(double[] data)
        {
            int N = data.Length;
            var real = new double[N];
            var imag = new double[N];
            var result = new double[N];
            var pi_div = 2.0 * Math.PI / N;
            Parallel.For(0, N, k =>
            {
                var a = k * pi_div;
                Parallel.For(0, N, n =>
                {
                    real[k] += data[n] * Math.Cos(a * n);
                    imag[k] += data[n] * Math.Sin(a * n);
                });
                result[k] = Math.Sqrt(real[k] * real[k] + imag[k] * imag[k]) ;
            });
            return result;
        }
    }
}
