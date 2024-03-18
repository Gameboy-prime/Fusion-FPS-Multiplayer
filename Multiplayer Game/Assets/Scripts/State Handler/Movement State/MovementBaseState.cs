using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBaseState 
{
    public abstract void EnterState(CharacterMovementHandler movement);

    public abstract void UpdateState(CharacterMovementHandler movement);
}
