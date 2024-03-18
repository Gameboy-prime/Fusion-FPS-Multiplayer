using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseState
{
    public override void EnterState(CharacterMovementHandler movement)
    {
        movement.anime.SetBool("Running", true);
    }

    public override void UpdateState(CharacterMovementHandler movement)
    {
        if(Input.GetKeyUp(KeyCode.RightShift))
        {
            movement.characterInputHandler.sprintKeyWasPressed = false;
            ExitState(movement, movement.walk);

        }
        else if(movement.prototype.Velocity.magnitude<0.1f)
        {
            ExitState(movement, movement.idle);
        }
        
    }

    void ExitState(CharacterMovementHandler movement, MovementBaseState state)
    {
        movement.anime.SetBool("Running", false);
        movement.characterInputHandler.sprintKeyWasPressed=false;
        movement.Switch(state);

    }
}
