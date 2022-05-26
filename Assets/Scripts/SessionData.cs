using data.entity;
using fallingball.assetsloader;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

        public void Sync()
        {
            var shop = JsonUtility.ToJson(_shopData);
            var user = JsonUtility.ToJson(_userData);
            _logger.LogInfo(_logger.GetClassName(this), shop);
            _logger.LogInfo(_logger.GetClassName(this), user);
            File.WriteAllText(AssetDatabase.GetAssetPath(AssetsLoader.GetResource<TextAsset>(Keys.SHOP_DATA)), shop);
            File.WriteAllText(AssetDatabase.GetAssetPath(AssetsLoader.GetResource<TextAsset>(Keys.USER_DATA)), user);
#if _DEBUG
            _logger.LogWarning(_logger.GetClassName(this), "[SaveData] Setdirty: data will be revert after quit game");
            EditorUtility.SetDirty(AssetsLoader.GetResource<TextAsset>(Keys.SHOP_DATA));
            EditorUtility.SetDirty(AssetsLoader.GetResource<TextAsset>(Keys.USER_DATA));
#endif
        }
    }

}