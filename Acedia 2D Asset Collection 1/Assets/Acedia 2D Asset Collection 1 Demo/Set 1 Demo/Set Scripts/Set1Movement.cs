using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(DemoInput4D))]
public class Set1Movement : MonoBehaviour {

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed

    public Vector2 maxPosition;
    public Vector2 minPosition;

    private DemoInput4D input;                      //The current inputs for the player
    private Rigidbody2D rigidBody;                  //The rigidbody component
    private bool isActive;

    private void Start() {
        //Get a reference to the required components
        input = GetComponent<DemoInput4D>();
        rigidBody = GetComponent<Rigidbody2D>();

        isActive = true;
    }

    private void Update() {

        if (!isActive) return;

        if (transform.position.x <= minPosition.x)
            SetOutOfBoundsPosition(new Vector3(minPosition.x, transform.position.y));
        else if (transform.position.x >= maxPosition.x)
            SetOutOfBoundsPosition(new Vector3(maxPosition.x, transform.position.y));

        if (transform.position.y <= minPosition.y)
            SetOutOfBoundsPosition(new Vector3(transform.position.x, minPosition.y));
        else if (transform.position.y >= maxPosition.y)
            SetOutOfBoundsPosition(new Vector3(transform.position.x, maxPosition.y));
    }

    private void FixedUpdate() {

        if (!isActive) return;

        //Process ground and air movements
        Movement();
    }

    public void ReleaseAttack() {
        Debug.Log("Releasing Attack!");
    }

    public void AttackComplete() {
        Debug.Log("Attack Complete!");
    }

    public void MakeInActive() {
        isActive = false;
        rigidBody.velocity = Vector2.zero;
    }

    private void Movement() {

        //Calculate the desired velocity based on inputs
        float xVelocity = speed * input.horizontal;
        float yVelocity = speed * input.vertical;

        rigidBody.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void SetOutOfBoundsPosition(Vector3 newPos) {
        rigidBody.velocity = Vector2.zero;
        transform.position = newPos;
    }
}
