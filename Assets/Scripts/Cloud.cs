using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    public class Cloud : MonoBehaviour
    {
        public enum Direction
        {
            LTR,
            RTL,
            NONE
        }

        public Direction direction = Direction.NONE;
        public float speed;
        public float speedUp;

        private void Update()
        {
            if(GamePlayManager.isStartGame)
            {
                if (direction == Direction.LTR)
                {
                    transform.position += Vector3.right * Time.deltaTime * speed;
                }
                else if (direction == Direction.RTL)
                {
                    transform.position += Vector3.left * Time.deltaTime * speed;
                }

                transform.position += Vector3.up * Time.deltaTime * speedUp;
            }
        }
    }
}
