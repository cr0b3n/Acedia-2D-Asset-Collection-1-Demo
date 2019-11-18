using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Demo Vertical Horizontal Movement", menuName = "Acedia Demo/ Movement/ Vertical Horizontal Movement")]
public class DemoVerticalHorizontalMoveSO : DemoMovementSO {

    public float minXPosition = -7f;
    public float maxXPosition = 7f;
    public float minYPosition = -1.2f;
    public float maxYPosition = 4f;

    public override void UpdateMovementData(DemoAutoMovementController controller) {
        controller.UpdateBehaviour(new Vector2(Random.Range(minXPosition, maxXPosition), Random.Range(minYPosition, maxYPosition)));
    }
}
