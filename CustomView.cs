using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Platformer
{
    public class CustomView : View
    {
        RenderWindow window;

        public CustomView(RenderWindow window)
            : base(new FloatRect(0, 0, window.Size.X, window.Size.Y))
        {
            this.window = window;

            EventsHandler.Add_On_Resize_Event(Window_Resize_Event);
        }

        private void Window_Resize_Event(object? sender, SizeEventArgs e)
        {
            Update_Viewport(new Vector2f(e.Width, e.Height));
        }

        virtual protected void Update_Viewport(Vector2f size)
        {
            FloatRect rect = new FloatRect(0, 0, size.X, size.Y);

            Reset(rect);
        }

        public void Before_Draw()
        {
            window.SetView(this);
        }
    }
}
