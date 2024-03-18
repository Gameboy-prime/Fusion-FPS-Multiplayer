using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MovementBaseState
{
    
    public override void EnterState(CharacterMovementHandler movement)
    {
        
    }

    public override void UpdateState(CharacterMovementHandler movement)
    {
        if(movement.prototype.Velocity.magnitude>.2f)
        {
            if (Input.GetKey(KeyCode.RightShift))
            {
                movement.characterInputHandler.sprintKeyWasPressed = true;
                movement.Switch(movement.run);
            }
            else
            {
                movement.Switch(movement.walk);
            }
            //Get Crouch input

            if (Input.GetKeyDown(KeyCode.C))
            {
                movement.characterInputHandler.crouchKeyWasPressed = true;
                movement.Switch(movement.crouch);

            }

        }

    }
        
}
