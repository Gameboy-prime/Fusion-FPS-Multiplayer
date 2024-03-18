using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseState
{
    public override void EnterState(CharacterMovementHandler movement)
    {
        movement.anime.SetBool("Walking", true);
        
    }

    public override void UpdateState(CharacterMovementHandler movement)
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            movement.characterInputHandler.sprintKeyWasPressed= true;
            ExitState(movement, movement.run);
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            movement.characterInputHandler.crouchKeyWasPressed=true;
            ExitState(movement, movement.crouch);
        }
    }

    void ExitState(CharacterMovementHandler movement, MovementBaseState state)
    {
        movement.anime.SetBool("Walking", false);
        //movement.Switch(state);

    }
}
