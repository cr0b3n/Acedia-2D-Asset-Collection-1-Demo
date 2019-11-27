using UnityEngine;

[DisallowMultipleComponent]
public class Set7Animation : MonoBehaviour {

    public Animator animator;
    public DemoInput input;
    public Rigidbody2D rigidBody;
    public Set7Movement movement;

    private int speedAnimID;
    private int isJumpingAnimID;
    private int isGrounded;
    private int yVelocityAnimID;
    private int isDashingID;
    private int isDead;

    private void Start() {
        //Set up animation int ID cause they are faster than strings
        speedAnimID = Animator.StringToHash("speed");
        isJumpingAnimID = Animator.StringToHash("isJumping");
        isGrounded = Animator.StringToHash("onGround");
        yVelocityAnimID = Animator.StringToHash("yVelocity");
        isDashingID = Animator.StringToHash("isDashing");
        isDead = Animator.StringToHash("isDead");
    }

    private void Update() {

        animator.SetBool(isDashingID, movement.isDashing);
        animator.SetBool(isGrounded, movement.isOnGround);
        animator.SetFloat(yVelocityAnimID, rigidBody.velocity.y);
        animator.SetBool(isJumpingAnimID, movement.isJumping);

        animator.SetFloat(speedAnimID, Mathf.Abs(rigidBody.velocity.x));
    }
}
