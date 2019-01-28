using MachineLearning.Distances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.SupervisedLearning.InstanceBasedLearning
{
    public enum DistanceType
    {
        Euclidean
    }
    public class KNearestNeighbour
    {
        private int K;
        private DistanceType type;
        private double[][] inputs;
        private double[] outputs;
        private double[] distance;
        private double[] votting;
        private Distance dis;

        public KNearestNeighbour(int K, DistanceType type)
        {
            this.K = K;
            this.type = type;
        }

        public void learn(double[][] inputs, double[] outputs)
        {
            this.inputs = inputs;
            this.distance = new double[inputs.Length];
            this.dis = new Distance(inputs);
            this.outputs = outputs;
        }
        public double decide(double[] feature)//double rejectionDistance)
        {
            double maxValue = 0;
            if (type == DistanceType.Euclidean)
            {
                Parallel.For(0, inputs.Length, i =>
                {
                    distance[i] = dis.EuclideanDistance(feature, i);
                });
                var distancesTemp = distance;
                var outputsTemp = outputs;
                double temp = 0, temp1 = 0;
                for (int write = 0; write < distance.Length; write++)
                {
                    for (int sort = 0; sort < distancesTemp.Length - 1; sort++)
                    {
                        if (distancesTemp[sort] > distancesTemp[sort + 1])
                        {
                            temp = distancesTemp[sort + 1];
                            temp1 = outputsTemp[sort + 1];
                            distancesTemp[sort + 1] = distancesTemp[sort];
                            outputsTemp[sort + 1] = outputsTemp[sort];
                            distancesTemp[sort] = temp;
                            outputsTemp[sort] = temp1;
                        }
                    }
                }

                var distancesfromK = new double[K];
                var outputsfromK = new double[K];
                Parallel.For(0, K, i =>
                {
                    distancesfromK[i] = distancesTemp[i];
                    outputsfromK[i] = outputsTemp[i];
                });
                var kelasDupe = outputsfromK.Distinct().ToArray();
                votting = new double[kelasDupe.Length];
                for (int i = 0; i < kelasDupe.Length; i++)
                {
                    for (int j = 0; j < distancesfromK.Length; j++)
                    {
                        if (kelasDupe[i] == outputsfromK[j])
                        {
                            votting[i] += 1 / (Math.Pow(distancesfromK[j], 2));
                        }
                    }
                }
                maxValue = votting.Max();
            }
            return votting.ToList().IndexOf(maxValue);
        }
    }
}

