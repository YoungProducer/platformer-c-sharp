using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Platformer
{
    public static class Textures
    {
        public static readonly Texture TileSet = new(Get_Texture_Path("..\\..\\..\\assets\\tileset.png"));

        private static string Get_Texture_Path (string relativePath)
        {
            string currentDirectoryPath = Directory.GetCurrentDirectory();

            return Path.Combine(currentDirectoryPath, relativePath);
        }
    }
}
