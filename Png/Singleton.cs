using System.Collections;

namespace EmediaWPF
{
    public class Singleton<T> where T : class, new()
    {
        private static T m_instance;

        public static T Instance => GetInstance();

        public static T GetInstance()
        {
            if (m_instance == null)
                m_instance = new T();
            return m_instance;
        }

        public static bool IsInitialized() => m_instance != null;

        public IEnumerator WaitUntilInitialized()
        {
            while (Instance == null)
                yield return null;
        }
    }
}
