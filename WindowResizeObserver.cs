using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Window;

namespace Platformer
{
    public static class WindowResizeObserver
    {
        private static Vector2f size = new(800, 600);
        private static Vector2f initialSize = new(800, 600);
        private static Vector2f middlePoint = new(400, 300);
        private static float scale = 1;

        public static void Init(Vector2f InitialSize)
        {
            initialSize = InitialSize;
            size = InitialSize;
            middlePoint = InitialSize / 2;

            EventsHandler.Add_On_Resize_Event(Window_Resize_Event);
        }

        private static void Window_Resize_Event(object? sender, SizeEventArgs e)
        {
            Vector2f newSize = new(e.Width, e.Height);

            Update_Size_And_Middle_Point(newSize);
            Update_Scale(newSize);
        }

        private static void Update_Size_And_Middle_Point(Vector2f newSize)
        {
            size = newSize;
            middlePoint = newSize / 2;
        }

        private static void Update_Scale(Vector2f newSize)
        {
            float scaleX = newSize.X / initialSize.X;
            float scaleY = newSize.Y / initialSize.Y;

            scale = Math.Max(scaleX, scaleY);
        }

        public static Vector2f Size
        {
            get => size;
        }

        public static float Scale
        {
            get => scale;
        }

        public static Vector2f MiddlePoint
        {
            get => middlePoint;
        }
    }
}
