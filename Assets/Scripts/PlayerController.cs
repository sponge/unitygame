using UnityEngine;
using System.Collections;
using Prime31;

public class PlayerController : MonoBehaviour {
    private int idleAnim = Animator.StringToHash("player_idle");
    private int runAnim = Animator.StringToHash("player_run");

    private CharacterController2D _controller;
    private Animator _animator;
    private SpriteRenderer _sprite;

    public float gravity;
    public float jumpHeight;
    public float wallSlideSpeed;
    public float earlyJumpEndModifier;
    public float pogoJumpHeight;
    public float doubleJumpHeight;
    public float wallJumpX;
    public float maxSpeed, terminalVelocity;
    public float speedJumpBonus;
    public float attackLength;
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
    private bool attackHeld;
    private float attackTime;
    private float accelType;

    enum Direction { Left, Right };


    void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }


    // Use this for initialization
    void Start()
    {
	
	}

    private float getAccel(Direction direction)
    {
        var vel = _controller.velocity;
        var isSkidding = ((direction == Direction.Left && vel.x > 0) || (direction == Direction.Right && vel.y < 0));

        if (Time.time < stunTime)
        {
            return 0;
        }

        if (!_controller.isGrounded)
        {
            return isSkidding ? turnAirAccel : airAccel;
        }

        return isSkidding ? skidAccel : accel;
    }

    // Update is called once per frame
    void Update()
    {
        var leftPress = Input.GetKey(KeyCode.LeftArrow);
        var downPress = Input.GetKey(KeyCode.DownArrow);
        var rightPress = Input.GetKey(KeyCode.RightArrow);
        var jumpPress = Input.GetKey(KeyCode.Z);
        var attackPress = Input.GetKey(KeyCode.X);

        var vel = _controller.velocity;

        if (_controller.isGrounded)
        {
            didJump = false;
            canDoubleJump = false;
        } else
        {
            vel.y -= gravity * Time.deltaTime;
        }

        // FIXME: check for goal time and dont move. prob a better way to do this in unity.

        //var wasSliding = wallSliding;
        wallSliding = false;
        canWallJump = !_controller.isGrounded && (_controller.collisionState.left || _controller.collisionState.right);
        // FIXME: isGrounded doesn't seem to work right in corners in my test map
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

        if (!_controller.isGrounded && (vel.y < 0 || !willPogo))
        {
            willPogo = downPress;
        }

        // check for pogo jump
        if (_controller.isGrounded && willPogo)
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
            } else if (_controller.isGrounded)
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

        if (attackPress && !attackHeld)
        {
            attackTime = Time.time + attackLength;
            attackHeld = true;
        }

        // player wants to move left, check what their accel should be
        var lastAccel = accelType;
        if (leftPress && Time.time > attackTime)
        {
            accelType = getAccel(Direction.Left);
            vel.x -= accelType * Time.deltaTime;
            _sprite.flipX = true;
        // player wants to move right
        } else if (rightPress && Time.time > attackTime)
        {
            accelType = getAccel(Direction.Right);
            vel.x += accelType * Time.deltaTime;
            _sprite.flipX = false;
        // player isn't moving, bring them to stop
        } else if (vel.x != 0)
        {
            var friction = (_controller.isGrounded ? groundFriction : airFriction) * Time.deltaTime;
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

        _animator.Play(_controller.isGrounded && vel.x != 0 ? runAnim : idleAnim);

        _controller.move(vel * Time.deltaTime);

        if (vel.y > 0 && !_controller.collisionState.above)
        {
            _controller.velocity.y = uncappedY;
        }
	}
}
