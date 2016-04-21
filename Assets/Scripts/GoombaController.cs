using UnityEngine;
using System.Collections;
using Prime31;

public class GoombaController : MonoBehaviour {

    private CharacterController2D controller;
    private Hurtable hurtComponent;

    public float gravity;
    public float speed;

    private bool flipSpeed;

    private void onTriggerStayEvent(Collider2D hit)
    {
        var hurtable = hit.GetComponent<Hurtable>();
        if (hurtable && !hurtComponent.isOnDamageCooldown())
        {
            hurtable.Hurt(1, transform.position);
        }
    }

	void Start () {
        controller = GetComponent<CharacterController2D>();
        controller.onTriggerStayEvent += onTriggerStayEvent;

        hurtComponent = GetComponent<Hurtable>();
        hurtComponent.onHurt += onHurt;
    }
	
	void Update () {
        var vel = controller.velocity;
			
        if (controller.collisionState.left || controller.collisionState.right)
        {
            flipSpeed = !flipSpeed;
        }

		if (controller.isGrounded)
		{
			vel.x = speed * (flipSpeed ? -1 : 1);
		}

        vel.y -= 625 * Time.deltaTime;

        controller.move(vel * Time.deltaTime);
    }

    void onHurt(int amt, Vector3 dir)
    {
		flipSpeed = dir.x > 0;
		controller.velocity.x = dir.x < 0 ? 80 : -80;
		controller.velocity.y = 200;
		controller.move(controller.velocity * Time.deltaTime);
    }
}
