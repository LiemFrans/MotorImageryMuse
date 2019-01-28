using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Distances
{
    public class Distance
    {
        private double[][] featureTraining;
        private int lengthFeature;
        public Distance(double[][] featureTraining)
        {
            this.featureTraining = featureTraining;
            this.lengthFeature = featureTraining.First().Length;
        }
        public double EuclideanDistance(double[] featureTesting, int j)
        {
            var temp = new double[lengthFeature];
            Parallel.For(0, lengthFeature, i =>
            {
                temp[i] = Math.Pow(featureTesting[i] - featureTraining[j][i], 2);
            });
            return Math.Sqrt(temp.Sum());
        }
    }
}
