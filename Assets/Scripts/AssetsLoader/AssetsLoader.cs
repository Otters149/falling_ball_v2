using UnityEngine;

namespace fallingball
{
    namespace assetsloader
    {
        static public class AssetsLoader
        {
            static public T GetResource<T>(string path) where T : Object
            {
                return Resources.Load<T>(path);
            }
        }
    }
}
