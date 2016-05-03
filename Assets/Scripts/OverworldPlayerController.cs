using UnityEngine;
using System.Collections;
using Prime31;

public class OverworldPlayerController : MonoBehaviour {

    public float speed;

    private CharacterController2D controller;
    private SpriteRenderer sprite;

    private LevelEntrance currentEntrance;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        controller.onTriggerEnterEvent += Controller_onTriggerEnterEvent;
        controller.onTriggerExitEvent += Controller_onTriggerExitEvent;

        sprite = GetComponent<SpriteRenderer>();
    }

    private void Controller_onTriggerEnterEvent(Collider2D obj)
    {
        currentEntrance = obj.gameObject.GetComponent<LevelEntrance>();
    }

    private void Controller_onTriggerExitEvent(Collider2D obj)
    {
        currentEntrance = null;
    }

    // Update is called once per frame
    void Update()
    {
        var leftPress = Input.GetKey(KeyCode.LeftArrow);
        var downPress = Input.GetKey(KeyCode.DownArrow);
        var rightPress = Input.GetKey(KeyCode.RightArrow);
        var upPress = Input.GetKey(KeyCode.UpArrow);
        var usePress = Input.GetKey(KeyCode.Z);

        if (usePress && currentEntrance != null)
        {
            currentEntrance.Activate(this.gameObject);
        }

        var vel = controller.velocity;

        vel.x = rightPress ? speed : leftPress ? -speed : 0;
        vel.y = upPress ? speed : downPress? -speed : 0;

        if (vel.x != 0)
        {
            sprite.flipX = vel.x < 0;
        }

        controller.move(vel * Time.deltaTime);
    }
}
