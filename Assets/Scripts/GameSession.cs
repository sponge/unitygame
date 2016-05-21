using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

    public string overworldScene = "game_launch";
    public int levelCompleteBit;
    public int currentLevelBit;

    public Vector2 overworldPosition;
    public bool useSessionPosition;
    public Inventory.Items inventoryItems;

    void Start()
    {
        var alreadyExists = FindObjectsOfType<GameSession>();
        if (alreadyExists.Length > 1)
        {
            DestroyImmediate(gameObject);
            return;
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

    public void WinLevel()
    {
        StartCoroutine(EndLevelCoroutine());
        levelCompleteBit += currentLevelBit;

        // FIXME: hardcoded for one player: save inventory out to global inventory
        var player = FindObjectOfType<PlayerController>();

        var inv = player.GetComponent<Inventory>();
        inv.ResetTemporaryItems();
        inventoryItems = inv.items;
    }

    private IEnumerator EndLevelCoroutine() {
        yield return new WaitForSeconds(5);
        ExitLevel();
    }

}
