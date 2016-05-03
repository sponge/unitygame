using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

    private string overworldScene = "game_launch";

    void Start()
    {
        var alreadyExists = FindObjectsOfType<GameSession>();
        if (alreadyExists.Length > 1)
        {
            DestroyImmediate(gameObject);
        }

        DontDestroyOnLoad(this);
    }

    void Update()
    {

    }

    public void LoadLevel(string scene, bool isWorld)
    {
        var sceneName = "Levels/" + scene;
        SceneManager.LoadScene(sceneName);

        if (isWorld)
        {
            overworldScene = sceneName;
        }
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene(overworldScene);
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
