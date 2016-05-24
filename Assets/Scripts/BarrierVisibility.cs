using UnityEngine;

public class BarrierVisibility : MonoBehaviour {
    public int levelCompletedCondition;

    private Collider2D col;
    private GameSession session;
    private Renderer[] renderers;

    // Use this for initialization
    private void Start() {
        col = GetComponent<Collider2D>();
        session = FindObjectOfType<GameSession>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    private void Update() {
        if (session == null) {
            return;
        }

        if (col != null && (session.levelCompleteBit & levelCompletedCondition) != 0) {
            col.enabled = false;
        }

        foreach (var renderer in renderers) {
            renderer.enabled = (session.levelCompleteBit & levelCompletedCondition) == 0;
        }
    }
}