using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;

namespace Platformer
{
    public class Tile : RectangleShape
    {
        public Tile(Vector2f position, Vector2f size, TileType type) : base(size)
        {
            Position = position;
            Texture = Textures.TileSet;
            TextureRect = TileHelper.Get_Texture_Clip(type);
        }

        public void Draw(RenderWindow window)
        {
            window.Draw(this);
        }
    }
}
