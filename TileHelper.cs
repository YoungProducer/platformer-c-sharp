using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    public static class TileHelper
    {
        private static Random random = new();

        private static Vector2f Get_Random_Dirt_Texture_Offset()
        {
            int x = random.Next(0, 14);
            int y = random.Next(0, 12);

            if (x > 2 || y > 1) return new Vector2f(8, 8);

            Vector2f dirtSize = Get_Texture_Size(TileType.Dirt);

            return new Vector2f(x * dirtSize.X, y * dirtSize.Y + 40);
        }

        private static Vector2f Get_Random_Details_Offset(TileType type)
        {
            Vector2f beginPosition = type.textureOffset;

            int x = random.Next(0, 5);

            if (x > 2) return Get_Texture_Offset(TileType.None);

            return beginPosition + new Vector2f(x * type.textureSize.X, 0);
        }

        public static Vector2f Get_Texture_Offset(TileType type)
        {
            if (type == TileType.Dirt) return Get_Random_Dirt_Texture_Offset();
            if (type == TileType.GrassBottomDetails ||
                type == TileType.GrassTopDetails ||
                type == TileType.Flowers
                ) return Get_Random_Details_Offset(type);

            return type.textureOffset;
        }

        public static Vector2f Get_Texture_Size(TileType type)
        {
            return type.textureSize;
        }

        public static IntRect Get_Texture_Clip(TileType type)
        {
            Vector2f size = Get_Texture_Size(type);
            Vector2f offset = Get_Texture_Offset(type);

            return new IntRect((int)offset.X, (int)offset.Y, (int)size.X, (int)size.Y);
        }
    }
}
