using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Platformer
{
    public class UI
    {
        CustomView view;
        RenderWindow window;

        public UI(RenderWindow window)
        {
            view = new CustomView(window);
            this.window = window;
        }

        public void Draw()
        {
            view.Before_Draw();
        }
    }
}
