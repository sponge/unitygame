using Prime31;
using UnityEngine;

public class HammerBrosController : MonoBehaviour {
    public float gravity;
    public float speed;
    public Vector2 turnRange;

    public Vector2 burstTimeRange;

    private CharacterController2D controller;
    private Hurtable hurtComponent;
    private BaseWeapon weapon;

    private bool flipSpeed;
    private float moveTime = 0;
    private float nextBurstTime = 0;

    private void Controller_onTriggerStayEvent(Collider2D hit) {
        var hurtable = hit.GetComponent<Hurtable>();
        if (hurtable && !hurtComponent.isOnDamageCooldown()) {
            hurtable.Hurt(1, transform.position);
        }
    }

    private void Start() {
        controller = GetComponent<CharacterController2D>();
        controller.onTriggerStayEvent += Controller_onTriggerStayEvent;

        hurtComponent = GetComponent<Hurtable>();
        hurtComponent.onHurt += onHurt;

        weapon = GetComponent<BaseWeapon>();

        moveTime = Time.time + Random.Range(turnRange.x, turnRange.y);
        nextBurstTime = moveTime + Random.Range(burstTimeRange.x, burstTimeRange.y);
    }

    private void Update() {
        if (Time.time > moveTime || controller.collisionState.left || controller.collisionState.right) {
            flipSpeed = !flipSpeed;
            moveTime = Time.time + Random.Range(turnRange.x, turnRange.y);
        }

        var vel = controller.velocity;

        if (controller.isGrounded) {
            vel.x = speed * (flipSpeed ? -1 : 1);
        }

        if (Time.time > nextBurstTime) {
            weapon.Fire();
            nextBurstTime = Time.time + Random.Range(burstTimeRange.x, burstTimeRange.y);
        }

        if (weapon.isFiring()) {
            vel.x = 0;
        }

        vel.y -= gravity * Time.deltaTime;
        controller.move(vel * Time.deltaTime);
    }

    private void onHurt(int amt, Vector3 dir) {
        if (dir.y < 0.7f) {
            flipSpeed = dir.x > 0;
            controller.velocity.x = dir.x < 0 ? 80 : -80;
            controller.velocity.y = 200;
            controller.move(controller.velocity * Time.deltaTime);
        }
    }
}