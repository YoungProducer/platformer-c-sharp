using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public class Game
    {
        Camera camera;
        RenderWindow window;

        ProceduralLevel level;
        int levelScaleFactor = 4;

        public Game(RenderWindow window)
        {
            camera = new(window);
            this.window = window;

            level = new(levelScaleFactor);

            //level.Load_Data_From_JSON("levels\\level1.json");

            level.Generate_Tile_Data();

            camera.Horizontal_Limits = new Vector2f(0, level.Get_Level_Size_Px().X);
            camera.Vertical_Limits = new Vector2f(-(level.Get_Level_Size_Px().Y / 2), WindowResizeObserver.Size.Y);
            camera.Look_At(new Vector2f(WindowResizeObserver.Size.X / 2, WindowResizeObserver.Size.Y / 2));

            level.Generate_Level();
        }

        public void Draw(int elapsedTime)
        {
            camera.Use();
            camera.Update(elapsedTime);
            level.Draw_Background(window);
            level.Draw(window);
        }
    }
}
