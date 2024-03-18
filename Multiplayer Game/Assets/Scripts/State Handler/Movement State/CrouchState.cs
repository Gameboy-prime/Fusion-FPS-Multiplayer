using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MovementBaseState
{
    public override void EnterState(CharacterMovementHandler movement)
    {
        movement.anime.SetBool("Crouching", true);
        
    }

    public override void UpdateState(CharacterMovementHandler movement)
    {
        if(Input.GetKeyUp(KeyCode.RightShift))
        {
            movement.characterInputHandler.sprintKeyWasPressed = false;
            movement.characterInputHandler.crouchKeyWasPressed = false;
            if (movement.prototype.Velocity.magnitude > 0.1f)
            {
                ExitState(movement, movement.walk);
            }
            else
            {
                ExitState(movement, movement.idle);
            }

        }
        
        
    }

    void ExitState(CharacterMovementHandler movement, MovementBaseState state)
    {
        movement.anime.SetBool("Crouching", false);
        movement.characterInputHandler.crouchKeyWasPressed= false;
        movement.Switch(state);

    }
}
