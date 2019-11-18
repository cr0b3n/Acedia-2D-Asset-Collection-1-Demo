using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Set1Animation : MonoBehaviour {

    private Animator animator;
    private DemoInput4D input;
    private int attackAnimID;

    private void Start() {
        animator = GetComponent<Animator>();
        input = GetComponent<DemoInput4D>();
        attackAnimID = Animator.StringToHash("attack");
    }

    private void Update() {
        animator.SetBool(attackAnimID, input.attackPressed);
    }
}
