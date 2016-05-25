using Prime31;
using UnityEngine;

public class TurtleController : MonoBehaviour {
    public float gravity;
    public float speed;
    public float shellSpeed;
    public float shellFriction;
    public float shellStillDuration;

    private CharacterController2D controller;
    private Hurtable hurtComponent;
    private SpriteRenderer sprite;
    private Animator animator;

    private int animWalk = Animator.StringToHash("turtle_walk");
    private int animShellIdle = Animator.StringToHash("turtle_shell_idle");
    private int animShellWake = Animator.StringToHash("turtle_shell_wake");
    private int shellSpinAnim = Animator.StringToHash("turtle_shell_spin");
    private bool inShell = false;

    private bool flipSpeed;
    private float leaveShellTime;

    private void Controller_onTriggerStayEvent(Collider2D hit) {
        var hurtable = hit.GetComponent<Hurtable>();
        if (hurtable) {
            hurtable.Hurt(1, transform.position);
        }
    }

    private void Controller_onControllerCollidedEvent(RaycastHit2D obj) {
        if (!inShell || controller.velocity.x == 0) {
            return;
        }

        var hurtable = obj.collider.GetComponent<Hurtable>();
        if (hurtable) {
            hurtable.Hurt(1, transform.position);
        }
    }

    private void Start() {
        controller = GetComponent<CharacterController2D>();
        controller.onTriggerStayEvent += Controller_onTriggerStayEvent;
        controller.onControllerCollidedEvent += Controller_onControllerCollidedEvent;

        hurtComponent = GetComponent<Hurtable>();
        hurtComponent.onHurt += HurtComponent_onHurt;
        hurtComponent.canHurt += HurtComponent_canHurt;

        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        var vel = controller.velocity;

        if (inShell && leaveShellTime != 0) {
            if (Time.time > leaveShellTime) {
                animator.Play(animWalk);
                inShell = false;
                leaveShellTime = 0;
            }
            else if ((leaveShellTime - Time.time) / shellStillDuration < 0.5) {
                animator.Play(animShellWake);
            }
        }

        if (controller.collisionState.left || controller.collisionState.right) {
            vel.x *= -1;
            flipSpeed = !flipSpeed;
        }

        if (inShell && controller.isGrounded) {
            vel.x = vel.x - (vel.x > 0 ? shellFriction : -shellFriction) * Time.deltaTime;
            if (Mathf.Abs(vel.x) < 5) {
                vel.x = 0;
                if (leaveShellTime == 0) {
                    leaveShellTime = Time.time + shellStillDuration;
                    animator.Play(animShellIdle);
                }
            }
        }
        else if (controller.isGrounded) {
            vel.x = speed * (flipSpeed ? -1 : 1);
        }

        vel.y -= gravity * Time.deltaTime;

        sprite.flipX = vel.x > 0;

        controller.move(vel * Time.deltaTime);
    }

    private void HurtComponent_onHurt(int amt, Vector3 dir) {
        inShell = true;
        animator.Play(shellSpinAnim);
        leaveShellTime = 0;

        if (dir.y < 0.7f) {
            controller.velocity.y = 200;
        }
        flipSpeed = dir.x > 0;
        controller.velocity.x = dir.x < 0 ? shellSpeed : -shellSpeed;
        controller.move(controller.velocity * Time.deltaTime);
    }

    private bool HurtComponent_canHurt(int amt, Vector3 dir) {
        return !inShell || (inShell && leaveShellTime != 0 && (leaveShellTime - Time.time) / shellStillDuration < 0.5);
    }
}