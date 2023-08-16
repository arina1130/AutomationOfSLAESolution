using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySLAE
{
    static class Matrix
    {
        public static double[,] СreateMatrixA(EquationSystem system)
        {
            int power = system.Equations.Count;
            double[,] matrix = new double[power, power];
            for (int i = 0; i < power; i++)
            {
                int k = 0;
                for (int j = 0; j < power; j++)
                {
                    matrix[i, j] = system.Equations[i].a[k];
                    k++;
                }
            }
            return matrix;
        }

        public static double[] СreateMatrixB(EquationSystem system)
        {
            int power = system.Equations.Count;
            double[] matrix = new double[power];
            for (int i = 0; i < power; i++)
            {
                matrix[i] = system.Equations[i].b;
            }
            return matrix;
        }

        public static double[,] TransMatrix(double[,] matrix)
        {
            double[,] inv = new double[matrix.GetLength(1), matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    inv[j, i] = matrix[i, j];
                }
            }
            return inv;
        }
       
        public static double[,] ToTriangle(double[,] matrix)
        {
            double[,] m = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < m.GetLength(0); i++)
                for (int j = 0; j < m.GetLength(1); j++)
                    m[i, j] = matrix[i, j];
            double n = m.GetLength(0);
            for (int i = 0; i < n - 1; i++)
                for (int j = i + 1; j < n; j++)
                {
                    double koef = m[j, i] / m[i, i];
                    for (int k = i; k < n; k++)
                        m[j, k] -= m[i, k] * koef;

                }
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    m[i, j] = Math.Round(m[i, j], 2);
                }
            }
            return m;
        }

        public static double FindDet(double[,] matrix)
        {
            double det = 1;
            double[,] m = ToTriangle(matrix);
            for (int i = 0; i < m.GetLength(1); i++)
            {
                det *= m[i, i];
            }
            return det;
        }

        public static double[,] InverseMatrix(double[,] matrix)
        {
            //найти обратную матрицу
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            double[,] transMatrix = TransMatrix(matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = Math.Round(Math.Pow(-1, i + j) * FindDet(GetMinor(transMatrix, i, j)));
                }
            }

            return MatrixMultiplicationByNumber(1 / FindDet(matrix), result);
        }

        public static double[,] MatrixMultiplicationByNumber(double number, double[,] matrix)//умножение матрицы на число
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = number * matrix[i, j];
                }
            }
            return result;
        }

        public static double[,] GetMinor(double[,] matrix, int row, int col)
        {
            double[,] minor = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
            List<double> mList = new List<double>();
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i != row && j != col)
                    {
                        mList.Add(matrix[i, j]);
                    }
                }
            }
            int n = 0, k = 0;
            foreach (double m in mList)
            {
                minor[n, k] = m;
                if (k == minor.GetLength(1) - 1 && n < minor.GetLength(0)) { n++; k = 0; }
                else k++;
            }
            return minor;
        }

        public static double[] MatrixAMultiplicationByMatrixB(double[,] a, double[] b)
        {
            if (a.GetLength(1) != b.GetLength(0)) throw new Exception("Матрицы нельзя перемножить");
            double[] result = new double[b.GetLength(0)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int k = 0; k < b.GetLength(0); k++)
                {
                    result[i] += Math.Round(a[i, k] * b[k], 2);
                }
            }
            return result;
        }

        public static double[,] Difference(double[,] r, double[,] s)
        {
            double[,] d = new double[r.GetLength(0), r.GetLength(1)];

            for (int i = 0; i < d.GetLength(0); i++)
            {
                for (int j = 0; j < d.GetLength(1); j++)
                {
                    d[i, j] = r[i, j] - s[i, j];
                }
            }
            return d;
        }

        public static double[] Difference(double[] r, double[] s)
        {
            double[] d = new double[r.Length];

            for (int i = 0; i < d.GetLength(0); i++)
            {
                d[i] = r[i] - s[i];
            }
            return d;
        }

        public static double FindNorm(double[] m)
        {
            double n = 0;
            for (int i = 0; i < m.Length; i++)
            {
                n += m[i];
            }
            return n;
        }

        public static double[,] MatrixMultiplication(double[,] a, double[,] b)
        {
            if (a.GetLength(1) == b.GetLength(0))
            {
                double[,] res = new double[a.GetLength(0), b.GetLength(1)];
                for (int i = 0; i < res.GetLength(0); i++)
                {
                    for (int j = 0; j < res.GetLength(1); j++)
                    {
                        for (int m = 0; m < a.GetLength(1); m++)
                        {
                            res[i, j] += a[i, m] * b[m, j];
                        }
                    }
                }
                return res;
            }
            else return null;
        }
    }
}
