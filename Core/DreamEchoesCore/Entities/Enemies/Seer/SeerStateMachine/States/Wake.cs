using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Wake()
    {
        if (FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        animator.PlayAnimation("Idle");

        float distance()
        {
            return Mathf.Abs(Target().Position().x - Position.x);
        }
        yield return new WaitTill { Condition = () => distance() < Config.WakeDistance };
        yield return new ToState { State = nameof(Idle) };
    }
}
