using UnityEngine;
using System.Collections;
using Prime31;

public class OverworldPlayerController : MonoBehaviour {

    public float speed;

    private CharacterController2D controller;
    private SpriteRenderer sprite;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update()
    {
        var leftPress = Input.GetKey(KeyCode.LeftArrow);
        var downPress = Input.GetKey(KeyCode.DownArrow);
        var rightPress = Input.GetKey(KeyCode.RightArrow);
        var upPress = Input.GetKey(KeyCode.UpArrow);
        var usePress = Input.GetKey(KeyCode.Z);

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
