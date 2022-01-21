using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Inky : Ghost
    {
        [SerializeField] private GameObject _blinky;

        private new void Start()
        {
            base.Start();
            gameObject.SetActive(false);
            Invoke(nameof(SetActive), 8);
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

            Vector3 intermediatePoint;
            switch (PacmanGameObject.GetComponent<Pacman>().Direction)
            {
                case Pacman.Directions.Up:
                    intermediatePoint = PacmanGameObject.transform.position + new Vector3(0, 2, 0);
                    break;
                case Pacman.Directions.Right:
                    intermediatePoint = PacmanGameObject.transform.position + new Vector3(2, 0, 0);
                    break;
                case Pacman.Directions.Down:
                    intermediatePoint = PacmanGameObject.transform.position + new Vector3(0, -2, 0);
                    break;
                case Pacman.Directions.Left:
                    intermediatePoint = PacmanGameObject.transform.position + new Vector3(-2, 0, 0);
                    break;
                default:
                    intermediatePoint = PacmanGameObject.transform.position;
                    break;
            }

            Vector3 intermediateVector = intermediatePoint - _blinky.transform.position;
            MoveToTarget(2 * new Vector3((float) Math.Round(intermediateVector.x),
                (float) Math.Round(intermediateVector.y),
                (float) Math.Round(intermediateVector.z)));
        }
    }
}