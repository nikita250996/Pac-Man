using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    public float Speed;
    public Vector2 Destination = Vector2.zero;

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
        Destination = transform.position;
    }

    private void FixedUpdate()
    {
        var targetPosition = Vector2.MoveTowards(transform.position, Destination, Speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(targetPosition);

        if ((Vector2) transform.position == Destination)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Valid(Vector2.up))
            {
                Destination = (Vector2) transform.position + Vector2.up;
                Direction = Directions.Up;
            }

            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && Valid(Vector2.right))
            {
                Destination = (Vector2) transform.position + Vector2.right;
                Direction = Directions.Right;
            }

            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && Valid(-Vector2.up))
            {
                Destination = (Vector2) transform.position - Vector2.up;
                Direction = Directions.Down;
            }

            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && Valid(-Vector2.right))
            {
                Destination = (Vector2) transform.position - Vector2.right;
                Direction = Directions.Left;
            }
        }

        var direction = Destination - (Vector2) transform.position;
        GetComponent<Animator>().SetFloat("DirX", direction.x);
        GetComponent<Animator>().SetFloat("DirY", direction.y);
    }

    private bool Valid(Vector2 direction)
    {
        Vector2 position = transform.position;
        var hit = Physics2D.Linecast(position + direction, position);
        return hit.collider == GetComponent<Collider2D>();
    }
}