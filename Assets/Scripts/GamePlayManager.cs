using fallingball.helper;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  fallingball
{
    public class GamePlayManager : MonoBehaviour
    {
        static public bool isEndGame = false;
        static public bool isStartGame = false;

        [SerializeField]
        private GameObject _thornPrefabs;
        [SerializeField]
        private GameObject _ballPrefabs;
        [SerializeField]
        private Challenge _challenge;
        [SerializeField]
        private float _speed;
        private GameObject _ceiling;
        private GameObject _leftEdge;
        private GameObject _rightEdge;
        private GameObject _ground;

        private List<GameObject> _thornChallenge;
        private List<GameObject> _thornOnLeftSide;
        private List<GameObject> _thornOnRightSide;
        private GameObject _ball;
        public float Speed { get => _speed; set => _speed = value; }

        void Start()
        {
            _thornChallenge = new List<GameObject>();
            _thornOnLeftSide = new List<GameObject>();
            _thornOnRightSide = new List<GameObject>();

            _ceiling = new GameObject("Ceiling");
            var boxConlider = _ceiling.AddComponent<BoxCollider2D>();
            boxConlider.size = new Vector2(20f, .05f);
            _ceiling.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 0));

            _leftEdge = new GameObject("LeftEdge");
            var boxConliderLeft = _leftEdge.AddComponent<BoxCollider2D>();
            boxConliderLeft.size = new Vector2(.05f, 40f);
            _leftEdge.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));

            _rightEdge = new GameObject("RightEdge");
            var boxConliderRight = _rightEdge.AddComponent<BoxCollider2D>();
            boxConliderRight.size = new Vector2(.05f, 40f);
            _rightEdge.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));

            _ground = new GameObject("Ground");
            var boxConliderGround = _ground.AddComponent<BoxCollider2D>();
            boxConliderGround.size = new Vector2(20f, .05f);
            _ground.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));
            _ground.tag = TagManager.Die.Value();

            PrepareStart();
            StartCoroutine(CreateThornChallenge());

            GameManager.OnGameStart += OnStarted;
            GameManager.OnGameEnd += OnEnded;
            GameManager.OnGameRestart += OnTryAgain;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStart -= OnStarted;
            GameManager.OnGameEnd -= OnEnded;
            GameManager.OnGameRestart -= OnTryAgain;
        }

        void Update()
        {
            if(isStartGame && !isEndGame)
            {
                for (int i = 0; i < _thornChallenge.Count; i++)
                {
                    if (_thornChallenge[i])
                    {
                        _thornChallenge[i].transform.position += Vector3.up * Time.deltaTime * _speed;
                    }
                    else
                    {
                        _thornChallenge.RemoveAt(i);
                    }
                }

                for (int i = 0; i < _thornOnLeftSide.Count; i++)
                {
                    if (_thornOnLeftSide[i])
                    {
                        _thornOnLeftSide[i].transform.position += Vector3.up * Time.deltaTime * _speed;
                    }
                    else
                    {
                        _thornOnLeftSide.RemoveAt(i);
                    }
                }
                if (IsCreateNextLeft)
                {
                    CreateThornOnLeftSide();
                }

                for (int i = 0; i < _thornOnRightSide.Count; i++)
                {
                    if (_thornOnRightSide[i])
                    {
                        _thornOnRightSide[i].transform.position += Vector3.up * Time.deltaTime * _speed;
                    }
                    else
                    {
                        _thornOnRightSide.RemoveAt(i);
                    }
                }
                if (IsCreateNextRight)
                {
                    CreateThornOnRightSide();
                }
            }
        }

        private bool IsCreateNextLeft => Camera.main.WorldToScreenPoint(Vector3.up * (_thornOnLeftSide[_thornOnLeftSide.Count - 1].transform.position.y - _thornOnLeftSide[_thornOnLeftSide.Count - 1].GetComponent<SpriteRenderer>().bounds.extents.y)).y >= 0;
        private void CreateThornOnLeftSide()
        {
            var thorn = Instantiate(_thornPrefabs);
            thorn.name = "LeftSide";
            var spriteRender = thorn.GetComponent<SpriteRenderer>();
            spriteRender.sortingOrder = 500;
            var minPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
            thorn.transform.position = new Vector3(minPoint.x + spriteRender.bounds.extents.x, minPoint.y - spriteRender.bounds.extents.y);
            thorn.AddComponent<AutoDestroy>();
            _thornOnLeftSide.Add(thorn);
        }

        private bool IsCreateNextRight => Camera.main.WorldToScreenPoint(Vector3.up * (_thornOnRightSide[_thornOnRightSide.Count - 1].transform.position.y - _thornOnRightSide[_thornOnRightSide.Count - 1].GetComponent<SpriteRenderer>().bounds.extents.y)).y >= 0;
        private void CreateThornOnRightSide()
        {
            var thorn = Instantiate(_thornPrefabs);
            thorn.name = "RightSide";
            thorn.transform.localScale *= new Vector2(-1, 1);
            var spriteRender = thorn.GetComponent<SpriteRenderer>();
            spriteRender.sortingOrder = 500;
            var botRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
            thorn.transform.position = new Vector3(botRight.x - spriteRender.bounds.extents.x, botRight.y - spriteRender.bounds.extents.y, 0);
            thorn.AddComponent<AutoDestroy>();
            _thornOnRightSide.Add(thorn);
        }

        private IEnumerator CreateThornChallenge()
        {
            //_challenge.RandomChallenge(_thornChallenge, _thornPrefabs, Camera.main);
            while(true)
            {
                if(isStartGame && !isEndGame)
                {
                    _challenge.RandomChallenge(_thornChallenge, _thornPrefabs, Camera.main);
                }
                //else if(isEndGame)
                //{
                //    yield break;
                //}
                yield return new WaitForSeconds(3f);
            }
        }

        private void OnStarted()
        {
            isStartGame = true;
            //_ball.GetComponent<Rigidbody2D>().isKinematic = false;
            _ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnEnded()
        {
            isStartGame = false;
            isEndGame = true;
            _ball.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }

        private void OnTryAgain()
        {
            foreach (var thor in _thornChallenge.Where(i => i != null).ToList())
            {
                Destroy(thor.gameObject);
            }
            foreach(var thor in _thornOnLeftSide.Where(i => i != null).ToList())
            {
                Destroy(thor.gameObject);
            }
            foreach(var thor in _thornOnRightSide.Where(i => i != null).ToList())
            {
                Destroy(thor.gameObject);
            }
            _thornChallenge.Clear();
            _thornOnLeftSide.Clear();
            _thornOnRightSide.Clear();

            isEndGame = false;
            isStartGame = false;


            PrepareStart();
        }

        private void PrepareStart()
        {
            if(_ball == null)
            {
                _ball = Instantiate(_ballPrefabs);
            }
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.75f));
            _ball.transform.position = new Vector3(worldPosition.x, worldPosition.y, 0);
            _ball.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1000;
            _ball.GetComponent<Rigidbody2D>().isKinematic = true;

            CreateThornOnLeftSide();
            CreateThornOnRightSide();

            //StartCoroutine(CreateThornChallenge());
        }
    }
}
