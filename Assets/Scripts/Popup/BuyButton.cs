using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public enum BuyButtonState
    {
        Select,
        Selected,
        Buy
    }

    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private Sprite _bgSelected;

    [SerializeField]
    private Sprite _bgSelect;

    [SerializeField]
    private Sprite _bgBuy;

    private Button _btn;

    private BuyButtonState _currentState;

    public BuyButtonState State { get => _currentState;
        set
        {
            _currentState = value;
            switch (value)
            {
                case BuyButtonState.Select:
                    _btn.image.sprite = _bgSelect;
                    _btn.enabled = true;
                    _text.text = "Select";
                    break;
                case BuyButtonState.Selected:
                    _btn.image.sprite = _bgSelected;
                    _btn.enabled = false;
                    _text.text = "Selected";
                    break;
                case BuyButtonState.Buy:
                    _btn.image.sprite = _bgBuy;
                    _btn.enabled = true;
                    _text.text = "Buy";
                    break;
            }
        }
    }

    public string Text { set => _text.text = value; }

    public void Awake()
    {
        _btn = transform.GetComponent<Button>();
    }

    public void AddListener(Action cb)
    {
        _btn.onClick.AddListener(new UnityEngine.Events.UnityAction(cb));
    }
}
