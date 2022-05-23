using fallingball.helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using utilpackages.qlog;

namespace fallingball
{
    namespace popup
    {
        public class GameOver : MonoBehaviour
        {
            [SerializeField]
            private Button _buttonHome;
            [SerializeField]
            private Button _buttonRetry;
            [SerializeField]
            private Button _buttonAds;

            private QLog _logger;

            private void Awake()
            {
                _logger = QLog.GetInstance();

                _buttonHome.onClick.AddListener(delegate {
                    _logger.LogInfo(_logger.GetClassName(this), "Go to main menu");
                    ScenePref.Push(SceneName.MainMenuScene);
                    SceneManager.LoadScene(SceneName.LoadingScene.GetName());
                });

                _buttonRetry.onClick.AddListener(delegate {
                    _logger.LogInfo(_logger.GetClassName(this), "Try again");
                    GameManager.OnGameRestart?.Invoke();
                });
                _buttonAds.onClick.AddListener(delegate {
                    _logger.LogInfo(_logger.GetClassName(this), "View Ads");
                    GameManager.OnContinueFromAds?.Invoke();
                    //@todo: intergate unity ads
                });
            }
        }
    }
}
