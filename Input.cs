using System.Diagnostics;

namespace Engine2D
{
    struct InputState
    {
        public bool Down;
        public bool Pressed;
        public bool Released;

        public InputState()
        {
            Down = false;
            Pressed = false;
            Released = false;
        }
    }

    internal class Input
    {
        private Dictionary<string, int> m_actionMap = new Dictionary<string, int>();
        private Dictionary<int, InputState> m_stateMap = new Dictionary<int, InputState>();

        public void AddAction(string name, int key)
        {
            Debug.Assert(!m_actionMap.ContainsKey(name));
            Debug.Assert(!m_stateMap.ContainsKey(key));

            m_actionMap.Add(name, key);
            m_stateMap.Add(key, new InputState());
        }

        public void ResetState()
        {
            foreach (int key in m_stateMap.Keys)
            {
                InputState state = m_stateMap[key];

                state.Pressed = false;
                state.Released = false;

                m_stateMap[key] = state;
            }
        }

        public void SetAction(int key, bool isDown, bool wasDown)
        {
            if (!m_stateMap.ContainsKey(key))
            {
                return;
            }

            InputState state = new InputState
            {
                Down = isDown,
                Pressed = isDown && !wasDown,
                Released = !isDown && wasDown,
            };

            m_stateMap[key] = state;
        }

        public bool ActionDown(string name)
        {
            if (!m_actionMap.ContainsKey(name))
            {
                return false;
            }

            int key = m_actionMap[name];

            Debug.Assert(m_stateMap.ContainsKey(key));

            return m_stateMap[key].Down;
        }

        public bool ActionPressed(string name)
        {
            if (!m_actionMap.ContainsKey(name))
            {
                return false;
            }

            int key = m_actionMap[name];

            Debug.Assert(m_stateMap.ContainsKey(key));

            return m_stateMap[key].Pressed;
        }

        public bool ActionReleased(string name)
        {
            if (!m_actionMap.ContainsKey(name))
            {
                return false;
            }

            int key = m_actionMap[name];

            Debug.Assert(m_stateMap.ContainsKey(key));

            return m_stateMap[key].Released;
        }
    }
}
