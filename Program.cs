namespace Platformer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            App app = new(1200, 600, "Platformer");

            app.Setup_Events();

            while(app.Is_Open())
            {
                app.Before_Draw();
                app.Draw();
                app.After_Draw();
            }
        }
    }
}