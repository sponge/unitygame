using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

    private static List<string> levels = new List<string>() {
		"overworld",
        "blockout",
        "smw",
        "testlevel"
    };

    public void StartClick()
    {
        var sess = FindObjectOfType<GameSession>();
        var dropdown = GetComponentInChildren<Dropdown>();
        sess.LoadLevel(levels[dropdown.value], false);
    }

    // Use this for initialization
    void Start () {
        var dropdown = GetComponentInChildren<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(levels);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
