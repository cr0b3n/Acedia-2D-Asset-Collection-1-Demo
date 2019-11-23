using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(DemoInput4D))]
public class Set1Movement : MonoBehaviour, ICharActivatable {

    [Header("Movement Properties")]
    public float speed = 8f;                //Player speed
    public SortingGroup sortingGroup;
    public Vector2 maxPosition;
    public Vector2 minPosition;
    public Transform ammoSpawnPosition;
    public GameObject ammo;

    private DemoInput4D input;                      //The current inputs for the player
    private Rigidbody2D rigidBody;                  //The rigidbody component
    private int waterLayer;
    private int defaultLayer;
    private Vector3 originalPos;
    private bool canAttack;

    public CharacterSet CharSet { get => CharacterSet.Set1; }

    private void Start() {
        //Get a reference to the required components
        input = GetComponent<DemoInput4D>();
        rigidBody = GetComponent<Rigidbody2D>();

        canAttack = true;
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

        if (!canAttack) return;

        DemoController.instance.ShowEffect(ammoSpawnPosition.position, Vector3.one, EffectType.Shoot);
        GameObject obj = Instantiate(ammo, ammoSpawnPosition.position, Quaternion.identity);
        Destroy(obj, 2f);

    }

    public void AttackComplete() {
        canAttack = true;
    }

    private void Movement() {

        //Calculate the desired velocity based on inputs
        Vector2 velocity = new Vector2(input.horizontal, input.vertical).normalized * speed;

        rigidBody.velocity = velocity;
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
