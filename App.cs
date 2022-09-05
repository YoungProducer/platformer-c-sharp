using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using SFML.Graphics;
using SFML.Window;

namespace Platformer
{
    public class App
    {
        public RenderWindow Window { get; }
        ContextSettings Settings { get; set; }
        uint Height { get; }
        uint Width { get; }
        public string Title { get; set; }

        Color clearColor = Color.Black;

        Clock clock;
        int elapsedTime;

        UI appUi;
        Game game;

        public App(uint width, uint height, string title)
        {
            Height = height;
            Width = width;
            Title = title;

            ContextSettings settings = new()
            {
                AntialiasingLevel = 8
            };

            Settings = settings;

            Window = Create_Window();

            clock = new();

            EventsHandler.Init(Window);

            KeyBoardObserver.Init();
            WindowResizeObserver.Init(new Vector2f(width, height));

            appUi = new(Window);
            game = new(Window);
        }

        private RenderWindow Create_Window()
        {
            RenderWindow window = new RenderWindow(new VideoMode(Width, Height), Title, Styles.Default, Settings);
            window.SetVerticalSyncEnabled(true);

            return window;
        }

        public void Setup_Events()
        {
            EventsHandler.Add_On_Close_Event(On_Close_Event);
        }

        private void On_Close_Event(object? sender, EventArgs e)
        {
            Window.Close();
        }

        public void Before_Draw()
        {
            elapsedTime = clock.Restart().AsMilliseconds();

            Window.DispatchEvents();

            Window.Clear(clearColor);
        }

        public void Draw()
        {
            appUi.Draw();
            game.Draw(elapsedTime);
        }

        public void After_Draw()
        {
            Window.Display();
        }

        public bool Is_Open()
        {
            return Window.IsOpen;
        }

        public Color ClearColor
        {
            get => clearColor;
            set => clearColor = value;
        }
    }
}
