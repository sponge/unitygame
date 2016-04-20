using UnityEngine;
using System.Collections;
using Prime31;

public class GoombaController : MonoBehaviour {

    private CharacterController2D controller;

    public float gravity;
    public float speed;

    private bool flipSpeed;

    private void onTriggerStayEvent(Collider2D hit)
    {
        var hurtable = hit.GetComponent<Hurtable>();
        if (hurtable)
        {
            hurtable.Hurt(1);
        }
    }

	void Start () {
        controller = GetComponent<CharacterController2D>();
        controller.onTriggerStayEvent += onTriggerStayEvent;
    }
	
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
