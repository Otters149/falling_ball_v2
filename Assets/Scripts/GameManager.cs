using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    public class GameManager : MonoBehaviour
    {
        static public Action OnGameStart;
        static public Action OnGameEnd;
        static public Action OnScoreIncrease;
        static public Action OnGameRestart;
        static public Action OnContinueFromAds;
    }
}
