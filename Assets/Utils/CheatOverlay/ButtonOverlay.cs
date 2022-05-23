using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace utilpackages
{
    namespace cheatoverlay
    {
        public class ButtonOverlay : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
        {
            static bool _IS_INITIALIZE = false;
            private const float _DEFAULT_BUTTON_SIZE = 50f;
            private const float _MOVEMENT_RANGE = 9999f;
            private const float _PRESS_THRESHOLD = 0.2f;
            private const float _DRAG_THRESHOLD = 2f;
            private const float _SMOOTH_TRANSFORM = 700f;
            private const float _PADDING_EDGE = 0.5f;

            [SerializeField]
            private RectTransform _container;
            [SerializeField]
            private Vector2 _size;
            [SerializeField]
            private Sprite _sprite;

            [InputControl(layout = "Vector2")]
            [SerializeField]
            private string _controlPath;
            protected override string controlPathInternal
            {
                get => _controlPath;
                set => _controlPath = value;
            }

            protected Vector3 _startPos;
            protected Vector2 _touchDownPos;
            private CheatImageRendering _imageRendering;
            private PanelOverlay _panel;
            private bool _isUpdatePosition = true;
            private float _pressTime;

            private void Awake()
            {
                if (_IS_INITIALIZE)
                    Destroy(this.gameObject);
                _IS_INITIALIZE = true;
                DontDestroyOnLoad(this.gameObject);
            }

            private void Start()
            {
                if(_size == null)
                {
                    _size = Vector2.one * _DEFAULT_BUTTON_SIZE;
                }

                this.GetComponent<RectTransform>().sizeDelta = _size;

                _imageRendering = new CheatImageRendering(Vector3.one * _DEFAULT_BUTTON_SIZE);
                _imageRendering.IdleState = _sprite;
                _imageRendering.OnCreate(this.gameObject, "CheatButtonImage");

                _panel = new PanelOverlay();
                _panel.OnCreate(this.gameObject.transform.parent.gameObject, "CheatPanel", () => OnPanelStatusChanged(false));
                OnPanelStatusChanged(false);
            }

            private void Update()
            {
                if (_isUpdatePosition)
                {
                    if (GetComponent<RectTransform>().localPosition.x <= (_container.rect.xMax + _container.rect.xMin) / 2)
                    {
                        GetComponent<RectTransform>().localPosition += (Vector3)(Vector2.left * _SMOOTH_TRANSFORM * Time.deltaTime);
                    }
                    else
                    {
                        GetComponent<RectTransform>().localPosition += (Vector3)(Vector2.right * _SMOOTH_TRANSFORM * Time.deltaTime);
                    }
                    if ((GetComponent<RectTransform>().localPosition.x - _container.rect.xMin - _imageRendering.Transform.rect.width/2) <= _PADDING_EDGE
                        || (_container.rect.xMax - GetComponent<RectTransform>().localPosition.x - _imageRendering.Transform.rect.width/2) <= _PADDING_EDGE)
                    {
                        _isUpdatePosition = false;
                        _startPos = GetComponent<RectTransform>().localPosition;
                    }

                    if((GetComponent<RectTransform>().localPosition.x - _container.rect.xMin - _imageRendering.Transform.rect.width / 2) <= _PADDING_EDGE)
                    {
                        _isUpdatePosition = false;
                        _startPos = new Vector3(_container.rect.xMin + _imageRendering.Transform.rect.width / 2 + _PADDING_EDGE, GetComponent<RectTransform>().localPosition.y, 0);
                        GetComponent<RectTransform>().localPosition = _startPos;
                    }
                    else if((_container.rect.xMax - GetComponent<RectTransform>().localPosition.x - _imageRendering.Transform.rect.width / 2) <= _PADDING_EDGE)
                    {
                        _isUpdatePosition = false;
                        _startPos = new Vector3(_container.rect.xMax - _imageRendering.Transform.rect.width / 2 - _PADDING_EDGE, GetComponent<RectTransform>().localPosition.y, 0);
                        GetComponent<RectTransform>().localPosition = _startPos;
                    }
                }
            }

            public void OnPointerDown(PointerEventData eventData)
            {
                _isUpdatePosition = false;
                if (eventData == null)
                    throw new System.ArgumentNullException(nameof(eventData));

                _startPos = GetComponent<RectTransform>().localPosition;
                _imageRendering.UpdateState(CheatImageRendering.State.Pressed);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _touchDownPos);

                _pressTime = Time.time;
            }

            public void OnPointerUp(PointerEventData eventData)
            {
                _imageRendering.UpdateState(CheatImageRendering.State.Idle);
                SendValueToControl(Vector2.zero);
                _isUpdatePosition = true;

                if (Time.time - _pressTime < _PRESS_THRESHOLD && !IsDragged(_startPos, eventData.position))
                {
                    _pressTime = 0;
                    OnPanelStatusChanged(true);
                }
            }

            // TODO: Check drag on screen
            public void OnDrag(PointerEventData eventData)
            {
                if (eventData == null)
                    throw new System.ArgumentNullException(nameof(eventData));

                RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
                var delta = position - _touchDownPos;

                delta = Vector2.ClampMagnitude(delta, _MOVEMENT_RANGE);
                //((RectTransform)transform).anchoredPosition = _startPos + (Vector3)delta;
                var deltaClamp = _startPos + (Vector3)delta;
                ((RectTransform)transform).anchoredPosition =
                    new Vector3(Mathf.Clamp(deltaClamp.x,
                                            -1 * _container.rect.width / 2 + _imageRendering.Transform.rect.width / 2 + _PADDING_EDGE,
                                            _container.rect.width / 2 - _imageRendering.Transform.rect.width / 2 - _PADDING_EDGE),
                                Mathf.Clamp(deltaClamp.y,
                                            -1 * _container.rect.height / 2 + _imageRendering.Transform.rect.width / 2 + _PADDING_EDGE,
                                            _container.rect.height / 2 - _imageRendering.Transform.rect.width / 2 - _PADDING_EDGE),
                                0f);

                var newPos = new Vector2(delta.x / _MOVEMENT_RANGE, delta.y / _MOVEMENT_RANGE);
                SendValueToControl(newPos);
            }

            private void OnPanelStatusChanged(bool panelStatus)
            {
                this.gameObject.SetActive(!panelStatus);
                _panel.SetActive(panelStatus);
            }

            private bool IsDragged(Vector2 start, Vector2 end)
            {
                return false;
                // Check move in draghold
                //return Mathf.Sqrt(Mathf.Abs(start.x - end.x) * Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y) * Mathf.Abs(start.y - end.y)) >= _DRAG_THRESHOLD;
            }

        }
    }
}