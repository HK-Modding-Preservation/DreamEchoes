using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    public IEnumerator<Transition> ControlledIdle()
    {
        Velocity = Vector2.zero;
        Animator.PlayAnimation("Idle");
        while (true)
        {
            if (InputManager.Direction != 0)
            {
                yield return new ToState { State = nameof(ControlledRun) };
            }
            yield return new NoTransition();
        }
    }
}
