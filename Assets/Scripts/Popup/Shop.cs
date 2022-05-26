using data.entity;
using fallingball.assetsloader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            private BuyButton _buttonConfirm;
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
            private SessionData _session;
            private string _idItemSelected;
            private bool _isInitialize = false;
            private void Awake()
            {
                _logger = QLog.GetInstance();
                _itemShoppings = new List<ItemShopping>();
            }

            void Start()
            {
                _buttonClose.onClick.AddListener(OnCloseClicked);
                //_buttonConfirm.transform.GetComponent<Button>().onClick.AddListener(OnConfirmSettings);
                _buttonConfirm.AddListener(OnConfirmSettings);
                _buttonConfirm.State = BuyButton.BuyButtonState.Selected;
            }

            private void OnEnable()
            {
                if(!_isInitialize)
                {
                    _isInitialize = true;
                    _session = GameObject.Find("GameCore").GetComponent<SessionData>();
                    if (_session == null)
                    {
                        _logger.LogError(_logger.GetClassName(this), "[SHOP] Can not load data from session (gamecore)");
                        return;
                    }

                    User_Data userData = _session.UserData;
                    _textGold.text = userData.gold.ToString();

                    Shop_Data shopData = _session.ShopData;
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
                else
                {
                    OnItemSelectChanged(_session.UserData.current_selected);
                }
            }
            private void OnDestroy()
            {
                foreach (Transform child in _gridBallShopping.transform)
                {
                    // Clear all children in list
                    Destroy(child.gameObject);
                }
                _itemShoppings.Clear();
            }
            private void OnCloseClicked()
            {
                _logger.LogDebug(_logger.GetClassName(this), "Shop Popup Closed");
                gameObject.SetActive(false);
                _popupContainer.SetActive(false);
            }

            private void OnConfirmSettings()
            {
                _logger.LogInfo(_logger.GetClassName(this), "Shop Popup Confirmed");
                if(_buttonConfirm.State == BuyButton.BuyButtonState.Select)
                {
                    _session.UserData.current_selected = _idItemSelected;
                    _buttonConfirm.State = BuyButton.BuyButtonState.Selected;
                    _session.Sync();
                }
                else if( _buttonConfirm.State == BuyButton.BuyButtonState.Buy)
                {
                    //@todo: handle check gold and buy success or more gold

                    _session.UserData.current_selected = _idItemSelected;
                    _session.ShopData.shop_data.Where(item => item.id == _idItemSelected).First().is_bought = true;
                    _session.UserData.total_ball++;
                    _buttonConfirm.State = BuyButton.BuyButtonState.Selected;
                    _itemShoppings.Where(item => item.ItemData.id == _idItemSelected).First().Buy();
                    _session.Sync();
                }
            }

            private IEnumerator ResetItemsLayoutPosition()
            {
                yield return null;
                _scrollBar.value = 1f;
            }

            private void OnItemSelectChanged(string id)
            {
                _idItemSelected = id;
                if(id == _session.UserData.current_selected)
                {
                    _buttonConfirm.State = BuyButton.BuyButtonState.Selected;
                }
                //else if(_session.ShopData.shop_data.Where(item => item.is_bought).Select(item => item.id).Contains(id))
                else if(_session.ShopData.shop_data.Where(item => item.id == id).First().is_bought)
                {
                    _buttonConfirm.State = BuyButton.BuyButtonState.Select;
                }
                else
                {
                    _buttonConfirm.State = BuyButton.BuyButtonState.Buy;
                }

                foreach(var item in _itemShoppings)
                {
                    item.OnSelectChanged(id);
                }
            }
        }
    }
}
