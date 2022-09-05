using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public class NoiseGenerator
    {
        private uint _seed;

        public NoiseGenerator(int seed)
        {
            _seed = (uint)seed;
        }

        public double Next()
        {
            _seed += 0x6D2B79F5;
            var t = (_seed ^ (_seed >> 15)) * (1 | _seed);
            t = (t + ((t ^ (t >> 7)) * (61 | t))) ^ t;
            return (t ^ (t >> 14)) / (double)4294967296;
        }
    }
}
