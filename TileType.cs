using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Platformer
{
    public sealed class TileType
    {
        private readonly string name;
        private readonly int value;
        public readonly Vector2f textureOffset;
        public readonly Vector2f textureSize;

        public static readonly Dictionary<string, TileType> instance = new Dictionary<string, TileType>();

        public static readonly TileType GrassTop                        = new TileType(1, "grass-top", new Vector2f(8, 0), new Vector2f(8, 8));
        public static readonly TileType GrassTopLeftOutterCorner        = new TileType(2, "grass-top-left-outter-corner", new Vector2f(0, 0), new Vector2f(8, 8));
        public static readonly TileType GrassTopRightOutterCorner       = new TileType(3, "grass-top-right-outter-corner", new Vector2f(16, 0), new Vector2f(8, 8));
        public static readonly TileType GrassBottomLeftInnerCorner      = new TileType(4, "grass-bottom-left-inner-corner", new Vector2f(0, 32), new Vector2f(8, 8));
        public static readonly TileType GrassBottomRightInnerCorner     = new TileType(5, "grass-bottom-right-inner-corner", new Vector2f(8, 32), new Vector2f(8, 8));
        public static readonly TileType GrassBottom                     = new TileType(6, "grass-bottom", new Vector2f(8, 16), new Vector2f(8, 8));
        public static readonly TileType GrassLeft                       = new TileType(7, "grass-left", new Vector2f(0, 8), new Vector2f(8, 8));
        public static readonly TileType GrassRight                      = new TileType(8, "grass-right", new Vector2f(16, 8), new Vector2f(8, 8));
        public static readonly TileType Dirt                            = new TileType(9, "dirt", new Vector2f(0, 0), new Vector2f(8, 8));
        public static readonly TileType None                            = new TileType(10, "none", new Vector2f(24, 24), new Vector2f(8, 8));
        public static readonly TileType TreeRoot                        = new TileType(11, "tree-root", new Vector2f(64, 48), new Vector2f(8, 8));
        public static readonly TileType Tree                            = new TileType(12, "tree", new Vector2f(56, 40), new Vector2f(8, 16));
        public static readonly TileType TreeLeaf                        = new TileType(13, "tree-leaf", new Vector2f(32, 32), new Vector2f(24, 24));
        public static readonly TileType BackgroundHills                 = new TileType(14, "background-hills", new Vector2f(104, 24), new Vector2f(64, 32));
        public static readonly TileType Flowers                         = new TileType(15, "flowers", new Vector2f(64, 0), new Vector2f(8, 8));
        public static readonly TileType GrassTopDetails                 = new TileType(16, "grass-top-details", new Vector2f(32, 0), new Vector2f(8, 8));
        public static readonly TileType GrassBottomDetails              = new TileType(17, "grass-bottom-details", new Vector2f(32, 16), new Vector2f(8, 8));
        public static readonly TileType Ladder                          = new TileType(18, "ladder", new Vector2f(64, 24), new Vector2f(8, 8));


        private TileType(int value, string name, Vector2f textureOffset, Vector2f textureSize)
        {
            this.name = name;
            this.value = value;
            this.textureOffset = textureOffset;
            this.textureSize = textureSize;

            instance.Add(name, this);
        }

        public static explicit operator TileType(string str)
        {
            TileType result;
            if (instance.TryGetValue(str, out result))
                return result;
            else
                throw new InvalidCastException();
        }

        public override String ToString()
        {
            return name;
        }
    }
}
