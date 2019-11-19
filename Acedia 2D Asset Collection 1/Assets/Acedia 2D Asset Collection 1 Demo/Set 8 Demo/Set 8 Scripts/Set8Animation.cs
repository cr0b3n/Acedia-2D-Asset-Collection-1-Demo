using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Set8Animation : MonoBehaviour {

    public Animator animator;
    public DemoInput input;
    public Rigidbody2D rigidBody;
    public Set8Movement movement;

    private int speedAnimID;
    private int isJumpingAnimID;
    private int isLandingAnimID;
    private int yVelocityAnimID;
    private int attackAnimID;
    private int isDashingID;
    private int isDead;

    private void Start() {
        //Set up animation int ID cause they are faster than strings
        speedAnimID = Animator.StringToHash("speed");
        isJumpingAnimID = Animator.StringToHash("isJumping");
        isLandingAnimID = Animator.StringToHash("isLanding");
        yVelocityAnimID = Animator.StringToHash("yVelocity");
        attackAnimID = Animator.StringToHash("attack");
        isDashingID = Animator.StringToHash("isDashing");
        isDead = Animator.StringToHash("isDead");
    }

    private void Update() {

        animator.SetBool(attackAnimID, movement.isAttacking);
        animator.SetBool(isDashingID, movement.isDashing);
        animator.SetBool(isLandingAnimID, movement.isOnGround);
        animator.SetFloat(yVelocityAnimID, rigidBody.velocity.y);        
        animator.SetBool(isJumpingAnimID, movement.isJumping);
        
        animator.SetFloat(speedAnimID, Mathf.Abs(rigidBody.velocity.x));
    }
}
