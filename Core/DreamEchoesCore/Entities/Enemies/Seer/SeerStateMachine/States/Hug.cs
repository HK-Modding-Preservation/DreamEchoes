using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Hug()
    {
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }

        var direction = Direction();
        var velocityX = config.HugVelocityX * -direction;
        var velocityY = config.HugVelocityY;
        Transition updater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            var currentVelocityY = Mathf.Lerp(0, velocityY, normalizedTime);
            Velocity = new Vector2(currentVelocityX, currentVelocityY);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("HugStart", updater)
        };

        Rigidbody2D.gravityScale = 0;
        Velocity = Vector2.zero;
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("Hug", updater)
        };

        Rigidbody2D.gravityScale = config.GravityScale;
        animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = () => Landed() };
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = nameof(Idle) };
    }
}
