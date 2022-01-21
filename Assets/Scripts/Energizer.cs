using UnityEngine;

namespace Assets.Scripts
{
    public class Energizer : MonoBehaviour
    {
        private Highscore _highscore;

        [SerializeField] private Ghost[] _ghosts;

        private void Start()
        {
            _highscore = GameObject.Find("highscore").GetComponent<Highscore>();
        }

        private void OnTriggerEnter2D(Object component)
        {
            if (component.name != "pacman")
            {
                return;
            }

            Destroy(gameObject);

            _highscore.AddScore(50);

            foreach (Ghost ghost in _ghosts)
            {
                if (ghost.gameObject.activeInHierarchy)
                {
                    ghost.TurnFrightened();
                }
            }
        }
    }
}