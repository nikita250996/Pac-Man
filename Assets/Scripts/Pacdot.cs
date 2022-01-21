using UnityEngine;

namespace Assets.Scripts
{
    public class Pacdot : MonoBehaviour
    {
        private Highscore _highscore;

        private void Start()
        {
            _highscore = GameObject.Find("highscore").GetComponent<Highscore>();
            Invoke(nameof(CheckEnergizers), 0.1f);
        }

        private void OnTriggerEnter2D(Object component)
        {
            if (component.name != "pacman")
            {
                return;
            }

            Destroy(gameObject);
            _highscore.AddScore(10);
        }

        private void CheckEnergizers()
        {
            GameObject[] energizers = GameObject.FindGameObjectsWithTag("energizer");
            foreach (GameObject energizer in energizers)
            {
                if (energizer.transform.position == transform.position)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}