using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Set8Movement : MonoBehaviour {

    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public float maxFallSpeed = -25f;       //Max speed player can fall
    public float dashTime = 0.2f;
    public float dashForce = 50f;
    public Transform footPosition;
    //public PlayerAfterImage afterImage;

    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump
    public float jumpHoldForce = 1.9f;      //Incremental force when jump is held
    public float jumpHoldDuration = .1f;    //How long the jump key can be held
    public float landingResetTime = 0.1f;

    [Header("Environment Check Properties")]
    public float leftFootOffset = .4f;      //X Offset of feet raycast
    public float rightFootOffset = .4f;     //X Offset of feet raycast
    public float groundOffset = 1f;         //Y offset before ground
    public float groundDistance = .2f;      //Distance player is considered to be on the ground
    public LayerMask groundLayer;           //Layer of the ground

    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isJumping;                  //Is player jumping?

    private DemoInput input;                      //The current inputs for the player
    //BoxCollider2D bodyCollider;             //The collider component
    private Rigidbody2D rigidBody;                  //The rigidbody component

    private float jumpTime;                         //Variable to hold jump duration
    private float originalXScale;                   //Original scale on X axis
    private int direction = 1;                      //Direction player is facing
    private float stateTimeElapse;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isDashing;

    public event Action<bool> OnAttack;

    private void Start() {
        //Get a reference to the required components
        input = GetComponent<DemoInput>();
        rigidBody = GetComponent<Rigidbody2D>();

        //Record the original x scale of the player
        originalXScale = transform.localScale.x;
        isAttacking = false;
    }

    private void Update() {

        //Necessary for attack animation to be acurate
        if (input.attackPressed)
            isAttacking = true;
    }

    private void FixedUpdate() {

        //Check the environment to determine status
        PhysicsCheck();

        //Process ground and air movements
        Dash();
        GroundMovement();
        MidAirMovement();
    }

    private void OnDrawGizmos() {
        Debug.DrawRay(transform.position + new Vector3(leftFootOffset, groundOffset), Vector2.down * groundDistance, Color.red);
        Debug.DrawRay(transform.position + new Vector3(rightFootOffset, groundOffset), Vector2.down * groundDistance, Color.blue);
    }

    private void PhysicsCheck() {
        //Start by assuming the player isn't on the ground and the head isn't blocked
        isOnGround = false;

        //Cast rays for the left and right foot
        RaycastHit2D leftCheck = Raycast(new Vector2(leftFootOffset, groundOffset), Vector2.down, groundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(rightFootOffset, groundOffset), Vector2.down, groundDistance);

        //If either ray hit the ground, the player is on the ground
        if (leftCheck || rightCheck)
            isOnGround = true;
    }

    private void GroundMovement() {

        if (isAttacking || isDashing)
            return;
        
        //Calculate the desired velocity based on inputs
        float xVelocity = speed * input.horizontal;

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * direction < 0f)
            FlipCharacterDirection();

        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
    }

    private void ResetDash() {
        stateTimeElapse = 0f;
        isDashing = false;
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
    }

    private void Dash() {

        if (isDashing) {
            if (CheckIfCountDownElapsed(dashTime)) {
                ResetDash();
                return;
            }
        } else if (isDashing && isAttacking) {

            ResetDash();
        } else if (isAttacking) {
            return;
        }
        
        if (!input.dashPressed) return;

        if (!isOnGround && !isDashing) return;

        //Calculate the desired velocity based on inputs
        float xVelocity = speed * input.direction;

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * direction < 0f)
            FlipCharacterDirection();

        isDashing = true;
        rigidBody.velocity = new Vector2(dashForce * input.direction, rigidBody.velocity.y);
    }

    public void Attacking() {
        //This method is use by attack animation
        rigidBody.velocity = Vector2.zero;      
    }

    //This method is use by attack animation
    public void Hitting() => OnAttack?.Invoke(true);

    //This method is use by attack animation & attack behaviour
    public void Retract() => OnAttack?.Invoke(false);

    public void FootStep() {
        //This method is use by run animation
    }

    private void MidAirMovement() {

        if (isAttacking) return;

        //If the jump key is pressed AND the player isn't already jumping AND EITHER
        //the player is on the ground or within the coyote time window...
        if (input.jumpPressed && !isJumping && isOnGround) {//(isOnGround || coyoteTime > Time.time)) {

            //...The player is no longer on the groud and is jumping...
            isOnGround = false;
            isJumping = true;
            //...record the time the player will stop being able to boost their jump...
            jumpTime = Time.time + jumpHoldDuration;

            //...add the jump force to the rigidbody...
            rigidBody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        //Otherwise, if currently within the jump time window...
        else if (isJumping) {
            //...and the jump button is held, apply an incremental force to the rigidbody...
            if (input.jumpHeld)
                rigidBody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);

            //...and if jump time is past, set isJumping to false
            if (jumpTime <= Time.time)
                isJumping = false;
        }

        //If player is falling too fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);
    }

    private void FlipCharacterDirection() {

        //Turn the character by flipping the direction
        direction *= -1;

        //Record the current scale
        Vector3 scale = transform.localScale;

        rightFootOffset *= -1;
        leftFootOffset *= -1;

        //Set the X scale to be the original times the direction
        scale.x = originalXScale * direction;

        //Apply the new scale
        transform.localScale = scale;
    }


    //These two Raycast methods wrap the Physics2D.Raycast() and provide some extra
    //functionality
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length) {
        //Call the overloaded Raycast() method using the ground layermask and return 
        //the results
        return Raycast(offset, rayDirection, length, groundLayer);
    }

    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask mask) {
        //Record the player's position
        Vector2 pos = transform.position;
        //pos.y += 0.1f;

        //Send out the desired raycasr and record the result
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, mask);

        //If we want to show debug raycasts in the scene...
        if (drawDebugRaycasts) {
            //...determine the color based on if the raycast hit...
            Color color = hit ? Color.red : Color.green;
            //...and draw the ray in the scene view
            Debug.DrawRay(pos + offset, rayDirection * length, color);
        }

        //Return the results of the raycast
        return hit;
    }

    private bool CheckIfCountDownElapsed(float duration) {

        stateTimeElapse += Time.deltaTime;
        return stateTimeElapse >= duration;
    }
}
