using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Demo Horizontal Movement", menuName = "Acedia Demo/ Movement/ Horizontal Movement")]
public class DemoHorizontalMoveSO : DemoMovementSO {

    public float minXPosition = -7;
    public float maxXPosition = 7;

    public override void UpdateMovementData(DemoAutoMovementController controller) {
        controller.UpdateBehaviour(new Vector2(Random.Range(minXPosition, maxXPosition), controller.transform.position.y));
    }
}
