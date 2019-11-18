using UnityEngine;

public abstract class DemoMovementSO : ScriptableObject {

    public abstract void UpdateMovementData(DemoAutoMovementController controller);

    public virtual void Move(Transform objTransform, DemoAutoMovementController controller) {

        objTransform.position = Vector2.MoveTowards(objTransform.position, controller.targetPosition, controller.speed * Time.deltaTime);
        CheckDirection(objTransform, controller);
    }

    protected virtual void CheckDirection(Transform objTransform, DemoAutoMovementController controller) {

        if (objTransform.position.x > controller.targetPosition.x)
            controller.curDirection = -1;
        else if (objTransform.position.x < controller.targetPosition.x)
            controller.curDirection = 1;

        //Calculate the desired velocity based on inputs
        float xVelocity = controller.speed * controller.curDirection;

        //If the sign of the velocity and direction don't match, flip the character
        if (xVelocity * controller.facingDirection < 0f)
            FlipCharacterDirection(objTransform, controller);
    }

    protected virtual void FlipCharacterDirection(Transform objTransform, DemoAutoMovementController controller) {

        //Turn the character by flipping the direction
        controller.facingDirection *= -1;

        //Record the current scale
        Vector3 scale = objTransform.localScale;

        //Set the X scale to be the original times the direction
        scale.x = controller.originalXScale * controller.facingDirection;

        //Apply the new scale
        objTransform.localScale = scale;
    }
}
