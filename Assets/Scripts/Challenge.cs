using fallingball.helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fallingball
{
    public class Challenge : MonoBehaviour
    {
        private int _sameCount = 0;
        private int _preRandom = -1;
        public void RandomChallenge(List<GameObject> thornManager, GameObject prefabs, Camera mainCamera)
        {
            //var rd = Random.Range(0, 4);
            var rd = Random.Range(0, 3);
            if(rd != _preRandom)
            {
                _preRandom = rd;
                ChallengeFactory(rd, prefabs, mainCamera, thornManager);
            }
            else if(_sameCount > 0)
            { 
                _sameCount--;
                ChallengeFactory(rd, prefabs, mainCamera, thornManager);
            }
            else
            {
                if (_sameCount <= 0)
                {
                    _sameCount = Random.Range(1, 2);
                }
                RandomChallenge(thornManager, prefabs, mainCamera);
            }
        }
        private void ChallengeFactory(int type, GameObject prefabs, Camera mainCamera, List<GameObject> thornManager)
        {
            if(type == 0)
            {
                CreateChallengeLeft(thornManager, prefabs, mainCamera);
            }
            else if(type == 1)
            {
                CreateChallengeRight(thornManager, prefabs, mainCamera);
            }
            else if(type == 2)
            {
                CreateChallengeCenterSmall(thornManager, prefabs, mainCamera);
            }
            //else if(type == 3)
            //{
            //    CreateChallengeCenterLarge(thornManager, prefabs, mainCamera);
            //}
        }

        public void CreateChallengeLeft(List<GameObject> thornManager, GameObject prefabs, Camera mainCamera)
        {
            var thorn = Instantiate(prefabs);
            thorn.name = "LeftChallenge";
            thorn.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var worldPoint = mainCamera.ScreenToWorldPoint(Vector3.zero);
            thorn.transform.position = new Vector3(worldPoint.x + thorn.GetComponent<SpriteRenderer>().bounds.extents.x, worldPoint.y - thorn.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            thorn.GetComponent<SpriteRenderer>().sortingOrder = 400;
            thornManager.Add(thorn);

            var thorn2 = Instantiate(prefabs);
            thorn2.name = "LeftChallenge";
            thorn2.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var worldPoint2 = mainCamera.ScreenToWorldPoint(Vector3.zero);
            thorn2.transform.position = new Vector3(worldPoint2.x + thorn2.GetComponent<SpriteRenderer>().bounds.extents.x * 3, worldPoint2.y - thorn2.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            thorn2.GetComponent<SpriteRenderer>().sortingOrder = 400;
            thornManager.Add(thorn2);

            AddScoreCollider(thorn.transform);
        }

        public void CreateChallengeRight(List<GameObject> thornManager, GameObject prefabs, Camera mainCamera)
        {
            var thorn = Instantiate(prefabs);
            thorn.name = "RightChallenge";
            thorn.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
            thorn.transform.position = new Vector3(worldPoint.x - thorn.GetComponent<SpriteRenderer>().bounds.extents.x, worldPoint.y - thorn.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            thorn.GetComponent<SpriteRenderer>().sortingOrder = 400;
            thornManager.Add(thorn);

            var thorn2 = Instantiate(prefabs);
            thorn2.name = "RightChallenge";
            thorn2.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var worldPoint2 = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
            thorn2.transform.position = new Vector3(worldPoint2.x - thorn2.GetComponent<SpriteRenderer>().bounds.extents.x * 3, worldPoint2.y - thorn2.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            thorn2.GetComponent<SpriteRenderer>().sortingOrder = 400;
            thornManager.Add(thorn2);

            AddScoreCollider(thorn.transform);
        }

        public void CreateChallengeCenterSmall(List<GameObject> thornManager, GameObject prefabs, Camera mainCamera)
        {
            var thorn = Instantiate(prefabs);
            thorn.name = "SmallChallenge";
            thorn.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0f, 0f));
            thorn.transform.position = new Vector3(worldPoint.x, worldPoint.y - thorn.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            thorn.GetComponent<SpriteRenderer>().sortingOrder = 400;
            thornManager.Add(thorn);

            AddScoreCollider(thorn.transform);
        }

        public void CreateChallengeCenterLarge(List<GameObject> thornManager, GameObject prefabs, Camera mainCamera)
        {
            var thorn = Instantiate(prefabs);
            thorn.name = "LargeChallenge";
            thorn.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0f, 0f));
            thorn.transform.position = new Vector3(worldPoint.x, worldPoint.y - thorn.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
            thorn.GetComponent<SpriteRenderer>().sortingOrder = 400;
            thornManager.Add(thorn);

            AddScoreCollider(thorn.transform);
        }

        private void AddScoreCollider(Transform parent)
        {
            var addScore = new GameObject("AddScore");
            addScore.transform.parent = parent;
            addScore.transform.localScale = Vector3.one;
            addScore.tag = TagManager.AddScore.Value();
            addScore.transform.localPosition = new Vector3(0f, 0f, 0f);
            var boxConlider = addScore.AddComponent<BoxCollider2D>();
            boxConlider.size = new Vector2(100f, 5f);
            boxConlider.isTrigger = true;
        }
    }
}
