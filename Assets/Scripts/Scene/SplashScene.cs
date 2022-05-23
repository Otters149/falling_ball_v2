using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using core;
using utilpackages.qlog;
using UnityEngine.Video;
using fallingball.helper;
using UnityEngine.SceneManagement;
using fallingball.assetsloader;

namespace fallingball
{
    namespace scene
    {
        public class SplashScene : NAScene
        {
            private AsyncOperation _loadSceneAsyncOperation;
            private VideoPlayer _videoPlayer;
            private QLog _logger;
            private void Awake()
            {
                _hasLaunchFromBegin = true;
                _logger = QLog.GetInstance();

                _videoPlayer = gameObject.transform.Find("VideoPlayer").GetComponent<VideoPlayer>();
                if (_videoPlayer == null)
                {
                    _logger.LogError(_logger.GetClassName(this), "Video Player was null");
#if _RETAIL
                    SceneHelper.Quit();
#endif
                }
                _videoPlayer.clip = AssetsLoader.GetResource<VideoClip>(Keys.INTRO_VIDEO);

                if (_videoPlayer.clip == null)
                {
                    _logger.LogError(_logger.GetClassName(this), "Can not set clip playback");
#if _RETAIL
                    SceneHelper.Quit();
#endif
                }
            }

            protected override void Start()
            {
                base.Start();

                ScenePref.Push(SceneName.MainMenuScene);

                _loadSceneAsyncOperation = SceneManager.LoadSceneAsync(SceneName.LoadingScene.GetName());
                _loadSceneAsyncOperation.allowSceneActivation = false;
                _loadSceneAsyncOperation.completed += (result) => _logger.LogInfo(_logger.GetClassName(this), "Load Loading Scene Success!");

                _videoPlayer.loopPointReached += EndPointReadched;
                _videoPlayer.Play();
            }

            private void EndPointReadched(VideoPlayer videoPlayer)
            {
                _videoPlayer.loopPointReached -= EndPointReadched;
                _videoPlayer.gameObject.SetActive(false);

                _loadSceneAsyncOperation.allowSceneActivation = true;
            }
        }
    }
}
