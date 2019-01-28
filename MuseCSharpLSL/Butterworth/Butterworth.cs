using DataType;
using LinearTimeInvariantProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Butterworth
{
    public enum Filter
    {
        LowPass,
        BandPass,
        HighPass
    }
    public class DesignButterworth
    {
        private double[] input, output;
        private int order;
        private double fc0, fc1;
        private double fc;
        private BiQuad[] biQuadsSections;
        private IIR iir;

        public double[] Input
        {
            set
            {
                input = value;
                output = new double[input.Length];
            }
        }
        public double[] Output
        {
            get
            {
                return output;
            }
        }
        public DesignButterworth(Filter filter, int order, double fc0, double fs)
        {
            this.fc0 = fc0;
            double omega = Math.Tan(Math.PI * fc1 / fs);
            if (filter == Filter.LowPass)
            {
                coefficientLowPass(order, omega);
            }
        }
        public DesignButterworth(Filter filter, int order, double fc0, double fc1, double fs)
        {
            this.fc0 = fc0;
            this.fc1 = fc1;
            this.fc = Math.Sqrt(fc0 * fc1);
            double omega0 = Math.Tan(Math.PI * fc0 / fs);
            double omega1 = Math.Tan(Math.PI * fc1 / fs);
            if (filter == Filter.BandPass)
            {
                coefficientBandPass(order, omega0, omega1);
            }
        }
        public void iirInitialization()
        {
            iir = new IIR();
            iir.Init(biQuadsSections, biQuadsSections.Length);
        }//FIRST AFTER DESIGNING
        public void compute()  
        {
            output = iir.Applyfilter(input);    
        }//COMPUTE
        private void coefficientBandPass(int order, double omega0,double omega1)
        {
            Complex[] poles = complexPolesBandPass(order, omega0, omega1);
            poles = billinearTransformPoles(poles);
            biQuadsSections = new BiQuad[order];
            Parallel.For(0, poles.Length, k =>
            {
                biQuadsSections[k] = new BiQuad(0, -1, -2 * poles[k].re, Complex.AbsSqr(poles[k]));
            });
            if (order % 2 != 0)
            {
                double omega2 = Math.Sqrt(omega0 * omega1);
                double p = -(omega1 - omega0) / omega2;
                Complex pole;
                if (Math.Abs(p) > 2)
                {
                    double u = Math.Sqrt(-4 * Math.Pow(p, 2));
                    pole = 0.5 * omega2 * new Complex(p - u, 0);
                }
                else
                {
                    double u = Math.Sqrt(4 - Math.Pow(p, 2));
                    pole = 0.5 * omega2 * new Complex(p - u, 0);
                }
                pole = billinearTransformPoles(pole);
                biQuadsSections[poles.Length] = new BiQuad(0, -1, -2 * pole.re, Complex.AbsSqr(pole));
            }
            double omega = 2 * Math.Atan(omega0 * omega1);
            biQuadsSections = normalizationBiQuadsSections(biQuadsSections, omega);
        }
        private void coefficientLowPass(int order, double omega)
        {
            Complex[] poles = complexPolesLowPass(order, omega);
            poles = billinearTransformPoles(poles);
            biQuadsSections = new BiQuad[(order + 1) / 2];
            Parallel.For(0, poles.Length, k =>
            {
                biQuadsSections[k] = new BiQuad(0, -1, -2 * poles[k].re, Complex.AbsSqr(poles[k]));
            });
            if (order % 2 != 0)
            {
                double pole = billinearTransformOmega(omega);
                biQuadsSections[poles.Length] = new BiQuad(1, 0, -pole, 0);

            }
            biQuadsSections = normalizationBiQuadsSections(biQuadsSections, omega);
        }
        private Complex[] complexPolesBandPass(int order, double omega0, double omega1)
        {
            Complex[] poles = new Complex[2 * (order / 2)];
            double omega2 = Math.Sqrt(omega0 * omega1);
            for (int k = 0; k < poles.Length / 2; k++)
            {
                double angular = ((2 * k + 1) * Math.PI) / (2 * order);
                Complex p = ((omega1 - omega0) / omega2) * new Complex(-Math.Sin(angular), Math.Cos(angular));
                double q = 0.5 * (4 - Math.Pow(p.re, 2) + Math.Pow(p.im, 2));
                double u = Math.Sqrt(q + Math.Sqrt(Math.Pow(q, 2) + Math.Pow(p.re * p.im, 2)));
                double v = p.re * p.im / u;
                poles[k] = 0.5 * omega2 * new Complex(p.re + v, p.im + u);
                poles[k + poles.Length / 2] = 0.5 * omega2 * new Complex(p.re - v, -p.im + u);
            }
            return poles;
        }
        private Complex[] complexPolesLowPass(int order, double omega)
        {
            var poles = new Complex[order / 2];
            Parallel.For(0, poles.Length, k =>
            {
                var angular = ((2 * k + 1) * Math.PI) / (2 * order);
                poles[k] = omega * new Complex(-Math.Sin(angular), Math.Cos(angular));
            });
            return poles;
        }
        private Complex[] billinearTransformPoles(Complex[] poles)
        {
            for (int k = 0; k < poles.Length; k++)
            {
                poles[k] = billinearTransformPoles(poles[k]);
            }
            return poles;
        }
        private Complex billinearTransformPoles(Complex poles)
        {
            return (1 + poles) / (1 - poles);
        }
        private double billinearTransformOmega(double omega)
        {
            return (1 + omega) / (1 - omega);
        }
        private BiQuad[] normalizationBiQuadsSections(BiQuad[] biQuadsSections, double omega)
        {
            Complex z1 = new Complex(Math.Cos(omega), -Math.Sin(omega));
            double skala;
            for (int k = 0; k < biQuadsSections.Length; k++)
            {
                Complex num = biQuadsSections[k].b0 + biQuadsSections[k].b1 * z1 * biQuadsSections[k].b2 * z1 * z1;
                Complex denom = biQuadsSections[k].a0 + biQuadsSections[k].a1 * z1 + biQuadsSections[k].a2 * z1 * z1;
                skala = 1 / Math.Sqrt(Complex.AbsSqr(num / denom));
                biQuadsSections[k].b0 *= skala;
                biQuadsSections[k].b1 *= skala;
                biQuadsSections[k].b2 *= skala;
            }
            return biQuadsSections;
        }
        
    }
}
