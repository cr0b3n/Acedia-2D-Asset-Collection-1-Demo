using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class DemoInput : MonoBehaviour {

    [HideInInspector] public float horizontal;      //Float that stores horizontal input
    [HideInInspector] public bool jumpHeld;         //Bool that stores jump pressed
    [HideInInspector] public bool jumpPressed;      //Bool that stores jump held

    private bool readyToClear;                      //Bool used to keep input in sync
    [HideInInspector] public bool isActive;
    [HideInInspector] public bool attackPressed;
    [HideInInspector] public bool dashPressed;

    private float lastPressedTime;
    private const float DOUBLE_PRESS_TIME = .2f;
    [HideInInspector] public float direction;

    private void Update() {

        if (!isActive)
            return;

        //Clear out existing input values
        ClearInput();

        //Process keyboard, mouse, gamepad (etc) inputs
        ProcessInputs();

        //Clamp the horizontal input to be between -1 and 1
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
    }

    private void FixedUpdate() {
        //In FixedUpdate() we set a flag that lets inputs to be cleared out during the 
        //next Update(). This ensures that all code gets to use the current inputs
        readyToClear = true;
    }

    private void ClearInput() {
        //If we're not ready to clear input, exit
        if (!readyToClear)
            return;

        //Reset all inputs
        horizontal = 0f;
        jumpPressed = false;
        jumpHeld = false;

        attackPressed = false;
        dashPressed = false;

        readyToClear = false;
    }

    private void ProcessInputs() {
        //Accumulate horizontal axis input
        horizontal += Input.GetAxis("Horizontal");

        //Accumulate button inputs
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");

        //Avoid attacking while pressing a button
        if(!EventSystem.current.IsPointerOverGameObject()) {
            attackPressed = attackPressed || Input.GetButtonDown("Fire1");
        }

        //Check dash direction & input
        float lastDirection = direction;
        float curDirection =  Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Horizontal")) {

            direction = curDirection;

            float timeSinceLastPressed = Time.time - lastPressedTime;

            if (timeSinceLastPressed <= DOUBLE_PRESS_TIME && lastDirection == direction)
                dashPressed = true;

            lastPressedTime = Time.time;
        }
    }
}
