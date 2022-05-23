using fallingball.helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using utilpackages.qlog;

namespace fallingball
{
    public class MainMenuScene : MonoBehaviour
    {
        [SerializeField]
        private Button _buttonStart;
        [SerializeField]
        private Button _buttonSettings;
        [SerializeField]
        private Button _buttonShop;
        [SerializeField]
        private Button _buttonRemoveAds;

        [Header("Popup")]
        [SerializeField]
        private GameObject _popupContainer;
        [SerializeField]
        private GameObject _popupSettings;
        [SerializeField]
        private GameObject _popupShop;
        [SerializeField]
        private GameObject _popupYesNoPrefabs;

        private QLog _logger;
        private void Awake()
        {
            _logger = QLog.GetInstance();
            _buttonStart.onClick.AddListener(OnStartClicked);
            _buttonSettings.onClick.AddListener(OnSettingClicked);
            _buttonShop.onClick.AddListener(OnShopClicked);
            _buttonRemoveAds.onClick.AddListener(OnRemoveAdsClicked);

            _popupContainer.SetActive(false);
            _popupSettings.SetActive(false);
            _popupShop.SetActive(false);
        }

        private void OnStartClicked()
        {
            _logger.LogDebug(_logger.GetClassName(this), "OnStartClicked");
            ScenePref.Push(SceneName.GameScene);
            SceneManager.LoadScene(SceneName.LoadingScene.GetName());
        }

        private void OnSettingClicked()
        {
            _logger.LogDebug(_logger.GetClassName(this), "OnSettingsClicked");
            _popupContainer.SetActive(true);
            _popupSettings.SetActive(true);
        }

        private void OnShopClicked()
        {
            _logger.LogDebug(_logger.GetClassName(this), "OnShopClicked");
            _popupContainer.SetActive(true);
            _popupShop.SetActive(true);
        }

        private void OnRemoveAdsClicked()
        {
            _logger.LogDebug(_logger.GetClassName(this), "OnRemoveAdsClicked");
            //@todo: integrate
        }
    }

}