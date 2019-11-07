using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    private int _score;
    private int _dotsEaten;
    private int _energizersEaten;
    public Text HighscoreText;
    public AudioSource Chomp;
    public AudioSource Win;
    private bool _win;

    private void Start()
    {
        _score = 0;
        _dotsEaten = 0;
        _energizersEaten = 0;
        HighscoreText.text = "HIGH SCORE " + _score;
        _win = false;
    }

    private void Update()
    {
        _score += _dotsEaten * 10 + _energizersEaten * 50;
        if (GameObject.FindGameObjectsWithTag("energizer").Length +
            GameObject.FindGameObjectsWithTag("pacdot").Length == 0)
        {
            HighscoreText.text = "YOU WIN!";
            if (_win) return;
            _win = true;
            Win.Play();
        }
        else
        {
            _dotsEaten = 0;
            _energizersEaten = 0;
            HighscoreText.text = "HIGH SCORE " + _score;
        }
    }

    public void DotEaten()
    {
        _dotsEaten += 1;
        PlayChomp();
    }

    public void EnergizerEaten()
    {
        _energizersEaten += 1;
        PlayChomp();
    }

    private void PlayChomp()
    {
        if (!Chomp.isPlaying)
        {
            Chomp.Play();
        }
    }
}