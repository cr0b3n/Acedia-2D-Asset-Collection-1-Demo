using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(DemoInput))]
public class Set10Movement : MonoBehaviour, IActivatable {
    public bool drawDebugRaycasts = true;   //Should the environment checks be visualized

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public float maxFallSpeed = -25f;       //Max speed player can fall
    public float dashTime = 0.15f;          //Time allowed for dashing
    public float dashForce = 50f;           //Force added when dashing
    public SortingGroup sortingGroup;

    [Header("Jump Properties")]
    public float jumpForce = 6.3f;          //Initial force of jump
    public float jumpHoldForce = 1.9f;      //Incremental force when jump is held
    public float jumpHoldDuration = .1f;    //How long the jump key can be held
    //public float landingResetTime = 0.1f;

    [Header("Environment Check Properties")]
    public float leftFootOffset = .4f;      //X Offset of feet raycast
    public float rightFootOffset = .4f;     //X Offset of feet raycast
    public float groundOffset = 1f;         //Y offset before ground
    public float groundDistance = .2f;      //Distance player is considered to be on the ground
    public LayerMask groundLayer;           //Layer of the ground

    [Header("Status Flags")]
    public bool isOnGround;                 //Is the player on the ground?
    public bool isJumping;                  //Is player jumping?

    private DemoInput input;      //The current inputs for the player
    private Rigidbody2D rigidBody;                  //The rigidbody component
    private float jumpTime;                         //Variable to hold jump duration
    private bool hasDashEffect;
    private float stateTimeElapse;

    [HideInInspector] public bool isDashing;

    private bool isFalling;
    private const float fallEffectRate = .5f;
    private float fallEffectTimer;
    private int waterLayer;
    private int defaultLayer;
    private Vector3 originalPos;

    private void Start() {
        //Get a reference to the required components
        input = GetComponent<DemoInput>();
        rigidBody = GetComponent<Rigidbody2D>();
        originalPos = new Vector3(transform.position.x, transform.position.y);
        waterLayer = LayerMask.NameToLayer("Water");
        defaultLayer = LayerMask.NameToLayer("Default");

        gameObject.layer = waterLayer;
    }

    private void FixedUpdate() {

        if (!input.isActive) {
            ReturnToOriginalPosition();
            return;
        }
            
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

    private void ReturnToOriginalPosition() {

        float remainingDistance = (transform.position - new Vector3(originalPos.x, transform.position.y)).sqrMagnitude;

        if (remainingDistance <= .1f) {

            if (rigidBody.velocity.y < -0.1) return;

            rigidBody.velocity = Vector2.zero;
            rigidBody.angularVelocity = 0f;
            return;
        }

        float dir = (transform.position.x > originalPos.x) ? -1 : 1;

        float xVelocity = speed * dir;

        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
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

        if (!isOnGround)
            isFalling = true;
    }

    private void GroundMovement() {

        if (isDashing)
            return;

        //Calculate the desired velocity based on inputs
        float xVelocity = speed * input.horizontal;

        //Apply the desired velocity 
        rigidBody.velocity = new Vector2(xVelocity, rigidBody.velocity.y);
    }

    private void ResetDash() {
        stateTimeElapse = 0f;
        isDashing = false;
        rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        hasDashEffect = false;
    }

    private void Dash() {

        if (isDashing) {
            if (CheckIfCountDownElapsed(dashTime)) {
                ResetDash();
                return;
            }
        } 

        if (!input.dashPressed) return;

        if (!isOnGround && !isDashing) return;

        if (!hasDashEffect) {
            //DO EFFECTS AND AUDIO
            DemoController.instance.ShowEffect(transform.position, new Vector3(input.direction * -1f, 1f, 1f), EffectType.Dash);
            hasDashEffect = true;
        }
   
        isDashing = true;
        rigidBody.velocity = new Vector2(dashForce * input.direction, rigidBody.velocity.y);
    }

    private void MidAirMovement() {

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

            JumpEffect();
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

        if (rigidBody.velocity.y < -0.1)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 2f);

        //If player is falling too fast, reduce the Y velocity to the max
        if (rigidBody.velocity.y < maxFallSpeed)
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, maxFallSpeed);

        if (isOnGround && isFalling) {
            isFalling = false;

            if (fallEffectTimer < Time.time) {
                fallEffectTimer = Time.time + fallEffectRate;
                LandEffect();
            }
        }
    }

    private void JumpEffect() {     
        DemoController.instance.ShowEffect(new Vector3(transform.position.x, transform.position.y + groundOffset), Vector3.one, EffectType.Jump);
    }

    private void LandEffect() {
        DemoController.instance.ShowEffect(new Vector3(transform.position.x, transform.position.y + groundOffset), Vector3.one, EffectType.Land);
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

    public void Active(bool isActive, int sortOrder) {
        input.isActive = isActive;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        sortingGroup.sortingOrder = sortOrder;

        gameObject.layer = (isActive) ? defaultLayer : waterLayer;
    }
}
