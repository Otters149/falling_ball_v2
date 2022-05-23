using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanelOverlay : MonoBehaviour
{
    private const float _CLOSE_BUTTON_SIZE = 50f;

    private GameObject _panel;
    private Image _background;
    private GameObject _buttonClose;
    private GameObject _actionListContainer;

    public GameObject Panel => _panel;

    public void OnCreate(GameObject parent, string name, Action onClose)
    {
        _panel = new GameObject(name);
        _panel.transform.parent = parent.transform;
        var rectTransform = _panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.one * 0.05f;
        rectTransform.anchorMax = Vector2.one * 0.95f;
        rectTransform.pivot = Vector2.one * 0.5f;
        rectTransform.localScale = Vector3.one;
        rectTransform.localPosition = Vector3.zero;
        // set bot, left
        rectTransform.offsetMin = Vector3.zero; //Vector3.one * _PADDING;
        // set top, right
        rectTransform.offsetMax = Vector3.zero; //Vector3.one * _PADDING;

        _background = _panel.AddComponent<Image>();
        _background.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        _background.color = new Color(1, 1, 1, 0.5f);
        _background.type = Image.Type.Sliced;
        _background.fillCenter = true;

        _buttonClose = AddButtonClose(_panel, "Button_Close", onClose);

        _actionListContainer = AddListButtonAction(_panel);

    }

    private GameObject AddButtonClose(GameObject parent, string name, Action onClose)
    {
        var button = new GameObject(name);
        button.transform.parent = parent.transform;
        var rectTranform = button.AddComponent<RectTransform>();
        rectTranform.anchorMax = Vector3.one;
        rectTranform.anchorMin = Vector3.one;
        rectTranform.pivot = Vector2.one * 0.5f;
        rectTranform.localScale = Vector3.one;
        rectTranform.sizeDelta = Vector2.one * _CLOSE_BUTTON_SIZE;
        rectTranform.anchoredPosition3D = Vector3.zero; // new Vector3(_CLOSE_BUTTON_SIZE / 2, _CLOSE_BUTTON_SIZE / 2, 0) * -1f;

        var buttonComponent = button.AddComponent<Button>();
        buttonComponent.onClick.AddListener(new UnityEngine.Events.UnityAction(onClose));

        var imageComponent = button.AddComponent<Image>();
        imageComponent.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        imageComponent.color = new Color(0.7f, 0f, 0f, 1f);
        buttonComponent.targetGraphic = imageComponent;

        // Create icon X by text
        var textObj = new GameObject("Close_Button_Text");
        textObj.transform.parent = button.transform;
        var textComponent = textObj.AddComponent<Text>();
        textComponent.text = "X";
        textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComponent.fontSize = 40;
        textComponent.fontStyle = FontStyle.Bold;
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        var textTransform = textObj.GetComponent<RectTransform>();
        textTransform.localScale = Vector3.one;
        textTransform.localPosition = Vector3.zero;
        textTransform.pivot = Vector2.one * 0.5f;
        textTransform.anchorMin = Vector2.one * 0.5f;
        textTransform.anchorMax = Vector2.one * 0.5f;

        return button;
    }

    private GameObject AddListButtonAction(GameObject gameObj)
    {
        var srollrect = gameObj.AddComponent<ScrollRect>();
        srollrect.horizontal = false;
        srollrect.vertical = true;
        srollrect.movementType = ScrollRect.MovementType.Elastic;

        // Add panel container
        var pannel = new GameObject("List_Action_Cheat");
        var pannelRectTransform = pannel.AddComponent<RectTransform>();
        pannelRectTransform.parent = gameObj.transform;
        pannelRectTransform.localScale = Vector2.one;
        pannelRectTransform.pivot = Vector2.one * 0.5f;
        pannelRectTransform.anchorMin = Vector2.zero;
        pannelRectTransform.anchorMax = Vector2.one;
        pannelRectTransform.localPosition = Vector3.zero;
        pannelRectTransform.sizeDelta = Vector3.zero;
        srollrect.content = pannelRectTransform;

        var gridLayout = pannel.AddComponent<GridLayoutGroup>();
        gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
        gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
        gridLayout.constraint = GridLayoutGroup.Constraint.Flexible;
        gridLayout.cellSize = new Vector2(200f, 100f);
        gridLayout.spacing = new Vector2(15f, 15f);

        var contentFilter = pannel.AddComponent<ContentSizeFitter>();
        contentFilter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        contentFilter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // TODO: Add action CB
        for (int i = 0; i < 10; i++)
        {
            var btn = new GameObject("BTN" + i);
            var component = btn.AddComponent<Button>();
            btn.transform.parent = pannel.transform;
        }

        return pannel;
    }

    public void SetActive(bool active)
    {
        _panel.SetActive(active);
    }
}
