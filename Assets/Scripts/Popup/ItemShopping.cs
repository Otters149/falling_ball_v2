using data.entity;
using fallingball.assetsloader;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace fallingball
{
    namespace popup
    {
        public class ItemShopping : MonoBehaviour
        {
            public Action<string> onItemSelectedAction;
            private Shop_Item _itemData;
            public Shop_Item ItemData
            {
                get
                {
                    return _itemData;
                }
                set
                {
                    _itemData = value;
                    OnDataChanged();
                }
            }

            private void Start()
            {
                GetComponent<Button>().onClick.AddListener(OnSelected);
            }

            private void OnDestroy()
            {
                onItemSelectedAction = null;
            }

            public void Active()
            {
                transform.Find("BG_Effect").GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }

            public void Deactive()
            {
                transform.Find("BG_Effect").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            }

            public void OnSelectChanged(string id)
            {
                if(_itemData.id == id)
                {
                    Active();
                }
                else
                {
                    Deactive();
                }
            }

            public void Buy()
            {
                _itemData.is_bought = true;
                transform.Find("Price").GetComponent<TextMeshProUGUI>().text = _itemData.is_bought ? "Owned" : _itemData.price.ToString();
            }

            private void OnDataChanged()
            {
                transform.Find("Image").GetComponent<Image>().sprite = AssetsLoader.GetResource<Sprite>(_itemData.place_holder);
                transform.Find("Price").GetComponent<TextMeshProUGUI>().text =_itemData.is_bought? "Owned" : _itemData.price.ToString();
            }

            private void Awake()
            {
                Deactive();
            }

            private void OnSelected()
            {
                onItemSelectedAction?.Invoke(_itemData.id);
            }
        }
    }
}
