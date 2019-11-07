using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GhostMove : MonoBehaviour
{
    public Transform[] Waypoints;
    private const int StartWaypoint = 0;
    internal int HomeCornerWaypoint = 1;
    public AudioSource PacmanDeath;
    internal GameObject Pacman;
    internal CircleCollider2D CircleCollider2D;
    public float Speed;
    public float Timer;
    internal Vector2 Destination = Vector2.zero;
    internal GameObject[] Intersections;
    internal GameObject[] Energizers;
    internal GameObject[] Pacdots;
    internal Vector2 Direction;
    internal GameObject NextIntersection;
    internal List<KeyValuePair<GameObject, string>> PossibleMoves;
    internal string PreviousDirection;
    private Sprite _normalSprite;
    public Sprite Fright;

    public enum States
    {
        Start,
        Chase,
        Scatter,
        Frightened
    }

    public States State;

    internal void Start()
    {
        Pacman = GameObject.Find("pacman");
        State = States.Start;
        Destination = Waypoints[StartWaypoint].position;
        CircleCollider2D = GetComponent<CircleCollider2D>();
        _normalSprite = GetComponent<SpriteRenderer>().sprite;
        Intersections = GameObject.FindGameObjectsWithTag("intersection");
        Time.timeScale = 1;
        PossibleMoves = new List<KeyValuePair<GameObject, string>>();
        StartCoroutine("TimeIsRunningOut");
    }

    private IEnumerator TimeIsRunningOut()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            --Timer;
        }
    }

    internal void FixedUpdate()
    {
        Energizers = GameObject.FindGameObjectsWithTag("energizer");
        Pacdots = GameObject.FindGameObjectsWithTag("pacdot");

        switch (State)
        {
            case States.Start:
                var targetPosition = Vector2.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
                GetComponent<Rigidbody2D>().MovePosition(targetPosition);
                if ((Vector2) transform.position == Destination)
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
                    break;
                }

                Chase();
                break;
            case States.Scatter:
                if (Timer <= 0)
                {
                    Timer = 20;
                    State = States.Chase;
                    break;
                }

                Scatter();
                break;
            case States.Frightened:
                Speed = 2.5f;
                if (Timer <= 0)
                {
                    GetComponent<SpriteRenderer>().sprite = _normalSprite;
                    GetComponent<Animator>().enabled = true;
                    Speed = 5;
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

                    break;
                }

                Frightened();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var direction = Destination - (Vector2) transform.position;
        GetComponent<Animator>().SetFloat("DirX", direction.x);
        GetComponent<Animator>().SetFloat("DirY", direction.y);
    }

    private void OnTriggerEnter2D(Component component)
    {
        if (component.name != "pacman" || State == States.Frightened) return;
        Destroy(component.gameObject);
        PacmanDeath.Play();
        Invoke("Restart", PacmanDeath.clip.length);
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
        MoveToTarget(Waypoints[HomeCornerWaypoint].transform.position);
    }

    public void MoveToTarget(Vector3 target)
    {
        //целевая позиция — назначение
        var targetPosition = Vector2.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
        //двигаемся к целевой позиции
        GetComponent<Rigidbody2D>().MovePosition(targetPosition);

        //пришли в целевую позицию?
        if ((Vector2) transform.position != Destination) return;
        var up = new KeyValuePair<bool, Vector2>(Valid(Vector2.up),
            new Vector2(transform.position.x, transform.position.y + 1));
        var right = new KeyValuePair<bool, Vector2>(Valid(Vector2.right),
            new Vector2(transform.position.x + 1, transform.position.y));
        var down = new KeyValuePair<bool, Vector2>(Valid(-Vector2.up),
            new Vector2(transform.position.x, transform.position.y - 1));
        var left = new KeyValuePair<bool, Vector2>(Valid(-Vector2.right),
            new Vector2(transform.position.x - 1, transform.position.y));
        switch (PreviousDirection)
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
            foreach (var intersection in Intersections)
            {
                //если игрек отличается
                if (Math.Abs(transform.position.y - intersection.transform.position.y) > 0.1f ||
                    //или перекрёсток справа
                    intersection.transform.position.x > transform.position.x ||
                    //или призрак уже на перекрёстке
                    transform.position == intersection.transform.position) continue;
                //если ближийший перекрёсток слева вообще не выбран
                if (leftIntersection is null)
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
                PossibleMoves.Add(new KeyValuePair<GameObject, string>(leftIntersection, "left"));
            }
        }

        //можно сходить вправо?
        if (right.Key)
        {
            //для всех перекрёстков
            foreach (var intersection in Intersections)
            {
                //если игрек отличается или перекрёсток слева, то даже не рассматриваем
                if (Math.Abs(transform.position.y - intersection.transform.position.y) > 0.1f ||
                    intersection.transform.position.x < transform.position.x ||
                    //или призрак уже на перекрёстке
                    transform.position == intersection.transform.position) continue;
                //если ближийший перекрёсток справа вообще не выбран
                if (rightIntersection is null)
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
                PossibleMoves.Add(new KeyValuePair<GameObject, string>(rightIntersection, "right"));
            }
        }

        //можно сходить вниз?
        if (down.Key)
        {
            //для всех перекрёстков
            foreach (var intersection in Intersections)
            {
                //если икс отличается или перекрёсток сверху, то даже не рассматриваем
                if (Math.Abs(transform.position.x - intersection.transform.position.x) > 0.1f ||
                    intersection.transform.position.y > transform.position.y ||
                    //или призрак уже на перекрёстке
                    transform.position == intersection.transform.position) continue;
                //если ближийший перекрёсток снизу вообще не выбран
                if (downIntersection is null)
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
                PossibleMoves.Add(new KeyValuePair<GameObject, string>(downIntersection, "down"));
            }
        }

        //можно сходить вверх?
        if (up.Key)
        {
            //для всех перекрёстков
            foreach (var intersection in Intersections)
            {
                //если икс отличается или перекрёсток снизу, то даже не рассматриваем
                if (Math.Abs(transform.position.x - intersection.transform.position.x) > 0.1f ||
                    intersection.transform.position.y < transform.position.y ||
                    //или призрак уже на перекрёстке
                    transform.position == intersection.transform.position) continue;
                //если ближийший перекрёсток сверху вообще не выбран
                if (upIntersection is null)
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
                PossibleMoves.Add(new KeyValuePair<GameObject, string>(upIntersection, "up"));
            }
        }

        foreach (var intersection in PossibleMoves)
        {
            if (NextIntersection is null)
            {
                NextIntersection = intersection.Key;
                PreviousDirection = intersection.Value;
            }

            if (!(DistanceToTarget(intersection.Key.transform.position, target) <
                  DistanceToTarget(NextIntersection.transform.position, target))) continue;
            NextIntersection = intersection.Key;
            PreviousDirection = intersection.Value;
        }

        if (NextIntersection != null)
        {
            Destination = NextIntersection.transform.position;
            NextIntersection = null;
        }

        PossibleMoves.Clear();
    }

    private bool Valid(Vector2 direction)
    {
        Vector2 position = transform.position;
        var hit = Physics2D.Linecast(position + direction, position);
        if (Energizers.Any(energizer => hit.collider == energizer.GetComponent<Collider2D>()))
        {
            return true;
        }

        if (Pacdots.Any(pacdot => hit.collider == pacdot.GetComponent<Collider2D>()))
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
        MoveToTarget(Intersections[new Random().Next(Intersections.Length)].transform.position);
    }
}