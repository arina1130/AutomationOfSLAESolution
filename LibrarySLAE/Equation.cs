using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySLAE
{
    public class Equation
    {
        public double[] a { get; private set; }
        public double b { get; private set; }
        public double[] x { get; private set; }
        public Equation(double[] a, double b)
        {
            this.a = a;
            this.b = b;
            x = new double[a.Length];
        }
    }
}
