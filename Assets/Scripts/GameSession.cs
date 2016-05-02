using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

    void Start()
    {
        var alreadyExists = FindObjectsOfType<GameSession>();
        if (alreadyExists.Length > 1)
        {
            DestroyImmediate(this);
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {

    }

    public void LoadLevel(string scene, bool isWorld)
    {
        SceneManager.LoadScene("Levels/" + scene);
    }

    public void ExitLevel()
    {
        // FIXME: if we have a world map, go back to the world map
        SceneManager.LoadScene("game_launch");
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCoroutine());
    }

    private IEnumerator EndLevelCoroutine() {
        yield return new WaitForSeconds(5);
        ExitLevel();
    }

}
