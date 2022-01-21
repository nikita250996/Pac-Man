using UnityEngine;

namespace Assets.Scripts
{
    public class Pinky : Ghost
    {
        private new void Start()
        {
            base.Start();

            gameObject.SetActive(false);
            Invoke(nameof(SetActive), 4);
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

            switch (PacmanGameObject.GetComponent<Pacman>().Direction)
            {
                case Pacman.Directions.Up:
                    MoveToTarget(PacmanGameObject.transform.position + new Vector3(0, 3, 0));
                    break;
                case Pacman.Directions.Right:
                    MoveToTarget(PacmanGameObject.transform.position + new Vector3(3, 0, 0));
                    break;
                case Pacman.Directions.Down:
                    MoveToTarget(PacmanGameObject.transform.position + new Vector3(0, -3, 0));
                    break;
                case Pacman.Directions.Left:
                    MoveToTarget(PacmanGameObject.transform.position + new Vector3(-3, 0, 0));
                    break;
            }
        }
    }
}