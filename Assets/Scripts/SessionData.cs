using data.entity;
using fallingball.assetsloader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utilpackages.qlog;

namespace fallingball
{
    public class SessionData : MonoBehaviour
    {
        private Shop_Data _shopData;
        private User_Data _userData;

        private QLog _logger;

        public Shop_Data ShopData
        {
            get => _shopData;
        }

        public User_Data UserData
        {
            get => _userData;
        }

        private void Awake()
        {
            _logger = QLog.GetInstance();

            _shopData = JsonUtility.FromJson<Shop_Data>(AssetsLoader.GetResource<TextAsset>(Keys.SHOP_DATA).text);
            if(_shopData == null)
            {
                _logger.LogError(_logger.GetClassName(this), "Could not load shop data!");
            }

            _userData = JsonUtility.FromJson<User_Data>(AssetsLoader.GetResource<TextAsset>(Keys.USER_DATA).text);
            if (_userData == null)
            {
                _logger.LogError(_logger.GetClassName(this), "Could not load user data!");
            }
        }
    }

}