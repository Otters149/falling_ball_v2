using fallingball.helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    public class Ball : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == TagManager.Die.Value())
            {
                GameManager.OnGameEnd.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.transform.tag == TagManager.AddScore.Value())
            {
                Destroy(other);
                GameManager.OnScoreIncrease.Invoke();
            }
        }
    }
}
