using data.entity;
using fallingball.assetsloader;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using utilpackages.qlog;

namespace fallingball
{
    namespace popup
    {
        public class Shop : MonoBehaviour
        {
            [SerializeField]
            private GameObject _popupContainer;
            [SerializeField]
            private Button _buttonClose;
            [SerializeField]
            private Button _buttonConfirm;
            [SerializeField]
            private TextMeshProUGUI _textGold;
            [SerializeField]
            private GameObject _gridBallShopping;
            [SerializeField]
            private Scrollbar _scrollBar;
            [SerializeField]
            private GameObject _itemPrefabs;

            private QLog _logger;

            private List<ItemShopping> _itemShoppings;

            private void Awake()
            {
                _logger = QLog.GetInstance();
                _itemShoppings = new List<ItemShopping>();
            }

            void Start()
            {
                _buttonClose.onClick.AddListener(OnCloseClicked);
                _buttonConfirm.onClick.AddListener(OnConfirmSettings);
            }

            private void OnEnable()
            {
                SessionData session = GameObject.Find("GameCore").GetComponent<SessionData>();
                if(session == null)
                {
                    _logger.LogError(_logger.GetClassName(this), "[SHOP] Can not load data from session (gamecore)");
                    return;
                }

                User_Data userData = session.UserData;
                _textGold.text = userData.gold.ToString();

                foreach (Transform child in _gridBallShopping.transform)
                {
                    // Clear all children in list
                    Destroy(child.gameObject);
                }
                Shop_Data shopData = session.ShopData;
                foreach (var item in shopData.shop_data)
                {
                    var ball = Instantiate(_itemPrefabs, _gridBallShopping.transform);
                    var itemShopping = ball.GetComponent<ItemShopping>();
                    itemShopping.ItemData = item;
                    itemShopping.onItemSelectedAction += OnItemSelectChanged;
                    _itemShoppings.Add(itemShopping);
                    if (item.id == userData.current_selected)
                    {
                        itemShopping.Active();
                    }
                    else
                    {
                        itemShopping.Deactive();
                    }  
                }
                StartCoroutine(ResetItemsLayoutPosition());
            }

            private void OnCloseClicked()
            {
                _logger.LogDebug(_logger.GetClassName(this), "Shop Popup Closed");
                gameObject.SetActive(false);
                _popupContainer.SetActive(false);
            }

            private void OnConfirmSettings()
            {
                _logger.LogDebug(_logger.GetClassName(this), "Shop Popup Confirmed");
                //gameObject.SetActive(false);
                //_popupContainer.SetActive(false);
                // dont quit
                //@todo: handle confirm select or buy character
            }

            private IEnumerator ResetItemsLayoutPosition()
            {
                yield return null;
                _scrollBar.value = 1f;
            }

            private void OnItemSelectChanged(string id)
            {
                //@todo: update session data

                foreach(var item in _itemShoppings)
                {
                    item.OnSelectChanged(id);
                }
            }
        }
    }
}
