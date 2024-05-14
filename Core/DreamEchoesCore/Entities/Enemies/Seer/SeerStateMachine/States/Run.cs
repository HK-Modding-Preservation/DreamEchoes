using RingLib.StateMachine;
using RingLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Run : State<SeerStateMachine>
{
    private RandomSelector<Type> randomSelector = new([
        new(typeof(Dash), 1, 2),
        new(typeof(Slash), 1, 2)
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

        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.RunVelocityX * direction;

        void startUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
        }
        yield return new CoroutineTransition
        {
            Routine = StateMachine.Animator.PlayAnimation("RunStart", startUpdater)
        };

        StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Run");
        yield return new WaitFor { Seconds = StateMachine.Config.RunDuration };

        void endUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, normalizedTime);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
        }
        yield return new CoroutineTransition
        {
            Routine = StateMachine.Animator.PlayAnimation("RunEnd", endUpdater)
        };

        yield return new ToState { State = typeof(Attack) };
    }
}
