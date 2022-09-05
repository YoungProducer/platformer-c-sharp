using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Platformer
{
    public static class VectorsHelpers
    {
        public static Vector2f Multiply_Values_2f(Vector2f left, Vector2f right)
        {
            return new(left.X * right.X, left.Y * right.Y);
        }
    }
}
