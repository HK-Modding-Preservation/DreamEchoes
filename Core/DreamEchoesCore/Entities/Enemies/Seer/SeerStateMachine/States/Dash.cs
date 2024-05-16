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
        var velocityX = Config.DashVelocityX * direction;
        BoxCollider2D.offset = Config.DashStartColliderOffset;
        BoxCollider2D.size = Config.DashStartColliderSize;
        Transition startUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = Animator.PlayAnimation("DashStart", startUpdater)
        };

        BoxCollider2D.offset = Config.DashColliderOffset;
        BoxCollider2D.size = Config.DashColliderSize;
        Rigidbody2D.gravityScale = 0;
        Velocity = new Vector2(velocityX, 0);
        Animator.PlayAnimation("Dash");
        yield return new WaitFor { Seconds = Config.DashDuration };

        BoxCollider2D.offset = Config.DashEndColliderOffset;
        BoxCollider2D.size = Config.DashEndColliderSize;
        Rigidbody2D.gravityScale = Config.GravityScale;
        Transition endUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, normalizedTime);
            Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = Animator.PlayAnimation("DashEnd", endUpdater)
        };
        BoxCollider2D.offset = OriginalBoxCollider2DOffset;
        BoxCollider2D.size = OriginalBoxCollider2DSize;
        yield return new ToState { State = nameof(Idle) };
    }
}
