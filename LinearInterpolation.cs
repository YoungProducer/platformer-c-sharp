using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Platformer
{
    public static class LinearInterpolation
    {
        public static List<Vector2f> InterpolateRange(Vector2f p1, Vector2f p2)
        {
            int range = (int)p2.X - (int)p1.X;

            List<Vector2f> result = new();

            for (int i = 1; i < range; i++)
            {
                result.Add(new Vector2f(p1.X + i, InterpolateValue(p1.X + i, p1, p2)));
            }

            return result;
        }

        public static float InterpolateValue(float x, Vector2f p1, Vector2f p2)
        {
            if ((p2.X - p2.Y) == 0)
            {
                return (p1.Y + p2.Y) / 2;
            }
            return p1.Y + (x - p1.X) * (p2.Y - p1.Y) / (p2.X - p1.X);
        }
    }
}
