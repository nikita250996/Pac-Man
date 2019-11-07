using UnityEngine;

public class Pacdot : MonoBehaviour
{
    GameObject _highscoreGameObject;
    private Highscore _highscore;

    private void Start()
    {
        _highscoreGameObject = GameObject.Find("highscore");
        _highscore = _highscoreGameObject.GetComponent<Highscore>();
        Invoke("CheckEnergizers", 0.1f);
    }

    private void OnTriggerEnter2D(Object component)
    {
        if (component.name != "pacman") return;
        Destroy(gameObject);
        _highscore.DotEaten();
    }

    private void CheckEnergizers()
    {
        var energizers = GameObject.FindGameObjectsWithTag("energizer");
        foreach (var energizer in energizers)
        {
            if (energizer.transform.position == transform.position)
            {
                Destroy(gameObject);
            }
        }
    }
}