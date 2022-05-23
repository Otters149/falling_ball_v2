using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utilpackages.scenehelper;
using utilpackages.qlog;

namespace core
{
    public class NAScene : MonoBehaviour
    {
        //protected QLog _logger = QLog.GetInstance();

#if _RETAIL
        protected static bool _hasLaunchFromBegin = false;
#else
        protected static bool _hasLaunchFromBegin = true;
#endif
        virtual protected void Start()
        {
            if (!_hasLaunchFromBegin)
            {
                SceneHelper.Quit();
            }
        }
    }
}
