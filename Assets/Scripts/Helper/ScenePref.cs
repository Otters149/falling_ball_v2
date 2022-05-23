using System.Collections.Generic;

/// <summary>
/// This class manage which scene will be next after run loading scene
/// And share bundle between scenes
/// </summary>
namespace fallingball
{
    namespace helper
    {
        public static class ScenePref
        {
            private class Bundle
            {
                Dictionary<string, object> _data = new Dictionary<string, object>();

                public void AddBundle<T>(string key, T value)
                {
                    _data.Add(key, value);
                }

                public bool HasKey(string key)
                {
                    return _data.ContainsKey(key);
                }

                public object GetBundle(string key)
                {
                    object outValue = null;
                    return _data.TryGetValue(key, out outValue);
                }
            }

            static private Stack<SceneName> _sceneStack = new Stack<SceneName>();
            static Bundle _bundle = new Bundle();

            static public bool IsNull() => _sceneStack.Count == 0;

            static public void Push(SceneName value)
            {
                _sceneStack.Push(value);
            }

            static public SceneName Pop()
            {
                if (IsNull())
                {
                    return SceneName.None;
                }
                return _sceneStack.Pop();
            }

            static public SceneName Peek()
            {
                if (IsNull())
                {
                    return SceneName.None;
                }
                return _sceneStack.Peek();
            }

            static object GetScenePref(string key)
            {
                return _bundle.GetBundle(key);
            }
            static void AddScenePref<T>(string key, T value)
            {
                _bundle.AddBundle(key, value);
            }

            static bool ScenePrefHas(string key)
            {
                return _bundle.HasKey(key);
            }
        }
    }
}
