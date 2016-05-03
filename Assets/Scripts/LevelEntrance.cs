using UnityEngine;
using System.Collections;

public class LevelEntrance : MonoBehaviour {
	public string destination;
    public int levelBit;

    private GameSession session;

    void Awake()
    {
        session = FindObjectOfType<GameSession>();
    }

    public void Activate(GameObject activator)
    {
        session.LoadLevel(destination, false);
        session.currentLevelBit = levelBit;
        session.overworldPosition = activator.transform.localPosition;
        session.useSessionPosition = true;
    }
}
