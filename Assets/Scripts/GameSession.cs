using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Levels/blockout");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
