using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace Platformer
{
    public static class EventsHandler
    {
        private static RenderWindow? window;

        public static void Init(RenderWindow Window)
        {
            window = Window;
        }

        public static void Add_On_Close_Event(EventHandler evt)
        {
            if (window == null) return;

            window.Closed += evt;
        }

        public static void Add_On_Resize_Event(EventHandler<SizeEventArgs> evt)
        {
            if (window == null) return;

            window.Resized += evt;
        }

        public static void Add_On_Key_Pressed(EventHandler<KeyEventArgs> evt)
        {
            if (window == null) return;

            window.KeyPressed += evt;
        }

        public static void Add_On_Key_Released(EventHandler<KeyEventArgs> evt)
        {
            if (window == null) return;

            window.KeyReleased += evt;
        }
    }
}
