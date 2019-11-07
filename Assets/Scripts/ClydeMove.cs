using UnityEngine;

public class ClydeMove : GhostMove
{
    private new void Start()
    {
        gameObject.SetActive(false);
        Invoke("SetActive", 12);
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
        var distance = DistanceToTarget(transform.position, Pacman.transform.position);
        if (distance >= 8)
        {
            MoveToTarget(Pacman.transform.position);
        }
        else
        {
            Scatter();
        }
    }
}