using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class DemoInput4D : MonoBehaviour {

    [HideInInspector] public float horizontal;      //Float that stores horizontal input
    [HideInInspector] public float vertical;        //Float that stores vertical input
    [HideInInspector] public bool attackPressed;

    private bool readyToClear;                      //Bool used to keep input in sync
    [HideInInspector] public bool isActive;
    private void Update() {

        if (!isActive)
            return;

        //Clear out existing input values
        ClearInput();

        //Process keyboard, mouse, gamepad (etc) inputs
        ProcessInputs();

        //Clamp the horizontal & vertical input to be between -1 and 1
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        vertical = Mathf.Clamp(vertical, -1f, 1f);
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
        vertical = 0f;
        attackPressed = false;

        readyToClear = false;
    }

    private void ProcessInputs() {
        //Accumulate horizontal & vertical axis input
        horizontal += Input.GetAxis("Horizontal");
        vertical += Input.GetAxis("Vertical");

        //Avoid attacking while pressing a button
        if (!EventSystem.current.IsPointerOverGameObject()) {
            attackPressed = attackPressed || Input.GetButtonDown("Fire1");
        }
    }
}
