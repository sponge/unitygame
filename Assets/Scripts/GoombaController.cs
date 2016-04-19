using UnityEngine;
using System.Collections;
using Prime31;

public class GoombaController : MonoBehaviour {

    private CharacterController2D controller;

    public float gravity;
    public float speed;

    private bool flipSpeed;

	void Start () {
        controller = GetComponent<CharacterController2D>();
    }
	
	// Update is called once per frame
	void Update () {
        var vel = controller.velocity;

        if (controller.collisionState.left || controller.collisionState.right)
        {
            flipSpeed = !flipSpeed;
        }

        vel.x = speed * (flipSpeed ? -1 : 1);
        vel.y -= 625 * Time.deltaTime;

        var oldLayer = this.gameObject.layer;
        gameObject.layer = 31;
        controller.move(vel * Time.deltaTime);
        gameObject.layer = oldLayer;
    }
}
