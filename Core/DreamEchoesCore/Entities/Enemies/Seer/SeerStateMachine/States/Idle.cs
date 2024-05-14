using RingLib.StateMachine;
using RingLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Idle : State<SeerStateMachine>
{
    private RandomSelector<Type> randomSelector = new([
        new(typeof(Run), 1, 2),
        new(typeof(EvadeJump), 1, 2)
    ]);

    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }

    private IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            yield return new CoroutineTransition { Routine = StateMachine.Turn() };
        }
        StateMachine.Velocity = Vector2.zero;
        StateMachine.Animator.PlayAnimation("Idle");
        yield return new WaitFor { Seconds = StateMachine.Config.IdleDuration };
        yield return new ToState { State = randomSelector.Get() };
    }
}
