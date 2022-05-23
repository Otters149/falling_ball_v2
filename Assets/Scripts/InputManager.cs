using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace fallingball
{
    public class InputManager : MonoBehaviour
    {
        private GameObject _ball;
        [SerializeField]
        private float _jumpHeight;
        [SerializeField]
        private float _pushForce;

        private float _jumpForce;

        private bool _isHolding = false;

        private bool _isJump = false;
        void Start()
        {
            _ball = this.gameObject;
            _jumpForce = Mathf.Sqrt(_jumpHeight * -2 * (Physics2D.gravity.y * _ball.GetComponentInChildren<Rigidbody2D>().gravityScale));
            //_pushForce = Mathf.Sqrt(_pushDistance * -2 * (Physics2D.gravity.y * _ball.GetComponentInChildren<Rigidbody2D>().gravityScale));
        }

        void Update()
        {
            //@todo: add condition with touch screen
            if (Mouse.current.leftButton.wasPressedThisFrame && !_isHolding)
            {
                _isHolding = true;
                _isJump = true;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                _isHolding = false;
            }
        }

        private void FixedUpdate()
        {
            if(_isJump)
            {
                _isJump = false;
                var posClick = Mouse.current.position.ReadValue();
                if (posClick.x < Screen.width / 2)
                {
                    _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(_pushForce, _jumpForce), ForceMode2D.Impulse);
                }
                else
                {
                    _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-_pushForce, _jumpForce), ForceMode2D.Impulse);
                }
            }
        }
    }
}
