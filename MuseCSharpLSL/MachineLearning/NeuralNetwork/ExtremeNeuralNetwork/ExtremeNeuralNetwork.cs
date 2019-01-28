using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MachineLearning.NeuralNetwork.ExtremeNeuralNetwork
{
    public class ExtremeNeuralNetwork
    {
        private Network _net;
        private Layers _layer;
        private double[,] _weight;
        private double[,] _inputTraining;
        private double[] _outputTraining;
        private double[,] _inputTesting;
        private double[] _outputTesting;

        private double[] _betaHatt;
        private double[,] _betaHatt2D;
        private Activation _activation;
        public double Acc;
        public double[] YHattTraining;
        public double[,] YHattTraining2D;
        public ExtremeNeuralNetwork() { }
        public ExtremeNeuralNetwork(Network network, Activation activation)
        {
            this._net = network;
            this._activation = activation;
        }
        public ExtremeNeuralNetwork(Activation activation) { _activation = activation; }
        public double teach(double[,] input, double[] output)
        {
            _net.generateNetworkWeight(input.GetLength(1));
            _layer = new Layers(_net.Weight);
            _layer.Input = input;
            _layer.Output = output;
            _layer.HiddenLayers(_activation);
            _weight = _layer.Weight;
            _betaHatt = _layer.BetaHatt;
            var error = MAPEEval(_layer.YHatt, output);
            Acc = Accuration(_layer.YHatt, output);
            YHattTraining = _layer.YHatt;
            return error;
        }

        public double teach(double[,] input, double[,] output)
        {
            _net.generateNetworkWeight(input.GetLength(1));
            _layer = new Layers(_net.Weight);
            _layer.Input = input;
            _layer.Output2D = output;
            _layer.HiddenLayers2DOutputs(_activation);
            _weight = _layer.Weight;
            _betaHatt2D = _layer.BetaHatt2D;
            YHattTraining2D = _layer.YHatt2D;
            Acc = Accuration(YHattTraining2D, output);
            return Acc;
        }


        public void teach(double[,] input, double[] output, bool optimization = true)
        {
            _layer = new Layers(_weight);
            _layer.Input = input;
            _layer.Output = output;
            _layer.HiddenLayers(_activation);
            _weight = _layer.Weight;
            _betaHatt = _layer.BetaHatt;
            YHattTraining = _layer.YHatt;
        }
        public double[,] ConvertTo2DArrayKelas(int[]KelasNonDupe, double[] kelas)
        {
            var outs = new double[kelas.Length, KelasNonDupe.Length];
            for(int i = 0; i < kelas.Length; i++)
            {
                for(int j = 0; j < KelasNonDupe.Length; j++)
                {
                    if (kelas[i] == (j+1))
                    {
                        outs[i, j] = 1;
                    }
                    else
                    {
                        outs[i, j] = -1;
                    }
                }
            }
            return outs;
        }
        public double[] testing(double[,] input,double[,]weight,double[] betaHatt, Activation activation)
        {
            _layer = new Layers();
            return _layer.HiddenLayers(input, weight, betaHatt, activation);
        }
        public double[] testing(double[,]input, Activation activation)
        {
            return _layer.HiddenLayers(input, _layer.Weight, _layer.BetaHatt, activation);
        }
        public double[,] testing(double[,] input, double[,] weight, double[,] betaHatt, Activation activation)
        {
            _layer = new Layers();
            return _layer.HiddenLayers2DOutputs(input, weight, betaHatt, activation);
        }

        public double MAPEEval(double[] yHatt, double[] y)
        {
            var temp = new double[y.Length];
            Parallel.For(0, y.Length, i =>
            {
                temp[i] = Math.Abs(((yHatt[i] - y[i]) / y[i]) * 100);
            });
            return (1.0 / Convert.ToDouble(y.Length)) * temp.Sum();
        }
        public double Accuration(double[] yHatt, double[] y)
        {
            var temp = new double[y.Length];
            Parallel.For(0, y.Length, i =>
            {
                if (Math.Round(yHatt[i]) == Math.Round(y[i]))
                {
                    temp[i] = 1;
                }
                else
                {
                    temp[i] = 0;
                }
            });
            return (temp.Sum()/(double)temp.Length)*100;
        }
        public double Accuration(double[,] yHatt2D, double[,] y2D)
        {
            var temp = new double[y2D.GetLength(0)];
            Parallel.For(0, y2D.GetLength(0), i =>
            {
                var yHattMax = Enumerable.Range(0, yHatt2D.GetLength(1)).Max(j => yHatt2D[i, j]);
                var yMax = Enumerable.Range(0, y2D.GetLength(1)).Max(j => y2D[i, j]);
                int maxIndexyHatt = -1;
                int maxIndexy = -1;
                for (int j = 0; j < yHatt2D.GetLength(1); j++)
                {
                    if (yHatt2D[i, j] == yHattMax)
                    {
                        maxIndexyHatt = j;
                    }
                    if (y2D[i, j] == yMax)
                    {
                        maxIndexy = j;
                    }
                }
                if (maxIndexy == maxIndexyHatt && maxIndexy != -1 && maxIndexyHatt != -1)
                {
                    temp[i] = 1;
                }
                else
                {
                    temp[i] = 0;
                }
            });
            return (temp.Sum() / (double)temp.GetLength(0)) * 100;
        }
        public double[,] Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }
        public double[] BetaHatt
        {
            get {
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
    }
}

