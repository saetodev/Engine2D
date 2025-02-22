using OpenGL;
using System.Diagnostics;
using static SDL2.SDL;

namespace Engine2D
{
    internal class Application
    {
        public Input Input = new Input();

        private IntPtr m_window;
        private IntPtr m_context;

        private bool m_running;

        public void Run()
        {
            Init();

            while (m_running)
            {
                Input.ResetState();

                SDL_Event @event;

                while (SDL_PollEvent(out @event) != 0)
                {
                    if (@event.type == SDL_EventType.SDL_QUIT)
                    {
                        m_running = false;
                    }

                    if (@event.type == SDL_EventType.SDL_KEYUP || @event.type == SDL_EventType.SDL_KEYDOWN)
                    {
                        int key = (int)@event.key.keysym.sym;

                        bool isDown  = (@event.key.state == SDL_PRESSED);
                        bool wasDown = (@event.key.state == SDL_RELEASED) || (@event.key.repeat != 0);
                        
                        Input.SetAction(key, isDown, wasDown);
                    }
                }

                Renderer2D.Clear();
                Renderer2D.Render();

                SDL_GL_SwapWindow(m_window);
            }

            Shutdown();
        }

        private void Init()
        {
            if (SDL_Init(SDL_INIT_VIDEO) != 0)
            {
                Console.WriteLine($"ERROR: {SDL_GetError()}");
                Environment.Exit(-1);
            }

            m_window = SDL_CreateWindow("Engine2D", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 1280, 720, SDL_WindowFlags.SDL_WINDOW_OPENGL);

            if (m_window == IntPtr.Zero)
            {
                Console.WriteLine($"ERROR: {SDL_GetError()}");
                Environment.Exit(-1);
            }

            m_context = SDL_GL_CreateContext(m_window);

            if (m_context == IntPtr.Zero)
            {
                Console.WriteLine($"ERROR: {SDL_GetError()}");
                Environment.Exit(-1);
            }

            SDL_GL_MakeCurrent(m_window, m_context);

            GL.Import(SDL_GL_GetProcAddress);
            Renderer2D.Init();

            m_running = true;
        }

        private void Shutdown()
        {
            Debug.Assert(m_window != IntPtr.Zero);
            Debug.Assert(m_context != IntPtr.Zero);

            SDL_GL_DeleteContext(m_context);
            SDL_DestroyWindow(m_window);

            SDL_Quit();
        }
    }
}
