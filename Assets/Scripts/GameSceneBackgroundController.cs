using core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utilpackages.qlog;

namespace fallingball
{
    public class GameSceneBackgroundController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _cloud1;
        [SerializeField]
        private GameObject _cloud2;

        private QLog _logger;

        private void Awake()
        {
            _logger = QLog.GetInstance();
        }
        private void Start()
        {
            if (_cloud1 == null)
            {
                _logger.LogError(_logger.GetClassName(this), "Object cloud1 null");
            }
            if (_cloud2 == null)
            {
                _logger.LogError(_logger.GetClassName(this), "Object cloud2 null");
            }

            StartCoroutine(StartBoundingCloud());
        }

        private IEnumerator StartBoundingCloud()
        {
            CreateClound();
            while(true)
            {
                if(GamePlayManager.isStartGame)
                {
                    CreateClound();
                }
                yield return new WaitForSeconds(10f);
            }
        }

        private void CreateClound()
        {
            var cloud1 = Instantiate(_cloud1);
            var direction1 = Cloud.Direction.LTR; /*(int)Random.Range(0, 100) / 2 == 0 ? Cloud.Direction.LTR : Cloud.Direction.RTL;*/
            cloud1.GetComponent<Cloud>().direction = direction1;
            cloud1.GetComponent<Cloud>().speed = Random.Range(0.3f, 0.5f);
            cloud1.transform.position = direction1 == Cloud.Direction.LTR ? GetLeftBounding(cloud1) : GetRightBounding(cloud1);
            cloud1.GetComponent<SpriteRenderer>().sortingOrder = 10;
            cloud1.AddComponent<AutoDestroy>();

            var cloud2 = Instantiate(_cloud2);
            var direction2 = Cloud.Direction.LTR; /*(int)Random.Range(0, 100) / 2 == 0 ? Cloud.Direction.LTR : Cloud.Direction.RTL;*/
            cloud2.GetComponent<Cloud>().direction = direction2;
            cloud2.GetComponent<Cloud>().speed = Random.Range(0.1f, 0.3f);
            cloud2.transform.position = direction2 == Cloud.Direction.LTR ? GetLeftBounding(cloud2) : GetRightBounding(cloud2);
            cloud1.GetComponent<SpriteRenderer>().sortingOrder = 5;
            cloud2.AddComponent<AutoDestroy>();
        }

        private Vector2 GetLeftBounding(GameObject gObj)
        {
            var leftSide = new Vector2(0, Random.Range(0, Screen.height));
            var leftSideInWordSpace = Camera.main.ScreenToWorldPoint((Vector3)leftSide);
            return leftSideInWordSpace - new Vector3(gObj.GetComponent<SpriteRenderer>().bounds.max.x - gObj.GetComponent<SpriteRenderer>().bounds.center.x, 0, 0);
        }
        private Vector2 GetRightBounding(GameObject gObj)
        {
            var rightSide = new Vector2(Screen.width, Random.Range(0, Screen.height));
            var rightSideInWordSpace = Camera.main.ScreenToWorldPoint((Vector3)rightSide);
            return rightSideInWordSpace + new Vector3(gObj.GetComponent<SpriteRenderer>().bounds.center.x - gObj.GetComponent<SpriteRenderer>().bounds.min.x, 0, 0);
        }
    }
}
