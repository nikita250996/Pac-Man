using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

namespace Assets.Scripts
{
    public class Ghost : MonoBehaviour
    {
        [SerializeField] private Transform[] _waypoints;

        private const int StartWaypoint = 0;
        internal int HomeCornerWaypoint = 1;

        [SerializeField] private AudioSource _pacmanDeath;

        internal GameObject PacmanGameObject;

        [SerializeField] private float _speed;
        public float Timer;

        private Vector2 _destination = Vector2.zero;

        private GameObject[] _intersections;
        private GameObject[] _energizers;
        private GameObject[] _pacdots;

        private GameObject _nextIntersection;

        private List<KeyValuePair<GameObject, string>> _possibleMoves;

        private string _previousDirection;

        private Sprite _normalSprite;

        [SerializeField] private Sprite _fright;

        internal enum States
        {
            Start,
            Chase,
            Scatter,
            Frightened
        }

        internal States State;

        internal void Start()
        {
            PacmanGameObject = GameObject.Find("pacman");
            State = States.Start;
            _destination = _waypoints[StartWaypoint].position;
            _normalSprite = GetComponent<SpriteRenderer>().sprite;
            _intersections = GameObject.FindGameObjectsWithTag("intersection");
            _possibleMoves = new List<KeyValuePair<GameObject, string>>();
        }

        internal IEnumerator TimeIsRunningOut()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                --Timer;
            }
        }

        private void FixedUpdate()
        {
            _energizers = GameObject.FindGameObjectsWithTag("energizer");
            _pacdots = GameObject.FindGameObjectsWithTag("pacdot");

            switch (State)
            {
                case States.Start:
                    Vector2 targetPosition =
                        Vector2.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
                    GetComponent<Rigidbody2D>().MovePosition(targetPosition);
                    if ((Vector2) transform.position == _destination)
                    {
                        Timer = 20;
                        State = States.Chase;
                    }

                    break;
                case States.Chase:
                    if (Timer <= 0)
                    {
                        Timer = 7;
                        State = States.Scatter;
                    }
                    else
                    {
                        Chase();
                    }

                    break;
                case States.Scatter:
                    if (Timer <= 0)
                    {
                        Timer = 20;
                        State = States.Chase;
                    }
                    else
                    {
                        Scatter();
                    }

                    break;
                case States.Frightened:
                    _speed = 2.5f;
                    if (Timer <= 0)
                    {
                        GetComponent<SpriteRenderer>().sprite = _normalSprite;
                        GetComponent<Animator>().enabled = true;
                        _speed = 5;
                        if (new Random().Next(2) == 1)
                        {
                            State = States.Chase;
                            Timer = 20;
                        }
                        else
                        {
                            State = States.Scatter;
                            Timer = 7;
                        }
                    }
                    else
                    {
                        Frightened();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Vector2 direction = _destination - (Vector2) transform.position;
            GetComponent<Animator>().SetFloat("DirX", direction.x);
            GetComponent<Animator>().SetFloat("DirY", direction.y);
        }

        private void OnTriggerEnter2D(Component component)
        {
            if (component.name != "pacman" || State == States.Frightened)
            {
                return;
            }

            Destroy(component.gameObject);
            _pacmanDeath.Play();
            Invoke(nameof(Restart), _pacmanDeath.clip.length);
        }

        private void Restart()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public virtual void Chase()
        {
            /*
           CHASE—A ghost's objective in chase mode is to find and capture Pac-Man by hunting him down
           through the maze. Each ghost exhibits unique behavior when chasing Pac-Man, giving them their
           different personalities: Blinky (red) is very aggressive and hard to shake once he gets behind you,
           Pinky (pink) tends to get in front of you and cut you off, Inky (light blue) is the least predictable of
           the bunch, and Clyde (orange) seems to do his own thing and stay out of the way.
         */
        }

        public virtual void Scatter()
        {
            /*
           SCATTER—In scatter mode, the ghosts give up the chase for a few seconds and head for their
           respective home corners. It is a welcome but brief rest—soon enough, they will revert to chase
           mode and be after Pac-Man again.
         */
            MoveToTarget(_waypoints[HomeCornerWaypoint].transform.position);
        }

        internal void MoveToTarget(Vector3 target)
        {
            //целевая позиция — назначение
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
            //двигаемся к целевой позиции
            GetComponent<Rigidbody2D>().MovePosition(targetPosition);

            //пришли в целевую позицию?
            if ((Vector2) transform.position != _destination)
            {
                return;
            }

            KeyValuePair<bool, Vector2> up = new KeyValuePair<bool, Vector2>(Valid(Vector2.up),
                new Vector2(transform.position.x, transform.position.y + 1));
            KeyValuePair<bool, Vector2> right = new KeyValuePair<bool, Vector2>(Valid(Vector2.right),
                new Vector2(transform.position.x + 1, transform.position.y));
            KeyValuePair<bool, Vector2> down = new KeyValuePair<bool, Vector2>(Valid(-Vector2.up),
                new Vector2(transform.position.x, transform.position.y - 1));
            KeyValuePair<bool, Vector2> left = new KeyValuePair<bool, Vector2>(Valid(-Vector2.right),
                new Vector2(transform.position.x - 1, transform.position.y));
            switch (_previousDirection)
            {
                case "down":
                    up = new KeyValuePair<bool, Vector2>(false, new Vector2());
                    break;
                case "left":
                    right = new KeyValuePair<bool, Vector2>(false, new Vector2());
                    break;
                case "up":
                    down = new KeyValuePair<bool, Vector2>(false, new Vector2());
                    break;
                case "right":
                    left = new KeyValuePair<bool, Vector2>(false, new Vector2());
                    break;
            }

            GameObject leftIntersection = null;
            GameObject rightIntersection = null;
            GameObject downIntersection = null;
            GameObject upIntersection = null;

            //можно сходить влево?
            if (left.Key)
            {
                //для всех перекрёстков
                foreach (GameObject intersection in _intersections)
                {
                    //если игрек отличается
                    if (Math.Abs(transform.position.y - intersection.transform.position.y) > 0.1f ||
                        //или перекрёсток справа
                        intersection.transform.position.x > transform.position.x ||
                        //или призрак уже на перекрёстке
                        transform.position == intersection.transform.position)
                    {
                        continue;
                    }

                    //если ближийший перекрёсток слева вообще не выбран
                    if (leftIntersection == null)
                    {
                        //то это первый попавшийся
                        leftIntersection = intersection;
                    }

                    //если расстояние от перекрёстка до призрака меньше
                    if (Math.Abs(intersection.transform.position.x - transform.position.x) <
                        //расстояния от ближайшего перекрёстка до призрака
                        Math.Abs(leftIntersection.transform.position.x - transform.position.x))
                    {
                        leftIntersection = intersection;
                    }
                }

                if (leftIntersection != null)
                {
                    _possibleMoves.Add(new KeyValuePair<GameObject, string>(leftIntersection, "left"));
                }
            }

            //можно сходить вправо?
            if (right.Key)
            {
                //для всех перекрёстков
                foreach (GameObject intersection in _intersections)
                {
                    //если игрек отличается или перекрёсток слева, то даже не рассматриваем
                    if (Math.Abs(transform.position.y - intersection.transform.position.y) > 0.1f ||
                        intersection.transform.position.x < transform.position.x ||
                        //или призрак уже на перекрёстке
                        transform.position == intersection.transform.position)
                    {
                        continue;
                    }

                    //если ближийший перекрёсток справа вообще не выбран
                    if (rightIntersection != null)
                    {
                    }
                    else
                    {
                        //то это первый попавшийся
                        rightIntersection = intersection;
                    }

                    //если расстояние от перекрёстка до призрака меньше
                    if (Math.Abs(intersection.transform.position.x - transform.position.x) <
                        //расстояния от ближайшего перекрёстка до призрака
                        Math.Abs(rightIntersection.transform.position.x - transform.position.x))
                    {
                        rightIntersection = intersection;
                    }
                }

                if (rightIntersection != null)
                {
                    _possibleMoves.Add(new KeyValuePair<GameObject, string>(rightIntersection, "right"));
                }
            }

            //можно сходить вниз?
            if (down.Key)
            {
                //для всех перекрёстков
                foreach (GameObject intersection in _intersections)
                {
                    //если икс отличается или перекрёсток сверху, то даже не рассматриваем
                    if (Math.Abs(transform.position.x - intersection.transform.position.x) > 0.1f ||
                        intersection.transform.position.y > transform.position.y ||
                        //или призрак уже на перекрёстке
                        transform.position == intersection.transform.position)
                    {
                        continue;
                    }

                    //если ближийший перекрёсток снизу вообще не выбран
                    if (downIntersection == null)
                    {
                        //то это первый попавшийся
                        downIntersection = intersection;
                    }

                    //если расстояние от перекрёстка до призрака меньше
                    if (Math.Abs(intersection.transform.position.y - transform.position.y) <
                        //расстояния от ближайшего перекрёстка до призрака
                        Math.Abs(downIntersection.transform.position.y - transform.position.y))
                    {
                        downIntersection = intersection;
                    }
                }

                if (downIntersection != null)
                {
                    _possibleMoves.Add(new KeyValuePair<GameObject, string>(downIntersection, "down"));
                }
            }

            //можно сходить вверх?
            if (up.Key)
            {
                //для всех перекрёстков
                foreach (GameObject intersection in _intersections)
                {
                    //если икс отличается или перекрёсток снизу, то даже не рассматриваем
                    if (Math.Abs(transform.position.x - intersection.transform.position.x) > 0.1f ||
                        intersection.transform.position.y < transform.position.y ||
                        //или призрак уже на перекрёстке
                        transform.position == intersection.transform.position) continue;
                    //если ближийший перекрёсток сверху вообще не выбран
                    if (upIntersection == null)
                    {
                        //то это первый попавшийся
                        upIntersection = intersection;
                    }

                    //если расстояние от перекрёстка до призрака меньше
                    if (Math.Abs(intersection.transform.position.y - transform.position.y) <
                        //расстояния от ближайшего перекрёстка до призрака
                        Math.Abs(upIntersection.transform.position.y - transform.position.y))
                    {
                        upIntersection = intersection;
                    }
                }

                if (upIntersection != null)
                {
                    _possibleMoves.Add(new KeyValuePair<GameObject, string>(upIntersection, "up"));
                }
            }

            foreach (KeyValuePair<GameObject, string> intersection in _possibleMoves)
            {
                if (_nextIntersection == null)
                {
                    _nextIntersection = intersection.Key;
                    _previousDirection = intersection.Value;
                }

                if (!(DistanceToTarget(intersection.Key.transform.position, target) <
                      DistanceToTarget(_nextIntersection.transform.position, target)))
                {
                    continue;
                }

                _nextIntersection = intersection.Key;
                _previousDirection = intersection.Value;
            }

            if (_nextIntersection != null)
            {
                _destination = _nextIntersection.transform.position;
                _nextIntersection = null;
            }

            _possibleMoves.Clear();
        }

        private bool Valid(Vector2 direction)
        {
            Vector2 position = transform.position;
            RaycastHit2D hit = Physics2D.Linecast(position + direction, position);

            if (_energizers.Any(energizer => hit.collider == energizer.GetComponent<Collider2D>()))
            {
                return true;
            }

            if (_pacdots.Any(pacdot => hit.collider == pacdot.GetComponent<Collider2D>()))
            {
                return true;
            }

            return hit.collider == GetComponent<Collider2D>();
        }

        internal float DistanceToTarget(Vector2 position, Vector3 target)
        {
            return Math.Abs(position.x - target.x) + Math.Abs(position.y - target.y);
        }

        public virtual void Frightened()
        {
            /*
           FRIGHTENED—Ghosts enter frightened mode whenever Pac-Man eats one of the four energizers
           located in the far corners of the maze. During the early levels, the ghosts will all turn dark blue
           (meaning they are vulnerable) and aimlessly wander the maze for a few seconds. They will flash
           moments before returning to their previous mode of behavior.
         */
            MoveToTarget(_intersections[new Random().Next(_intersections.Length)].transform.position);
        }

        public void TurnFrightened()
        {
            State = States.Frightened;
            Timer = 6;
            GetComponent<SpriteRenderer>().sprite = _fright;
            GetComponent<Animator>().enabled = false;
        }
    }
}