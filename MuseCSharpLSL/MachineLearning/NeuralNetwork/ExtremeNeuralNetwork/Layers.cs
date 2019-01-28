using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mathic;
using Accord.Math;
namespace MachineLearning.NeuralNetwork.ExtremeNeuralNetwork
{
    public enum Activation
    {
        SigmoidBiner,
        Linear,
        Sin,
        RadialBasis,
        SigmoidBipolar,
        HardLimit,
        Tanh
    }
    class Layers
    {
        private double[,] _input;
        private double[,] _weight;
        private double[] _output;
        private double[] _betaHatt;
        private double[] _yHatt;
        private double[,] _h;
        private double[,] _betaHatt2D;
        private double[,] _output2D;
        private double[,] _yHatt2D;
        public Layers(double[,] weight)
        {
            this._weight = weight;
        }
        public Layers() { }
        public void HiddenLayers(Activation activation)
        {
            var hInit = _input.MultiplyParallel(_weight.TransposeParallel());
            switch (activation)
            {
                case Activation.Linear:
                    _h=Linear(hInit);
                    break;
                case Activation.RadialBasis:
                    _h=RadialBasis(hInit);
                    break;
                case Activation.SigmoidBiner:
                    _h=SigmoidBiner(hInit);
                    break;
                case Activation.SigmoidBipolar:
                    _h=SigmoidBipolar(hInit);
                    break;
                case Activation.Sin:
                    _h=Sin(hInit);
                    break;
                case Activation.HardLimit:
                    _h = HardLimit(hInit);
                    break;
                case Activation.Tanh:
                    _h = Tanh(hInit);
                    break;
            }
            var htranspose = _h.TransposeParallel();
            var hPlus = (htranspose.MultiplyParallel(_h)).PseudoInverse().MultiplyParallel(htranspose);
            _betaHatt = hPlus.DotParallel(_output);
            _yHatt = H.DotParallel(_betaHatt);
        }
        public double[,] SigmoidBiner(double[,] hInit)
        {
            var result = new double[hInit.GetLength(0), hInit.GetLength(1)];
            Parallel.For(0, result.GetLength(0), i =>
            {
                Parallel.For(0, result.GetLength(1), j =>
                {
                    result[i, j] = 1 / (1 + Math.Exp(-hInit[i, j]));
                });
            });
            return result;
        }
        public double[,] Linear(double[,] hInit)
        {
            return hInit;
        }
        public double[,] Sin(double[,] hInit)
        {
            var result = new double[hInit.GetLength(0), hInit.GetLength(1)];
            Parallel.For(0, result.GetLength(0), i =>
            {
                Parallel.For(0, result.GetLength(1), j =>
                {
                    result[i, j] = Math.Sin(hInit[i, j]);
                });
            });
            return result;
        }
        public double[,] RadialBasis(double[,] hInit)
        {
            var result = new double[hInit.GetLength(0), hInit.GetLength(1)];
            Parallel.For(0, result.GetLength(0), i =>
            {
                Parallel.For(0, result.GetLength(1), j =>
                {
                    result[i, j] = Math.Exp(-Math.Pow(hInit[i, j],2));
                });
            });
            return result;
        }
        public double[,] SigmoidBipolar(double[,] hInit)
        {
            var result = new double[hInit.GetLength(0), hInit.GetLength(1)];
            Parallel.For(0, result.GetLength(0), i =>
            {
                Parallel.For(0, result.GetLength(1), j =>
                {
                    result[i, j] = (1-Math.Exp(hInit[i, j]))/(1+(Math.Exp(hInit[i,j])));
                });
            });
            return result;
        }
        public double[,] HardLimit(double[,]hInit) {
            var result = new double[hInit.GetLength(0), hInit.GetLength(1)];
            Parallel.For(0, result.GetLength(0), i =>
            {
                Parallel.For(0, result.GetLength(1), j =>
                {
                    if (hInit[i, j] >= 0)
                    {
                        result[i, j] = 1;
                    }
                    else
                    {
                        result[i, j] = 0;
                    }
                });
            });
            return result;
        }
        public double[,] Tanh(double[,] hInit)
        {
            var result = new double[hInit.GetLength(0), hInit.GetLength(1)];
            Parallel.For(0, result.GetLength(0), i =>
            {
                Parallel.For(0, result.GetLength(1), j =>
                {
                    result[i, j] = (2 / (1 + Math.Exp(-2 * hInit[i, j]))) - 1;
                });
            });
            return result;
        }
        public double[] HiddenLayers(double[,]input, double[,]weight, double[]betaHatt, Activation activation)
        {
            var hInit = input.MultiplyParallel(weight.TransposeParallel());
            var h = new double[hInit.GetLength(0), hInit.GetLength(1)];
            switch (activation)
            {
                case Activation.Linear:
                    h = Linear(hInit);
                    break;
                case Activation.RadialBasis:
                    h = RadialBasis(hInit);
                    break;
                case Activation.SigmoidBiner:
                    h = SigmoidBiner(hInit);
                    break;
                case Activation.SigmoidBipolar:
                    h = SigmoidBipolar(hInit);
                    break;
                case Activation.Sin:
                    h = Sin(hInit);
                    break;
                case Activation.HardLimit:
                    h = HardLimit(hInit);
                    break;
                case Activation.Tanh:
                    h = Tanh(hInit);
                    break;
            }
            return h.DotParallel(betaHatt);
        }
        public void HiddenLayers2DOutputs(Activation activation)
        {
            var hInit = _input.MultiplyParallel(_weight.TransposeParallel());
            switch (activation)
            {
                case Activation.Linear:
                    _h = Linear(hInit);
                    break;
                case Activation.RadialBasis:
                    _h = RadialBasis(hInit);
                    break;
                case Activation.SigmoidBiner:
                    _h = SigmoidBiner(hInit);
                    break;
                case Activation.SigmoidBipolar:
                    _h = SigmoidBipolar(hInit);
                    break;
                case Activation.Sin:
                    _h = Sin(hInit);
                    break;
                case Activation.HardLimit:
                    _h = HardLimit(hInit);
                    break;
                case Activation.Tanh:
                    _h = Tanh(hInit);
                    break;
            }
            var htranspose = _h.TransposeParallel();
            var hPlus = (htranspose.MultiplyParallel(_h)).PseudoInverse().MultiplyParallel(htranspose);
            _betaHatt2D = hPlus.MultiplyParallel(_output2D);
            _yHatt2D = H.MultiplyParallel(_betaHatt2D);
        }
        public double[,] HiddenLayers2DOutputs(double[,] input, double[,] weight, double[,] betaHatt, Activation activation)
        {
            var hInit = input.MultiplyParallel(weight.TransposeParallel());
            var h = new double[hInit.GetLength(0), hInit.GetLength(1)];
            switch (activation)
            {
                case Activation.Linear:
                    h = Linear(hInit);
                    break;
                case Activation.RadialBasis:
                    h = RadialBasis(hInit);
                    break;
                case Activation.SigmoidBiner:
                    h = SigmoidBiner(hInit);
                    break;
                case Activation.SigmoidBipolar:
                    h = SigmoidBipolar(hInit);
                    break;
                case Activation.Sin:
                    h = Sin(hInit);
                    break;
                case Activation.HardLimit:
                    h = HardLimit(hInit);
                    break;
                case Activation.Tanh:
                    h = Tanh(hInit);
                    break;
            }
            return h.MultiplyParallel(betaHatt);
        }
        public double[,] Input
        {
            set
            {
                _input = value;
            }
        }
        public double[] Output
        {
            set
            {
                _output = value;
            }
        }
        public double[,] Output2D
        {

            set
            {
                _output2D = value;
            }
        }
        public double[] BetaHatt
        {
            get
            {
                return _betaHatt;
            }
        }
        public double[,] BetaHatt2D
        {
            get
            {
                return _betaHatt2D;
            }
        }
        public double[,] H
        {
            get
            {
                return _h;
            }
        }
        public double[,] Weight
        {
            get
            {
                return _weight;
            }
        }
        public double[] YHatt
        {
            get
            {
                return _yHatt;
            }
        }
        public double[,] YHatt2D
        {
            get
            {
                return _yHatt2D;
            }
        }

    }
}

