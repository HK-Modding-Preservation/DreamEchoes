using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;

internal class ControlledIdle : State<SeerStateMachine>
{
    public override IEnumerator<Transition> Routine()
    {
        StateMachine.Velocity = Vector2.zero;
        StateMachine.Animator.PlayAnimation("Idle");
        while (true)
        {
            if (StateMachine.InputManager.Direction != 0)
            {
                yield return new ToState { State = typeof(ControlledRun) };
            }
            yield return new NoTransition();
        }
    }
}
