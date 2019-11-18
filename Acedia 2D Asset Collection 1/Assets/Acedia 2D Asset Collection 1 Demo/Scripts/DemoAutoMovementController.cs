using UnityEngine;

[DisallowMultipleComponent]
public class DemoAutoMovementController : MonoBehaviour {

    public DemoMovementSO autoMovement;
    public float minIdleTime = 1f;
    public float maxIdleTime = 5f;
    public float minSpeed = 1f;
    public float maxSpeed = 2f;

    [HideInInspector] public float speed;
    [HideInInspector] public Vector3 targetPosition;
    [HideInInspector] public int facingDirection = 1;
    [HideInInspector] public int curDirection = 1;
    [HideInInspector] public float originalXScale;

    private float idleTime;
    private float stateTimeElapse;
    private bool canMove;
    private Animator animator;

    private void Start() {

        //To ensure that z is always 0
        transform.position = new Vector3(transform.position.x, transform.position.y);
        animator = GetComponent<Animator>();
        originalXScale = transform.localScale.x;
        autoMovement.UpdateMovementData(this);      
    }

    private void Update() {

        if(!canMove) {

            if(CheckIfCountDownElapsed(idleTime)) {               
                autoMovement.UpdateMovementData(this);
            }
            return;
        }
            
        autoMovement.Move(transform, this);
        CheckPosition();
    }

    public void UpdateBehaviour(Vector2 targetPos) {

        targetPosition = targetPos;
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        speed = Random.Range(minSpeed, maxSpeed);
        stateTimeElapse = 0f;
        canMove = true;
        
        if (animator != null)
            animator.speed = speed;
    }

    private void CheckPosition() {

        float remainingDistance = (transform.position - targetPosition).sqrMagnitude;

        if (remainingDistance > float.Epsilon) return;
        
        canMove = false;

        if (animator != null)
            animator.speed = 1f;
    }

    private bool CheckIfCountDownElapsed(float duration) {

        stateTimeElapse += Time.deltaTime;
        return stateTimeElapse >= duration;
    }
}
