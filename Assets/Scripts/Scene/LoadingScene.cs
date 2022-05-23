using fallingball.helper;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using utilpackages.qlog;

namespace fallingball
{
    namespace scene
    {
        public class LoadingScene : MonoBehaviour
        {
            private AsyncOperation _loadSceneAsyncOperation;
            private QLog _logger;

            [SerializeField]
            private Slider _loadingBar;

            private TextMeshProUGUI _textValue;

            [SerializeField]
            [Range(0, 1)]
            private float _smoothness = 0.5f;

            private float _currentPercent;
            private float _loadedPercent;
            private bool _isLoadingCompleted = false;
            void Awake()
            {
                _logger = QLog.GetInstance();
            }

            private void Start()
            {
                if (_loadingBar == null)
                {
                    _logger.LogError(_logger.GetClassName(this), "Loading bar not found!");
#if _RETAIL
                    SceneHelper.Quit();
#endif
                    return;
                }
                _textValue = _loadingBar.transform.Find("Text_Value").GetComponent<TextMeshProUGUI>();
                _currentPercent = _loadedPercent = 0f;
                _loadingBar.value = 0f;


                SceneName nextSceneToLoad = ScenePref.Pop();
                if (nextSceneToLoad == SceneName.None)
                {
                    _logger.LogError(_logger.GetClassName(this), "Next scene was null, can not laod!");
#if _RETAIL
                    SceneHelper.Quit();
#endif
                    return;
                }

                _loadSceneAsyncOperation = SceneManager.LoadSceneAsync(nextSceneToLoad.GetName());
                _loadSceneAsyncOperation.allowSceneActivation = false;
                _loadSceneAsyncOperation.completed += (result) => _logger.LogInfo(_logger.GetClassName(this), string.Format("Load {0} Scene Success!", nextSceneToLoad.GetName()));
            }

            private IEnumerator OnLoadingCompleted()
            {
                yield return new WaitForSeconds(0.5f);
                _loadSceneAsyncOperation.allowSceneActivation = true;
            }

            private void Update()
            {
                if (!_isLoadingCompleted)
                {
                    _loadedPercent = _loadSceneAsyncOperation.progress / 0.9f;
                    _currentPercent = Mathf.MoveTowards(_currentPercent, _loadedPercent, _smoothness * Time.deltaTime);
                    _loadingBar.value = _currentPercent;
                    _textValue?.SetText("Loading..." + string.Format("{0}%", Mathf.Ceil(_currentPercent * 100)));

                    if (Mathf.Approximately(_currentPercent, 1f))
                    {
                        _isLoadingCompleted = true;
                        StartCoroutine(OnLoadingCompleted());
                    }
                }
            }
        }
    }
}
