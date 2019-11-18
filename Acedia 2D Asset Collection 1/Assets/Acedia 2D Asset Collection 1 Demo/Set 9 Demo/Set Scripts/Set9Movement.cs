using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(DemoInput4D))]
public class Set9Movement : MonoBehaviour {

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed

    public Vector2 maxPosition;
    public Vector2 minPosition;

    private DemoInput4D input;                      //The current inputs for the player
    private Rigidbody2D rigidBody;                  //The rigidbody component
    private float originalXScale;                   //Original scale on X axis
    private int direction = 1;                      //Direction player is facing
    private bool isActive;

    private void Start() {
        //Get a reference to the required components
        input = GetComponent<DemoInput4D>();
        rigidBody = GetComponent<Rigidbody2D>();

        //Record the original x scale of the player
        originalXScale = transform.localScale.x;
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

    public void MakeInActive() {
        isActive = false;
        rigidBody.velocity = Vector2.zero;
    }

    private void Movement() {

        //Calculate the desired velocity based on inputs
        float xVelocity = speed * input.horizontal;
        float yVelocity = speed * input.vertical;

        if (xVelocity * direction < 0f)
            FlipCharacterDirection();

        rigidBody.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void SetOutOfBoundsPosition(Vector3 newPos) {
        rigidBody.velocity = Vector2.zero;
        transform.position = newPos;
    }

    private void FlipCharacterDirection() {

        //Turn the character by flipping the direction
        direction *= -1;

        //Record the current scale
        Vector3 scale = transform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = originalXScale * direction;

        //Apply the new scale
        transform.localScale = scale;
    }
}
