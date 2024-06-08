using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Run()
    {
        // RunStart
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        var direction = Direction();
        var velocityX = Config.RunVelocityX * direction;
        Transition startUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("RunStart", startUpdater)
        };

        // Run
        animator.PlayRunSound();
        Velocity = new Vector2(velocityX, 0);
        animator.PlayAnimation("Run");
        yield return new WaitFor { Seconds = Config.RunDuration };
        animator.StopRunSound();

        // RunEnd
        Transition endUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, normalizedTime);
            Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("RunEnd", endUpdater)
        };
        Velocity = Vector2.zero;
        yield return new ToState { State = nameof(Attack) };
    }
}
