using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(DemoInput4D))]
public class Set1Movement : MonoBehaviour, IActivatable {

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public SortingGroup sortingGroup;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    private DemoInput4D input;                      //The current inputs for the player
    private Rigidbody2D rigidBody;                  //The rigidbody component
    private int waterLayer;
    private int defaultLayer;
    private Vector3 originalPos;

    private void Start() {
        //Get a reference to the required components
        input = GetComponent<DemoInput4D>();
        rigidBody = GetComponent<Rigidbody2D>();

        originalPos = new Vector3(transform.position.x, transform.position.y);
        waterLayer = LayerMask.NameToLayer("Water");
        defaultLayer = LayerMask.NameToLayer("Default");

        gameObject.layer = waterLayer;
    }

    private void Update() {

        if (!input.isActive) return;

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

        if (!input.isActive) {
            ReturnToOriginalPosition();
            return;
        }

        //Process ground and air movements
        Movement();
    }

    private void ReturnToOriginalPosition() {

        float remainingDistance = (transform.position - originalPos).sqrMagnitude;

        if (remainingDistance <= float.Epsilon) return;

        transform.position = Vector2.MoveTowards(transform.position, originalPos, speed * Time.fixedDeltaTime);
    }

    public void ReleaseAttack() {
        Debug.Log("Releasing Attack!");
    }

    public void AttackComplete() {
        Debug.Log("Attack Complete!");
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

    public void Active(bool isActive, int sortOrder) {
        input.isActive = isActive;
        rigidBody.velocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        sortingGroup.sortingOrder = sortOrder;

        gameObject.layer = (isActive) ? defaultLayer : waterLayer;
    }
}
