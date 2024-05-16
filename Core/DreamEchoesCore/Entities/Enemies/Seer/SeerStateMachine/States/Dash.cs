using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Dash()
    {
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }

        var direction = Direction();
        var velocityX = config.DashVelocityX * direction;
        BoxCollider2D.offset = config.DashStartColliderOffset;
        BoxCollider2D.size = config.DashStartColliderSize;
        Transition startUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("DashStart", startUpdater)
        };

        BoxCollider2D.offset = config.DashColliderOffset;
        BoxCollider2D.size = config.DashColliderSize;
        Rigidbody2D.gravityScale = 0;
        Velocity = new Vector2(velocityX, 0);
        animator.PlayAnimation("Dash");
        yield return new WaitFor { Seconds = config.DashDuration };

        BoxCollider2D.offset = config.DashEndColliderOffset;
        BoxCollider2D.size = config.DashEndColliderSize;
        Rigidbody2D.gravityScale = config.GravityScale;
        Transition endUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, normalizedTime);
            Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = animator.PlayAnimation("DashEnd", endUpdater)
        };
        BoxCollider2D.offset = originalBoxCollider2DOffset;
        BoxCollider2D.size = originalBoxCollider2DSize;
        yield return new ToState { State = nameof(Idle) };
    }
}
