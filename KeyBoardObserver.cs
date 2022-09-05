using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;

namespace Platformer
{
    public static class KeyBoardObserver
    {
        static readonly bool[] pressedKeys = new bool[(int)Keyboard.Key.KeyCount];

        static public void Init()
        {
            EventsHandler.Add_On_Key_Pressed(Key_Pressed_Event);
            EventsHandler.Add_On_Key_Released(Key_Released_Event);
        }

        static private void Key_Pressed_Event(object? sender, KeyEventArgs e)
        {
            pressedKeys[(int)e.Code] = true;
        }

        static private void Key_Released_Event(object? sender, KeyEventArgs e)
        {
            pressedKeys[(int)e.Code] = false;
        }

        static public bool Is_Key_Pressed(Keyboard.Key key)
        {
            return pressedKeys[(int)key];
        }
    }
}
