using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Highscore : MonoBehaviour
    {
        private int _score;
        private int _energizersCount;
        private int _pacdotsCount;

        [SerializeField] private Text _highscoreText;

        [SerializeField] private AudioSource chompSound;
        [SerializeField] private AudioSource winSound;

        private bool _win;

        private void Start()
        {
            _score = 0;
            _energizersCount = GameObject.FindGameObjectsWithTag("energizer").Length;
            _pacdotsCount = GameObject.FindGameObjectsWithTag("pacdot").Length - _energizersCount;

            _highscoreText.text = "HIGH SCORE " + _score;

            _win = false;
        }

        private void Update()
        {
            if (_win)
            {
                return;
            }

            if (_energizersCount + _pacdotsCount > 0)
            {
                return;
            }

            _highscoreText.text = "YOU WIN!";
            _win = true;
            winSound.Play();
            Time.timeScale = 0;
        }

        public void AddScore(int score)
        {
            if (score == 50)
            {
                --_energizersCount;
            }
            else
            {
                --_pacdotsCount;
            }

            _score += score;
            _highscoreText.text = "HIGH SCORE " + _score;

            PlayChomp();
        }

        private void PlayChomp()
        {
            if (!chompSound.isPlaying)
            {
                chompSound.Play();
            }
        }
    }
}