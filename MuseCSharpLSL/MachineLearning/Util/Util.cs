using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning.Util
{
    public class Util
    {
        public double NormalizedMinMax(double value, double min, double max)
        {
            return ((value - min) * (1 - 0)) / ((max - min) + 0);
        }
    }
}
