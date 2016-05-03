using UnityEngine;
using System.Collections;

public class LevelEntrance : MonoBehaviour {
	public string destination;

    private GameSession session;

    void Awake()
    {
        session = FindObjectOfType<GameSession>();
    }

    public void Activate()
    {
        session.LoadLevel(destination, false);
    }
}
