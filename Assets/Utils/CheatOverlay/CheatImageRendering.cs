using System;
using UnityEngine;
using UnityEngine.UI;

namespace utilpackages
{
    namespace cheatoverlay
    {
        [Serializable]
        public class CheatImageRendering
        {
            public enum State
            {
                Idle,
                Pressed,
            }

            [SerializeField]
            private Sprite _idleState;
            [SerializeField]
            private Vector2 _size;
            [SerializeField]
            [Range(0,1)]
            private float _defaultOpacity = 0.2f;

            private GameObject _gameObject;
            private Image _image;
            private State _currentState;

            public Sprite IdleState { set => _idleState = value; }
            public Vector2 Size { get => _size; }
            public RectTransform Transform { get => _gameObject.transform as RectTransform; }

            public CheatImageRendering(Vector2 size)
            {
                _currentState = State.Idle;
                _size = size;
            }

            public void UpdateState(State newState)
            {
                if (_currentState != newState)
                {
                    _currentState = newState;
                    switch (newState)
                    {
                        case State.Idle:
                            _image.color = new Color(1f, 1f, 1f, _defaultOpacity);
                            return;
                        case State.Pressed:
                            _image.color = new Color(1f, 1f, 1f, 1f);
                            return;
                    }
                }
            }

            public void OnCreate(GameObject parent, string name)
            {
                _gameObject = CreateObject(parent, name);
                _image = CreateImage(_gameObject, _idleState);
                Transform.sizeDelta = _size;
                Transform.localScale = Vector2.one;
            }

            private GameObject CreateObject(GameObject parent, string name)
            {
                var gameObj = new GameObject(name);
                gameObj.transform.parent = parent.transform;
                gameObj.transform.localPosition = Vector3.zero;
                return gameObj;
            }

            private Image CreateImage(GameObject gameObj, Sprite sprite)
            {
                var image = gameObj.AddComponent<Image>();
                image.sprite = sprite;
                image.color = new Color(1f, 1f, 1f, _defaultOpacity);
                return image;
            }
        }
    }
}
