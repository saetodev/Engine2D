using GlmSharp;
using System.Globalization;
using static OpenGL.GL;

namespace Engine2D
{
    struct Vertex
    {
        public vec2 Position;
        public vec4 Color;
    }

    internal unsafe static class Renderer2D
    {
        private static uint m_vao;
        private static uint m_vbo;
        private static uint m_shader;

        private static mat4 m_projection;

        public static void Init()
        {
            m_vao = glGenVertexArray();
            glBindVertexArray(m_vao);

            m_vbo = glGenBuffer();
            glBindBuffer(GL_ARRAY_BUFFER, m_vbo);

            float[] vertices =
            {
                -0.5f, -0.5f,
                 0.5f, -0.5f,
                 0.5f,  0.5f,

                 0.5f,  0.5f,
                -0.5f,  0.5f,
                -0.5f, -0.5f,
            };

            fixed (void* ptr = vertices)
            {
                glBufferData(GL_ARRAY_BUFFER, vertices.Length * sizeof(float), ptr, GL_STATIC_DRAW);
            }

            glEnableVertexAttribArray(0);
            glVertexAttribPointer(0, 2, GL_FLOAT, false, 0, 0);

            m_shader = LoadShader("vertex.glsl", "fragment.glsl");

            m_projection = mat4.Ortho(0f, 1280f, 720f, 0f, 0f, 1f);
        }

        public static void Clear()
        {
            glClear(GL_COLOR_BUFFER_BIT);
        }

        public static void Render()
        {
            glUseProgram(m_shader);

            {
                int loc = glGetUniformLocation(m_shader, "u_projection");
                glUniformMatrix4fv(loc, 1, false, m_projection.ToArray());
            }

            {
                mat4 transform = mat4.Translate(new vec3(640f, 320f, 0f)) * mat4.Scale(new vec3(48f, 48f, 1f));

                int loc = glGetUniformLocation(m_shader, "u_transform");
                glUniformMatrix4fv(loc, 1, false, transform.ToArray());
            }

            glDrawArrays(GL_TRIANGLES, 0, 6);
        }

        private static uint LoadShader(string vertexFilename, string fragmentFilename)
        {
            string vertexSource = File.ReadAllText(vertexFilename);
            string fragmentSource = File.ReadAllText(fragmentFilename);

            uint program = glCreateProgram();

            uint vertexShader = glCreateShader(GL_VERTEX_SHADER);
            glShaderSource(vertexShader, vertexSource);
            glCompileShader(vertexShader);

            {
                int result;
                glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &result);

                if (result == GL_FALSE)
                {
                    string info = glGetShaderInfoLog(vertexShader);
                    Console.WriteLine($"ERROR: Failed to compile vertex shader: {info}");
                }
            }

            uint fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
            glShaderSource(fragmentShader, fragmentSource);
            glCompileShader(fragmentShader);

            {
                int result;
                glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &result);

                if (result == GL_FALSE)
                {
                    string info = glGetShaderInfoLog(fragmentShader);
                    Console.WriteLine($"ERROR: Failed to compile fragment shader: {info}");
                }
            }

            glAttachShader(program, vertexShader);
            glAttachShader(program, fragmentShader);
            glLinkProgram(program);

            glDeleteShader(fragmentShader);
            glDeleteShader(vertexShader);

            return program;
        }
    }
}
