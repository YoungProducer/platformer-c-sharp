using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public class RangeConverter
    {
        readonly float inputStart;
        readonly float inputEnd;
        readonly float outputStart;
        readonly float outputEnd;

        public RangeConverter(float inputStart, float inputEnd, float outputStart, float outputEnd)
        {
            this.inputStart = inputStart;
            this.inputEnd = inputEnd;
            this.outputStart = outputStart;
            this.outputEnd = outputEnd;
        }

        public double Map(double value)
        {
            double slope = 1.0 * (outputEnd - outputStart) / (inputEnd - inputStart);
            double output = outputStart + slope * (value - inputStart);

            return Math.Round(output);
        }
    }
}
