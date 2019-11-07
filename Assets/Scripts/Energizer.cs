using UnityEngine;

public class Energizer : MonoBehaviour
{
    private GameObject _highscoreGameObject;
    private Highscore _highscore;

    private void Start()
    {
        _highscoreGameObject = GameObject.Find("highscore");
        _highscore = _highscoreGameObject.GetComponent<Highscore>();
    }

    private void OnTriggerEnter2D(Object component)
    {
        if (component.name != "pacman") return;
        Destroy(gameObject);
        _highscore.EnergizerEaten();
        var ghosts = FindObjectsOfType<GhostMove>();
        foreach (var ghost in ghosts)
        {
            ghost.State = GhostMove.States.Frightened;
            ghost.Timer = 6;
            ghost.GetComponent<SpriteRenderer>().sprite = ghost.Fright;
            ghost.GetComponent<Animator>().enabled = false;
        }
    }
}