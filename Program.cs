namespace Engine2D
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new Application();

            app.Input.AddAction("test", (int)SDL2.SDL.SDL_Keycode.SDLK_SPACE);

            app.Run();
        }
    }
}
