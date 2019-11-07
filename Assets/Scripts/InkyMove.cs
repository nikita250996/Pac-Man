using System;
using UnityEngine;

public class InkyMove : GhostMove
{
    private new void Start()
    {
        gameObject.SetActive(false);
        Invoke("SetActive", 8);
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
        Vector3 intermediatePoint;
        switch (Pacman.GetComponent<PacmanMove>().Direction)
        {
            case PacmanMove.Directions.Up:
                intermediatePoint = Pacman.transform.position + new Vector3(0, 2, 0);
                break;
            case PacmanMove.Directions.Right:
                intermediatePoint = Pacman.transform.position + new Vector3(2, 0, 0);
                break;
            case PacmanMove.Directions.Down:
                intermediatePoint = Pacman.transform.position + new Vector3(0, -2, 0);
                break;
            case PacmanMove.Directions.Left:
                intermediatePoint = Pacman.transform.position + new Vector3(-2, 0, 0);
                break;
            default:
                intermediatePoint = Pacman.transform.position;
                break;
        }

        var intermediateVector = intermediatePoint - GameObject.Find("blinky").transform.position;
        MoveToTarget(2 * new Vector3((float) Math.Round(intermediateVector.x), (float) Math.Round(intermediateVector.y),
                         (float) Math.Round(intermediateVector.z)));
    }
}