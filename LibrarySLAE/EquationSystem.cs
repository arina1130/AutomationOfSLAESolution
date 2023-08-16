using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySLAE
{
    public class EquationSystem
    {
        private List<Equation> equations = new List<Equation>();
        public List<Equation> Equations { get { return equations; } set { equations = value; OnDischar(equations); } }

        public EquationSystem(List<Equation> equations)
        {
            Equations = equations;
        }

        public EquationSystem(int countEq)
        {
            for (int i = 0; i < countEq; i++)
            {
                equations.Add(new Equation(new double[countEq], 0));
            }
            Equations = equations;
        }
        public delegate void СhangingNumberEventHandler(List<Equation> system);
        public event СhangingNumberEventHandler СhangingNumber;
        private void OnDischar(List<Equation> system)
        {
            СhangingNumber?.Invoke(system);
        }

        private double[,] FindAlpha(double e, double[,] am)
        {
            double[,] em = new double[am.GetLength(0), am.GetLength(1)];
            for (int i = 0; i < am.GetLength(0); i++)
            {
                for (int j = 0; j < am.GetLength(1); j++)
                {
                    em[i, j] = e * Math.Pow(10, -9);
                }
            }
            return Matrix.MatrixMultiplication(em, am);
        }

        private double[] FindBetta(double e, double[,] am, double[] bm)
        {
            double[,] em = new double[am.GetLength(0), am.GetLength(1)];
            for (int i = 0; i < am.GetLength(0); i++)
            {
                for (int j = 0; j < am.GetLength(1); j++)
                {
                    em[i, j] = e * Math.Pow(10, -5); ;
                }
            }
            return Matrix.MatrixAMultiplicationByMatrixB(Matrix.Difference(Matrix.InverseMatrix(am), em), bm);
        }

        public double[] ZeidelIterations(double e, ref List<IterationData> data)
        {
            double[] b = Matrix.СreateMatrixB(this);
            double[,] a = Matrix.СreateMatrixA(this);
            if (Matrix.FindDet(a) != 0)
            {
                double[,] alpha = FindAlpha(e, a);
                double[] betta = FindBetta(e, a, b);
                List<double[]> results = new List<double[]>();
                results.Add(new double[b.Length]);
                data.Add(new IterationData(results[0], 0, 0));
                double norm = 0;
                int k = 0;
                Stopwatch sw;
                do
                {
                    sw = Stopwatch.StartNew();
                    double[] currentX = new double[b.Length];
                    for (int i = 0; i < alpha.GetLength(0); i++)
                    {
                        double sumA = 0;
                        for (int j = 0; j < alpha.GetLength(1); j++)
                        {
                            double x;
                            if (j >= i || k == 0)
                            {
                                x = results[k][j];
                            }
                            else
                            {
                                x = currentX[j];
                            }
                            sumA += alpha[i, j] * x;
                        }
                        currentX[i] = sumA + betta[i];
                    }
                    k++;
                    norm = Math.Abs(Matrix.FindNorm(Matrix.Difference(currentX, results[k - 1])));
                    sw.Stop();
                    data.Add(new IterationData(currentX, sw.Elapsed.TotalMilliseconds, norm));
                    results.Add(currentX);
                }
                while (norm > e);
                if (norm.Equals(double.NaN)) return null;
                else return results[results.Count - 1];
            }
            return null;
        }

        public double[] MathodJacobi(double e, ref List<IterationData> data)
        {
            double[] b = Matrix.СreateMatrixB(this);
            double[,] a = Matrix.СreateMatrixA(this);
            if (Matrix.FindDet(a) != 0)
            {
                double[,] alpha = FindAlpha(e, a);
                double[] betta = FindBetta(e, a, b);
                List<double[]> results = new List<double[]>();
                results.Add(new double[b.Length]);
                data.Add(new IterationData(results[0], 0, 0));
                double norm = 0;
                int k = 0;
                Stopwatch sw;
                do
                {
                    sw = Stopwatch.StartNew();
                    double[] currentX = new double[b.Length];
                    for (int i = 0; i < alpha.GetLength(0); i++)
                    {
                        double sumA = 0;
                        for (int j = 0; j < alpha.GetLength(1); j++)
                        {
                            sumA += alpha[i, j] * results[k][j];
                        }
                        currentX[i] = sumA + betta[i];
                    }
                    k++;
                    norm = Math.Abs(Matrix.FindNorm(Matrix.Difference(currentX, results[k - 1])));
                    sw.Stop();
                    data.Add(new IterationData(currentX, sw.Elapsed.TotalMilliseconds, norm));
                    results.Add(currentX);
                }
                while (norm > e);
                if (norm.Equals(double.NaN)) return null;
                else return results[results.Count - 1];
            }
            return null;
        }
        public double[] RelaxationMethod(double e, ref List<IterationData> data, double param)
        {
            double[] b = Matrix.СreateMatrixB(this);
            double[,] a = Matrix.СreateMatrixA(this);
            if (Matrix.FindDet(a) != 0)
            {
                double[,] alpha = FindAlpha(e, a);
                double[] betta = FindBetta(e, a, b);
                List<double[]> results = new List<double[]>();
                results.Add(new double[b.Length]);
                data.Add(new IterationData(results[0], 0, 0));
                double norm = 0;
                int k = 0;
                Stopwatch sw;
                do
                {
                    sw = Stopwatch.StartNew();
                    double[] currentX = new double[b.Length];
                    for (int i = 0; i < alpha.GetLength(0); i++)
                    {
                        double sumA = (1 - param) * results[k][i];
                        for (int j = 0; j < alpha.GetLength(1); j++)
                        {
                            double x;
                            if (j >= i || k == 0)
                            {
                                x = results[k][j];
                            }
                            else
                            {
                                x = currentX[j];
                            }
                            sumA += alpha[i, j] * x * param;
                        }
                        currentX[i] = sumA + betta[i] * param;
                    }
                    k++;
                    norm = Math.Abs(Matrix.FindNorm(Matrix.Difference(currentX, results[k - 1])));
                    sw.Stop();
                    data.Add(new IterationData(currentX, sw.Elapsed.TotalMilliseconds, norm));
                    results.Add(currentX);
                }
                while (norm > e);
                if (norm.Equals(double.NaN)) return null;
                else return results[results.Count - 1];
            }
            return null;
        }
    }
}
