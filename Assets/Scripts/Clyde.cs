namespace Assets.Scripts
{
    public class Clyde : Ghost
    {
        private new void Start()
        {
            base.Start();

            gameObject.SetActive(false);
            Invoke(nameof(SetActive), 12);
        }

        private void SetActive()
        {
            gameObject.SetActive(true);
            StartCoroutine(nameof(TimeIsRunningOut));
        }

        public override void Chase()
        {
            if (PacmanGameObject == null)
            {
                return;
            }

            float distance = DistanceToTarget(transform.position, PacmanGameObject.transform.position);
            if (distance >= 8)
            {
                MoveToTarget(PacmanGameObject.transform.position);
            }
            else
            {
                Scatter();
            }
        }
    }
}