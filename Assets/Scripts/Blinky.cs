namespace Assets.Scripts
{
    public class Blinky : Ghost
    {
        private new void Start()
        {
            base.Start();
            StartCoroutine(nameof(TimeIsRunningOut));
        }

        public override void Chase()
        {
            if (PacmanGameObject != null)
            {
                MoveToTarget(PacmanGameObject.transform.position);
            }
        }
    }
}