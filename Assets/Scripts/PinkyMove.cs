using UnityEngine;

public class PinkyMove : GhostMove
{
    private new void Start()
    {
        gameObject.SetActive(false);
        Invoke("SetActive", 4);
        base.Start();
        Direction = new Vector2(0, 0);
    }

    private void SetActive()
    {
        gameObject.SetActive(true);
        StartCoroutine("TimeIsRunningOut");
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Chase()
    {
        switch (Pacman.GetComponent<PacmanMove>().Direction)
        {
            case PacmanMove.Directions.Up:
                MoveToTarget(Pacman.transform.position + new Vector3(0, 3, 0));
                break;
            case PacmanMove.Directions.Right:
                MoveToTarget(Pacman.transform.position + new Vector3(3, 0, 0));
                break;
            case PacmanMove.Directions.Down:
                MoveToTarget(Pacman.transform.position + new Vector3(0, -3, 0));
                break;
            case PacmanMove.Directions.Left:
                MoveToTarget(Pacman.transform.position + new Vector3(-3, 0, 0));
                break;
        }
    }
}
