using UnityEngine;
using System.Collections;

public class BarrierVisibility : MonoBehaviour {

    public int levelCompletedCondition;

    private Collider2D col;
    private GameSession session;

	// Use this for initialization
	void Start () {
        col = GetComponent<Collider2D>();
        session = FindObjectOfType<GameSession>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (col == null || session == null)
        {
            return;
        }

        if ((session.levelCompleteBit & levelCompletedCondition) != 0)
        {
            col.enabled = false;
        }
	}
}
