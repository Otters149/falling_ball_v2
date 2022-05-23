using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    public class GamePlayUIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _popupEndGame;

        void Start()
        {
            _popupEndGame.SetActive(false);
            GameManager.OnGameEnd += OnEndGame;
            GameManager.OnGameRestart += OnGameRestart;
            GameManager.OnContinueFromAds += OnContinue;
        }

        private void OnDestroy()
        {
            GameManager.OnGameEnd -= OnEndGame;
            GameManager.OnGameRestart -= OnGameRestart;
            GameManager.OnContinueFromAds -= OnContinue;
        }

        private void OnGameRestart()
        {
            _popupEndGame.SetActive(false);
        }

        private void OnEndGame()
        {
            _popupEndGame.SetActive(true);
        }

        private void OnContinue()
        {
            _popupEndGame.SetActive(false);
        }
    }
}
