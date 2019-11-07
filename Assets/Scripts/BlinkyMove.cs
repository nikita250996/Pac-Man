using UnityEngine;

public class BlinkyMove : GhostMove
{
    private new void Start()
    {
        base.Start();
        Direction = new Vector2(0, 0);
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Chase()
    {
        MoveToTarget(Pacman.transform.position);
    }
}