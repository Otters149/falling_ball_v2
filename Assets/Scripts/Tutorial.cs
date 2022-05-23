using fallingball.assetsloader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using utilpackages.qlog;

namespace fallingball
{
    public class Tutorial : MonoBehaviour
    {
        //private UnityAction _onClickAction;
        private GameObject _textObject;
        private Text _text;
        //private Button _button;
        private EventTrigger _eventTrigger;
        private QLog _logger;
        void Start()
        {
            _logger = QLog.GetInstance();
            _textObject = new GameObject("TutorialText");
            _textObject.transform.parent = transform;
            _text = _textObject.AddComponent<Text>();
            _text.text = "Tap To Start!";
            _text.font = AssetsLoader.GetResource<Font>(Keys.TUTORIAL_FONT);
            _text.fontSize = 60;
            _text.fontStyle = FontStyle.Bold;
            _text.alignment = TextAnchor.MiddleCenter;

            var rectTransform = _textObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = Vector2.one * 0.5f;
            rectTransform.localPosition = Vector2.zero;
            rectTransform.localScale = Vector2.one;

            //_button = _textObject.AddComponent<Button>();
            //_onClickAction += OnStart;
            //_button.onClick.AddListener(_onClickAction);

            _eventTrigger = _textObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener(data =>
            {
                _logger.LogDebug(_logger.GetClassName(this), data.ToString());
                GameManager.OnGameStart.Invoke();
                gameObject.SetActive(false);
            });
            _eventTrigger.triggers.Add(entry);

            GameManager.OnGameRestart += OnRestart;
        }

        private void OnDestroy()
        {
            GameManager.OnGameRestart -= OnRestart;
        }

        public void OnStart()
        {
            _logger.LogDebug(_logger.GetClassName(this), "Start Game");
            GameManager.OnGameStart.Invoke();
            gameObject.SetActive(false);
        }

        private void OnRestart()
        {
            gameObject.SetActive(true);
        }
    }
}
