using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace utilpackages
{
    namespace network
    {
        public class NetworkManager : MonoBehaviour
        {
            public void OnInternetConnected(Action onConnected, Action onFailed, float timeout = 3f)
            {
                StartCoroutine(CheckInternetConnection(isConnected =>
                {
                    if(isConnected)
                    {
                        onConnected.Invoke();
                    }
                    else
                    {
                        onFailed.Invoke();
                    }
                }, timeout));
            }

            private IEnumerator CheckInternetConnection(Action<bool> action, float timeout)
            {
                UnityWebRequest request = new UnityWebRequest("http://google.com");
                float deadline = Time.time + timeout;
                while(Time.time < deadline)
                {
                    yield return request.SendWebRequest();
                    if (request.error != null)
                    {
                        qlog.QLog.GetInstance().LogError(this.GetType().FullName, request.error.ToString());
                    }
                    else
                    {
                        action(true);
                        yield break;
                    }
                }
                if(request.error != null)
                {
                    action(false);
                }
            }
        }
    }
}
