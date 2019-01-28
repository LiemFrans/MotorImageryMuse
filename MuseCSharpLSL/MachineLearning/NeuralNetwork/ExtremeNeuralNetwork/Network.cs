using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.NeuralNetwork.ExtremeNeuralNetwork
{
    public class Network
    {
        private double[,] weight;
        private double _randomMin, _randomMax;
        private int _hiddenLayers;
        public Network(int hiddenLayers, double randomMin, double randomMax)
        {
            _hiddenLayers = hiddenLayers;
            _randomMax = randomMax;
            _randomMin = randomMin;
        }
        public void generateNetworkWeight(int inputLayers)
        {
            var ran = new Random(0);
            this.weight = new double[_hiddenLayers, inputLayers];
            Parallel.For(0, _hiddenLayers, i =>
            {
                Parallel.For(0, inputLayers, j =>
                {
                    weight[i, j] = ran.NextDouble() * (_randomMax - _randomMin) + _randomMin;
                });
            });
        }
        public double[,] Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }
    }
}
