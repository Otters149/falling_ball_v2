using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace fallingball
{
    public class Score : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();
        }
        void Start()
        {
            GameManager.OnScoreIncrease += OnScoreChanged;
            GameManager.OnGameRestart += OnTryAgain;
        }

        private void OnDestroy()
        {
            GameManager.OnScoreIncrease -= OnScoreChanged;
            GameManager.OnGameRestart -= OnTryAgain;
        }

        private void OnScoreChanged()
        {
            var currentScore = int.Parse(_text.text);
            _text.text = (currentScore + 1).ToString();
        }

        private void OnTryAgain()
        {
            _text.text = "0";
        }
    }
}
