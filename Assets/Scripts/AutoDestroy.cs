using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    public class AutoDestroy : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private bool _hasEnterTheScreen = false;
        void Start()
        {
            _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if(!_hasEnterTheScreen)
            {
                if ((Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.center).y >= 0 && Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.center).y <= Screen.height)
                    && (Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.center).x >= 0 && Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.center).x <= Screen.width))
                {
                    _hasEnterTheScreen = true;
                }
            }
            else if (_spriteRenderer != null)
            {
                if (Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.min).y > Screen.height
                    || Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.max).y < 0
                    || Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.min).x > Screen.width
                    || Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.max).x < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
