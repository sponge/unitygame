using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    private static List<string> levels = new List<string>() {
        "blockout",
        "smw",
        "testlevel"
    };

    public void OverworldClick() {
        var sess = FindObjectOfType<GameSession>();
        sess.LoadLevel("overworld", true);
    }

    public void StartClick() {
        var sess = FindObjectOfType<GameSession>();
        var dropdown = GetComponentInChildren<Dropdown>();
        sess.LoadLevel(levels[dropdown.value], false);
    }

    // Use this for initialization
    private void Start() {
        var dropdown = GetComponentInChildren<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(levels);
    }

    // Update is called once per frame
    private void Update() {
    }
}