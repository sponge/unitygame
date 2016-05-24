using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LevelEntrance : MonoBehaviour {
    public string destination;
    public int levelBit;
    public Sprite image;
    public Sprite completedImage;

    private GameSession session;
    private SpriteRenderer sprite;
    private BoxCollider2D col;

    private void Awake() {
        session = FindObjectOfType<GameSession>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = (session.levelCompleteBit & levelBit) != 0 ? completedImage : image;
    }

    public void Activate(GameObject activator) {
        session.LoadLevel(destination, false);
        session.currentLevelBit = levelBit;
        session.overworldPosition = activator.transform.localPosition;
        session.useSessionPosition = true;
    }
}