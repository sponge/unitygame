using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

    GameSession session;

    public void Awake()
    {
        session = FindObjectOfType<GameSession>();
    }

    public void StartLevelExit()
    {
        var players = FindObjectsOfType<PlayerController>();
        foreach (var player in players)
        {
            player.enabled = false;
            player.GetComponent<Hurtable>().invulnerable = true;
        }

        session.EndLevel();
    }
}
