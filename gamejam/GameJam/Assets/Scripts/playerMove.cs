using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public Rigidbody2D playersRigidBody;
    public float jumpStrength;
    public float walkSpeed = 7;
    private Animator playersAnimator;
    private BoxCollider2D coll;
    public SpriteRenderer spriterend;
    private float dirX = 0;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;


    [SerializeField] private LayerMask jumpableGround;
    private enum movementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSound;
    private void Start()
    {
        playersAnimator = GetComponent<Animator>();
        spriterend = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }


    private void Update()
    {
        if (playersRigidBody.velocity.y < 0)
        {
            playersRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

        }
        else if (playersRigidBody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            playersRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        
        dirX = Input.GetAxisRaw("Horizontal");
        playersRigidBody.velocity = new Vector2(dirX * walkSpeed, playersRigidBody.velocity.y);


        if (Input.GetButtonDown("Jump") && IHaveTheHighGround())
        {
            playersRigidBody.velocity = Vector2.up * jumpStrength;

        }
        animationStuff();
    }
    private bool IHaveTheHighGround()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    private void animationStuff()
    {
        movementState state;
        if (dirX > 0)
        {
            state = movementState.running;
            spriterend.flipX = false;
        }
        else if (dirX < 0)
        {
            state = movementState.running;
            spriterend.flipX = true;
        }
        else
        {
            state = movementState.idle;
        }
        if (playersRigidBody.velocity.y > 0.1f)
        {
            state = movementState.jumping;
        }
        else if (playersRigidBody.velocity.y < -0.1f)
        {
            state = movementState.falling;
        }
        playersAnimator.SetInteger("State", (int)state);
    }
}
