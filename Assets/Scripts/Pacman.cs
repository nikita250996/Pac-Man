using UnityEngine;

namespace Assets.Scripts
{
    public class Pacman : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Vector2 _destination = Vector2.zero;

        public enum Directions
        {
            Up,
            Right,
            Down,
            Left
        }

        public Directions Direction = Directions.Right;

        private void Start()
        {
            _destination = transform.position;
        }

        private void FixedUpdate()
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
            GetComponent<Rigidbody2D>().MovePosition(targetPosition);

            if ((Vector2) transform.position == _destination)
            {
                if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
                {
                    _destination = (Vector2) transform.position + Vector2.up;
                    Direction = Directions.Up;
                }

                if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
                {
                    _destination = (Vector2) transform.position + Vector2.right;
                    Direction = Directions.Right;
                }

                if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(-Vector2.up))
                {
                    _destination = (Vector2) transform.position - Vector2.up;
                    Direction = Directions.Down;
                }

                if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(-Vector2.right))
                {
                    _destination = (Vector2) transform.position - Vector2.right;
                    Direction = Directions.Left;
                }
            }

            Vector2 direction = _destination - (Vector2) transform.position;
            GetComponent<Animator>().SetFloat("DirX", direction.x);
            GetComponent<Animator>().SetFloat("DirY", direction.y);
        }

        private bool Valid(Vector2 direction)
        {
            Vector2 position = transform.position;
            RaycastHit2D hit = Physics2D.Linecast(position + direction, position);
            return hit.collider == GetComponent<Collider2D>();
        }
    }
}