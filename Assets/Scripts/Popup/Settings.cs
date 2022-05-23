using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using utilpackages.qlog;

namespace fallingball
{
    namespace popup
    {
        public class Settings : MonoBehaviour
        {
            [SerializeField]
            private GameObject _popupContainer;
            [SerializeField]
            private Button _buttonClose;
            [SerializeField]
            private Button _buttonDone;

            private QLog _logger;

            private void Awake()
            {
                _logger = QLog.GetInstance();
            }

            void Start()
            {
                _buttonClose.onClick.AddListener(OnCloseClicked);
                _buttonDone.onClick.AddListener(OnConfirmSettings);
            }

            private void OnCloseClicked()
            {
                _logger.LogDebug(_logger.GetClassName(this), "Settings Popup Closed");
                gameObject.SetActive(false);
                _popupContainer.SetActive(false);
            }

            private void OnConfirmSettings()
            {
                _logger.LogDebug(_logger.GetClassName(this), "Settings Popup Confirmed");
                gameObject.SetActive(false);
                _popupContainer.SetActive(false);
                //@todo: handle apply settings
            }
        }
    }
}
