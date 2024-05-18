using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Parry()
    {
        // ParryStart
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("ParryStart") };

        // ParryHold
        animator.PlayAnimation("ParryHold");
        parryed = false;
        yield return new CoroutineTransition
        {
            Routines = [
                new WaitTill { Condition = () => parryed },
                new WaitFor { Seconds = Config.ParryDuration },
            ]
        };

        // ParryCounter
        if (parryed)
        {
            if (!FacingTarget())
            {
                yield return new CoroutineTransition { Routine = Turn() };
            }
            var velocityX = Config.ParryVelocityX * Direction();
            Transition updater(float normalizedTime)
            {
                var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
                Velocity = new Vector2(currentVelocityX, 0);
                return new NoTransition();
            }
            yield return new CoroutineTransition
            {
                Routine = animator.PlayAnimation("ParryCounter", updater)
            };
            Velocity = Vector2.zero;
            yield return new CoroutineTransition
            {
                Routine = animator.PlayAnimation("ParryEnd")
            };
        }
        yield return new ToState { State = nameof(Idle) };
    }
}
