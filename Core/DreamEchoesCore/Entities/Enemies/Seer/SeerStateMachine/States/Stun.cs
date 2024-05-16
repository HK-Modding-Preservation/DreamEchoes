using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Stun()
    {
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }

        // pls also consider air stun and test it!

        var direction = Direction();
        var velocityX = Config.HugVelocityX * -direction;
        var velocityY = Config.HugVelocityY;
        Transition updater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            var currentVelocityY = Mathf.Lerp(0, velocityY, normalizedTime);
            Velocity = new Vector2(currentVelocityX, currentVelocityY);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = Animator.PlayAnimation("HugStart", updater)
        };

        Rigidbody2D.gravityScale = 0;
        Velocity = Vector2.zero;
        yield return new CoroutineTransition
        {
            Routine = Animator.PlayAnimation("Hug", updater)
        };

        Rigidbody2D.gravityScale = Config.GravityScale;
        Animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = () => Landed() };
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = Animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = nameof(Idle) };
    }
}
