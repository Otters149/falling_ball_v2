using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    namespace helper
    {
        public enum TagManager
        {
            Die,
            Ball,
            AddScore,
        }

        static public class Extentions
        {
            static public string Value(this TagManager tagManager)
            {
                switch(tagManager)
                {
                    case TagManager.Die: return "Die";
                    case TagManager.Ball: return "Ball";
                    case TagManager.AddScore: return "AddScore";
                    default: return "Unknown";
                }
            }
        }
    }
}
