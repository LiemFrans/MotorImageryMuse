using DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinearTimeInvariantProperties
{
    public class IIR
    {
        private BiQuad[] biQuads;
        private int lengthBiQuads;
        public void Init(BiQuad[] biQuads, int lengthBiQuads)
        {
            this.biQuads = biQuads;
            this.lengthBiQuads = lengthBiQuads;
        }
        private static int REG_SIZE = 100;
        private double[] RegX1 = new double[REG_SIZE];
        private double[] RegX2 = new double[REG_SIZE];
        private double[] RegY1 = new double[REG_SIZE];
        private double[] RegY2 = new double[REG_SIZE];
        private double[] Reg0, Reg1, Reg2;
        public double[] Applyfilter(double[] input)
        {
            var numSigPts = input.Length;
            var output = new double[numSigPts];
            double y;
            for (int i = 0; i < REG_SIZE; i++)
            {
                RegX1[i] = 0.0;
                RegX2[i] = 0.0;
                RegY1[i] = 0.0;
                RegY2[i] = 0.0;
            }
            for (int i = 0; i < numSigPts; i++)
            {
                y = sectCalc(0, input[i]);
                for (int j = 1; j < lengthBiQuads; j++)
                {
                    y = sectCalc(j, y);
                }
                output[i] = y;
            }
            return output;
        }
        private double sectCalc(int k, double x)
        {
            double y, centerTap;
            centerTap = x * biQuads[k].b0 + biQuads[k].b1 * RegX1[k] + biQuads[k].b2 * RegX2[k];
            y = biQuads[k].a0 * centerTap - biQuads[k].a1 * RegY1[k] - biQuads[k].a2 * RegY2[k];
            RegX2[k] = RegX1[k];
            RegX1[k] = x;
            RegY2[k] = RegY1[k];
            RegY1[k] = y;
            return y;
        }
    }

}

