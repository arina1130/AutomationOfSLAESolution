using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySLAE
{
    public class IterationData
    {
        public double[] Results { get; private set; }
        public double Speed { get; private set; }
        public double Accuracy { get; private set; }
        public IterationData(double[] results, double speed, double accuracy)
        {
            Results = results;
            Speed = speed;
            Accuracy = accuracy;
        }
    }
}
