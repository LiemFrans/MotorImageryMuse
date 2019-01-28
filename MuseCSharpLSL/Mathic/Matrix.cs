using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathic
{
    public static class Matrix
    {
        public static double Dot(this double[]a, double[] b)
        {
            if (a.Length != b.Length) throw new Exception("Illegal vector dimensions");
            var sum = 0.0;
            for (int i = 0; i < a.Length; i++) sum += a[i] * b[i];
            return sum;
        }
        public static double[] Dot(this double[,]a, double[] b)
        {
            var barisA = a.GetLength(0);
            var kolomA = a.GetLength(1);
            var barisB = b.Length;
            if(kolomA!=barisB) throw new Exception("Illegal vector dimensions");
            var result = new double[barisA];
            for(int i = 0; i < barisA; i++)
            {
                for(int j = 0; j < kolomA; j++)
                {
                    result[i] += a[i,j] * b[j];
                }
            }
            return result;
        }
        public static double[] DotParallel(this double[,] a, double[] b)
        {
            var barisA = a.GetLength(0);
            var kolomA = a.GetLength(1);
            var barisB = b.Length;
            if (kolomA != barisB) throw new Exception("Illegal vector dimensions");
            var result = new double[barisA];
            Parallel.For(0, barisA, i =>
            {
                for (int j = 0; j < kolomA; j++)
                {
                    result[i] += a[i, j] * b[j];
                }
            });
            return result;
        }
        public static double[] Dot(this double[]a, double[,] b)
        {
            var barisA = a.Length;
            var barisB = b.GetLength(0);
            var kolomB = b.GetLength(1);
            if(barisA!=barisB) throw new Exception("Illegal vector dimensions");
            var result = new double[kolomB];
            for(int j = 0; j < kolomB; j++)
            {
                for(int i = 0; i < barisB; i++)
                {
                    result[j] += b[i, j] * a[i];
                }
            }
            return result;
        }
        public static double[] DotParallel(this double[] a, double[,] b)
        {
            var barisA = a.Length;
            var barisB = b.GetLength(0);
            var kolomB = b.GetLength(1);
            if (barisA != barisB) throw new Exception("Illegal vector dimensions");
            var result = new double[kolomB];
            Parallel.For(0, kolomB, j =>
            {
                for (int i = 0; i < barisB; i++)
                {
                    result[j] += b[i, j] * a[i];
                }
            });
            return result;
        }
        public static double[,] Multiply(this double[,] a, double[,]b)
        {
            var barisA = a.GetLength(0);
            var kolomA = a.GetLength(1);
            var barisB = b.GetLength(0);
            var kolomB = b.GetLength(1);
            if(kolomA!=barisB) throw new Exception("Illegal vector dimensions");
            var result = new double[barisA, kolomB];
            for (int i = 0; i < barisA; i++)
            {
                for (int j = 0; j < kolomB; j++)
                {
                    var x = 0.0;
                    for (int k = 0; k < barisB; k++)
                    {
                        x += a[i, k] * b[k, j];
                    }
                    result[i, j] = x;
                }
            }
            return result;
        }
        public static double[,] MultiplyParallel(this double[,] a, double[,] b)
        {
            var barisA = a.GetLength(0);
            var kolomA = a.GetLength(1);
            var barisB = b.GetLength(0);
            var kolomB = b.GetLength(1);
            if (kolomA != barisB) throw new Exception("Illegal vector dimensions");
            var result = new double[barisA, kolomB];
            Parallel.For(0, barisA, i =>
            {
                Parallel.For(0, kolomB, j =>
                {
                    var x = 0.0;
                    for(int k = 0; k < barisB; k++)
                    {
                        x += a[i, k] * b[k, j];
                    }
                    result[i, j] = x;
                });
            });

            return result;
        }
        public static double[,] Transpose(this double[,] a)
        {
            var barisA = a.GetLength(0);
            var kolomA = a.GetLength(1);
            var result = new double[kolomA, barisA];
            for(int i = 0; i < barisA; i++)
            {
                for(int j = 0; j < kolomA; j++)
                {
                    result[j, i] = a[i, j];
                }
            }
            return result;
        }
        public static double[,] TransposeParallel(this double[,] a)
        {
            var barisA = a.GetLength(0);
            var kolomA = a.GetLength(1);
            var result = new double[kolomA, barisA];
            Parallel.For(0, barisA, i =>
            {
                Parallel.For(0, kolomA, j =>
                {
                    result[j, i] = a[i, j];
                });
            });
            return result;
        }

        /// <summary>
        /// Calculate the inverse of a matrix using Gauss-Jordan elimination.
        /// </summary>
        /// <param name="data">The matrix to invert.</param>
        /// <param name="inverse">The inverse of the matrix.</param>
        /// <returns>false of the matrix is singular, true otherwise.</returns>
        /// <summary>
        /// Calculate the inverse of a matrix using Gauss-Jordan elimination.
        /// </summary>
        /// <param name="data">The matrix to invert.</param>
        /// <param name="inverse">The inverse of the matrix.</param>
        /// <returns>false of the matrix is singular, true otherwise.</returns>
        //public static bool Inverse(double[,] data, double[,] inverse)
        //{
        //    if (data == null)
        //        throw new ArgumentNullException("data");
        //    if (inverse == null)
        //        throw new ArgumentNullException("null");

        //    int drows = data.GetLength(0),
        //        dcols = data.GetLength(1),
        //        irows = inverse.GetLength(0),
        //        icols = inverse.GetLength(1),
        //        n, r;
        //    double scale;



        //    //Validate the matrix sizes
        //    if (drows != dcols)
        //        throw new ArgumentException(

        //        "data is not a square matrix", "data");
        //    if (irows != icols)
        //        throw new ArgumentException(

        //        "inverse is not a square matrix", "inverse");
        //    if (drows != irows)
        //        throw new ArgumentException(

        //        "data and inverse must have identical sizes.");

        //    n = drows;



        //    //Initialize the inverse to the identity
        //    for (r = 0; r < n; ++r)
        //        for (int c = 0; c < n; ++c)
        //            inverse[r, c] = (r == c) ? 1.0 : 0.0;



        //    //Process the matrix one column at a time
        //    for (int c = 0; c < n; ++c)
        //    {
        //        //Scale the current row to start with 1

        //        //Swap rows if the current value is too close to 0.0

        //        if(Math.Abs(data[c, c]) <= 2.0 * double.Epsilon)
        //        {
        //            for (r = c + 1; r < n; ++r)
        //                if (Math.Abs(data[c, c]) <= 2.0 * double.Epsilon)
        //                {
        //                    RowSwap(data, n, c, r);
        //                    RowSwap(inverse, n, c, r);
        //                    break;
        //                }
        //            if (r >= n)
        //                return false;
        //        }
        //        scale = 1.0 / data[c, c];
        //        RowScale(data, n, scale, c);
        //        RowScale(inverse, n, scale, c);



        //        //Zero out the rest of the column

        //        for (r = 0; r < n; ++r)
        //        {
        //            if (r != c)
        //            {
        //                scale = -data[r, c];
        //                RowScaleAdd(data, n, scale, c, r);
        //                RowScaleAdd(inverse, n, scale, c, r);
        //            }
        //        }
        //    }

        //    return true;
        //}

        ///// <summary>
        ///// Swap 2 rows in a matrix.
        ///// </summary>
        ///// <param name="data">The matrix to operate on.</param>
        ///// <param name="cols">
        ///// The number of columns in <paramref name="data"/>.
        ///// </param>
        ///// <param name="r0">One of the 2 rows to swap.</param>
        ///// <param name="r1">One of the 2 rows to swap.</param>
        //private static void RowSwap(double[,] data, int cols,
        //                            int r0, int r1)
        //{
        //    double tmp;

        //    for (int i = 0; i < cols; ++i)
        //    {
        //        tmp = data[r0, i];
        //        data[r0, i] = data[r1, i];
        //        data[r1, i] = tmp;
        //    }
        //}

        ///// <summary>
        ///// Perform scale and add a row in a matrix to another
        ///// row:  data[r1,] = a*data[r0,] + data[r1,].
        ///// </summary>
        ///// <param name="data">The matrix to operate on.</param>
        ///// <param name="cols">
        ///// The number of columns in <paramref name="data"/>.
        ///// </param>
        ///// <param name="a">
        ///// The scale factor to apply to row <paramref name="r0"/>.
        ///// </param>
        ///// <param name="r0">The row to scale.</param>
        ///// <param name="r1">The row to add and store to.</param>
        //private static void RowScaleAdd(double[,] data, int cols,
        //                                double a, int r0, int r1)
        //{
        //    for (int i = 0; i < cols; ++i)
        //        data[r1, i] += a * data[r0, i];
        //}

        ///// <summary>
        ///// Scale a row in a matrix by a constant factor.
        ///// </summary>
        ///// <param name="data">The matrix to operate on.</param>
        ///// <param name="cols">The number of columns in the matrix.</param>
        ///// <param name="a">
        ///// The factor to scale row <paramref name="r"/> by.
        ///// </param>
        ///// <param name="r">The row to scale.</param>
        //private static void RowScale(double[,] data, int cols,
        //                             double a, int r)
        //{
        //    for (int i = 0; i < cols; ++i)
        //        data[r, i] *= a;
        //}

        //public static double[,] PseudoInverse(this double[,] matrix)
        //{
        //    double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];

        //    double determinant = Determinant(matrix);

        //    for (int i = 0; i < matrix.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < matrix.GetLength(1); j++)
        //        {
        //            result[i, j] = Math.Pow(-1, i + j) * matrix[i, j] * Determinant(GetSmallerMatrix(matrix, i, j)) / determinant;
        //        }
        //    }

        //    return TransposeParallel(result);
        //}
        //public static double Determinant(double[,] matrix)
        //{
        //    if ((matrix.GetLength(0) == 2) || (matrix.GetLength(1) == 2))
        //    {
        //        return (matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0]);
        //    }

        //    double result = 0;

        //    for (int i = 0; i < matrix.GetLength(1); i++)
        //    {
        //        double[,] tmpMatrix = GetSmallerMatrix(matrix, 0, i);
        //        result += Math.Pow(-1, 0 + i) * matrix[0, i] * Determinant(tmpMatrix);
        //    }

        //    return result;
        //}   
        //private static double[,] GetSmallerMatrix(double[,] matrix, int row, int column)
        //{
        //    double[,] result = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];

        //    if (result.GetLength(0) < 2 || result.GetLength(1) < 2)
        //        throw new Exception("The length of the matrix can not be less than 2  ");

        //    for (int i = 0, currentRow = 0; i < matrix.GetLength(0); i++)
        //    {
        //        if (i == row) continue;

        //        for (int j = 0, currentColumn = 0; j < matrix.GetLength(1); j++)
        //        {
        //            if (j == column) continue;

        //            result[currentRow, currentColumn] = matrix[i, j];

        //            currentColumn++;
        //        }

        //        currentRow++;
        //    }

        //    return result;
        //}


    }
}
