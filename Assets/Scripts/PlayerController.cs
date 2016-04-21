using UnityEngine;
using System.Collections;
using Prime31;

public class PlayerController : MonoBehaviour {
    private int idleAnim = Animator.StringToHash("player_idle");
    private int runAnim = Animator.StringToHash("player_run");
    private int skidAnim = Animator.StringToHash("player_skid");
    private int pogoAnim = Animator.StringToHash("player_pogo");
    private int swordAnim = Animator.StringToHash("player_sword");

    private CharacterController2D controller;
    private Animator animator;
    private SpriteRenderer sprite;
    private BaseWeapon weapon;
    private Hurtable hurtable;

    private TextMesh debugText;

    public float gravity;
    public float jumpHeight;
    public float wallSlideSpeed;
    public float earlyJumpEndModifier;
    public float pogoJumpHeight;
    public float doubleJumpHeight;
    public float wallJumpX;
    public float maxSpeed, terminalVelocity;
    public float speedJumpBonus;
    public float airAccel, turnAirAccel;
    public float accel, skidAccel;
    public float groundFriction, airFriction;

    private bool didJump;
    private bool canDoubleJump;
    private bool canWallJump;
    private bool wallSliding;
    private bool jumpHeld;
    private bool willPogo;
    private float stunTime;
    private float accelType;

    enum Direction { Left, Right };

    void Awake()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        weapon = GetComponent<BaseWeapon>();

        hurtable = GetComponent<Hurtable>();
        hurtable.onDeath += OnDeath;

        debugText = GameObject.Find("DebugText").GetComponent<TextMesh>();
    }

    private float getAccel(Direction direction)
    {
        var vel = controller.velocity;
        var isSkidding = ((direction == Direction.Left && vel.x > 0) || (direction == Direction.Right && vel.x < 0));

        if (Time.time < stunTime)
        {
            return 0;
        }

        if (!controller.isGrounded)
        {
            return isSkidding ? turnAirAccel : airAccel;
        }

        return isSkidding ? skidAccel : accel;
    }

    // Update is called once per frame
    void Update()
    {
        if (debugText)
        {
            debugText.text = "";
        }

        var leftPress = Input.GetKey(KeyCode.LeftArrow);
        var downPress = Input.GetKey(KeyCode.DownArrow);
        var rightPress = Input.GetKey(KeyCode.RightArrow);
        var jumpPress = Input.GetKey(KeyCode.Z);
        var attackPress = Input.GetKey(KeyCode.X);

        var vel = controller.velocity;

        if (controller.isGrounded)
        {
            didJump = false;
            canDoubleJump = false;
        }

        vel.y -= gravity * Time.deltaTime;

        // FIXME: check for goal time and dont move. prob a better way to do this in unity.

        //var wasSliding = wallSliding;
        wallSliding = false;
        canWallJump = !controller.isGrounded && (controller.collisionState.left || controller.collisionState.right);

        if (vel.y < 0 && canWallJump)
        {
            wallSliding = true;
        }

        // FIXME:
        // if not wasSlide and ent.wall_sliding then
        //     Tiny.addEntity(s.world, {event = 'sound', name = 'wallslide'})
        // elseif wasSlide and not ent.wall_sliding then
        //     Tiny.addEntity(s.world, {event = 'stopsound', name = 'wallslide'})
        // end

        if (wallSliding)
        {
            vel.y = -wallSlideSpeed;
        }

        if (!jumpPress && jumpHeld)
        {
            jumpHeld = false;
            if (vel.y > 0)
            {
                vel.y *= earlyJumpEndModifier;
            }
        }

        if (!controller.isGrounded && (vel.y > 0 || !willPogo))
        {
            willPogo = downPress;
        }

        // check for pogo jump
        if (controller.isGrounded && willPogo)
        {
            vel.y = pogoJumpHeight;
            canDoubleJump = true;
            didJump = true;
            willPogo = false;
            // FIXME: pogo sound here
        // check for other jumps
        } else if (jumpPress && !jumpHeld)
        {
            // check for walljump
            if (canWallJump)
            {
                vel.y = doubleJumpHeight;
                vel.x = wallJumpX * (rightPress ? -1 : 1);
                stunTime = Time.time + 0.1f;
                jumpHeld = true;
                didJump = true;
                // FIXME: jump sound here
            // check for first jump
            } else if (controller.isGrounded)
            {
                vel.y = jumpHeight + (Mathf.Abs(vel.x) >= maxSpeed * 0.25 ? speedJumpBonus : 0);
                jumpHeld = true;
                canDoubleJump = true;
                didJump = true;
                // FIXME: jump sound here
            // check for second jump
            } else if (canDoubleJump)
            {
                vel.y = doubleJumpHeight;
                canDoubleJump = false;
                jumpHeld = true;
                didJump = true;
                // FIXME: jump sound here
            }
        }

        if (weapon)
        {
            weapon.UpdatePress(attackPress);
        }

        // player wants to move left, check what their accel should be
        var lastAccel = accelType;
        if ((leftPress || rightPress) && !weapon.isFiring())
        {
            accelType = getAccel(leftPress ? Direction.Left : Direction.Right);
            vel.x += accelType * Time.deltaTime * (leftPress ? -1 : 1);
            transform.localScale = new Vector3(leftPress ? -1 : 1, 1, 0);
        } else if (vel.x != 0)
        {
            var friction = (controller.isGrounded ? groundFriction : airFriction) * Time.deltaTime;
            if (friction > Mathf.Abs(vel.x))
            {
                vel.x = 0;
            } else
            {
                vel.x += friction * (vel.x > 0 ? -1 : 1);
            }

            accelType = 0;
        }

        // FIXME: skid sound

        vel.x = Mathf.Clamp(vel.x, -maxSpeed, maxSpeed);
        var uncappedY = vel.y;
        vel.y = Mathf.Clamp(vel.y, -terminalVelocity, terminalVelocity);

        if (weapon && weapon.isFiring())
        {
            animator.Play(swordAnim);
        } else if (willPogo)
        {
            animator.Play(pogoAnim);
        } else if (accelType == skidAccel)
        {
            animator.Play(skidAnim);
        } else
        {
            animator.Play(controller.isGrounded && vel.x != 0 ? runAnim : idleAnim);
        }

        if (debugText)
        {
            debugText.text += "Health: " + hurtable.currentHealth + "/" + hurtable.maxHealth +"\n";
        }

        controller.move(vel * Time.deltaTime);

        if (vel.y > 0 && !controller.collisionState.above)
        {
            controller.velocity.y = uncappedY;
        }
	}

    void OnDeath()
    {
        debugText.text = "You died! Press jump to respawn";
        Destroy(gameObject);
    }
}
