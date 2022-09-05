using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Drawing;

namespace Platformer
{
    public class Camera : CustomView
    {
        float velocity = 40;
        Vector2f moveVector = new(0, 0);
        Vector2f horizontalLimits = new(0, 900);
        Vector2f verticalLimits = new(-1, -1);

        private static readonly Dictionary<Keyboard.Key, int> moveValues = new()
        {
            { Keyboard.Key.W, -1 },
            { Keyboard.Key.S, 1 },
            { Keyboard.Key.A, -1 },
            { Keyboard.Key.D, 1 }
        };

        public Camera(RenderWindow window) : base(window)
        {
            Center = WindowResizeObserver.MiddlePoint;
            Size = WindowResizeObserver.Size;

            verticalLimits = new(-1, WindowResizeObserver.Size.Y);

            EventsHandler.Add_On_Resize_Event(Window_Resize_Event);
        }

        private void Move(int elapsedTime)
        {
            Center = Get_New_Center(elapsedTime);
        }

        private Vector2f Get_New_Center(int elapsedTime)
        {
            Vector2f newCenter = Calculate_New_Center(elapsedTime);

            Vector2f correctionVector = new(Is_In_Horizontal_Limits(newCenter) ? 1 : 0, Is_In_Vertical_Limits(newCenter) ? 1 : 0);

            moveVector = VectorsHelpers.Multiply_Values_2f(moveVector, correctionVector);

            newCenter = Calculate_New_Center(elapsedTime);

            return newCenter;
        }

        private Vector2f Calculate_New_Center(int elapsedTime)
        {
            Vector2f centerOffset = moveVector * velocity * (elapsedTime / 10);
            Vector2f newCenter = Center + centerOffset;

            return newCenter;
        }

        private bool Is_In_Vertical_Limits(Vector2f newCenter)
        {
            if (verticalLimits.X != -1 && newCenter.Y - Size.Y / 2 < verticalLimits.X) return false;
            if (verticalLimits.Y != -1 && newCenter.Y + Size.Y / 2 > verticalLimits.Y) return false;

            return true;
        }

        private bool Is_In_Horizontal_Limits(Vector2f newCenter)
        {
            if (horizontalLimits.X != -1 && newCenter.X - Size.X / 2 < horizontalLimits.X) return false;
            if (horizontalLimits.Y != -1 && newCenter.X + Size.X / 2 > horizontalLimits.Y) return false;

            return true;
        }

        private void Update_Move_Vector()
        {
            int left = KeyBoardObserver.Is_Key_Pressed(Keyboard.Key.A) ? moveValues.TryGetValue(Keyboard.Key.A, out int l) ? l : 0 : 0;
            int right = KeyBoardObserver.Is_Key_Pressed(Keyboard.Key.D) ? moveValues.TryGetValue(Keyboard.Key.D, out int r) ? r : 0 : 0;
            int top = KeyBoardObserver.Is_Key_Pressed(Keyboard.Key.W) ? moveValues.TryGetValue(Keyboard.Key.W, out int t) ? t : 0 : 0;
            int bottom = KeyBoardObserver.Is_Key_Pressed(Keyboard.Key.S) ? moveValues.TryGetValue(Keyboard.Key.S, out int b) ? b : 0 : 0;

            moveVector = new(left + right, top + bottom);
        }

        protected override void Update_Viewport(Vector2f size)
        {
            Size = size;

            Vector2f newCenter = new(Center.X, size.Y / 2);

            if (!Is_In_Horizontal_Limits(newCenter))
            {
                if (newCenter.X < horizontalLimits.X + size.X / 2) newCenter.X = horizontalLimits.X + size.X / 2;
                if (newCenter.X > horizontalLimits.Y - size.X / 2) newCenter.X = horizontalLimits.Y - size.X / 2;
            }

            Center = newCenter;
        }

        private void Window_Resize_Event(object? sender, SizeEventArgs e)
        {
            verticalLimits = new(verticalLimits.X, WindowResizeObserver.Size.Y);
        }

        public void Use()
        {
            Before_Draw();
        }

        public void Update(int elapsedTime)
        {
            Update_Move_Vector();
            Move(elapsedTime);
        }

        public void Look_At(Vector2f center)
        {
            Center = center;
        }

        public Vector2f Horizontal_Limits
        {
            set => horizontalLimits = value;
        }

        public Vector2f Vertical_Limits
        {
            set => verticalLimits = value;
        }

        public Vector2f Camera_Look_At
        {
            get => Center;
        }
    }
}
