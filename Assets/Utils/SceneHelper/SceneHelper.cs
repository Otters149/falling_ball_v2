using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace utilpackages
{
    namespace scenehelper
    {
        static public class SceneHelper
        {
            static public void Quit()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
