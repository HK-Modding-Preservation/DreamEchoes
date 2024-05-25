using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Dash()
    {
        // DashStart
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
            Routine = animator.PlayAnimation("DashStart", startUpdater)
        };

        // Dash
        BoxCollider2D.offset = Config.DashColliderOffset;
        BoxCollider2D.size = Config.DashColliderSize;
        Rigidbody2D.gravityScale = 0;
        Velocity = new Vector2(velocityX, 0);
        animator.PlayAnimation("Dash");
        yield return new CoroutineTransition
        {
            Routines = [
                new WaitTill
                {
                    Condition = () =>
                    {
                        var distance = Mathf.Abs(Target().Position().x - Position.x);
                        return !FacingTarget() && distance > Config.DashAbortDistance;
                    }
                },
                new WaitFor { Seconds = Config.DashDuration },
            ]
        };

        // DashEnd
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
            Routine = animator.PlayAnimation("DashEnd", endUpdater)
        };
        BoxCollider2D.offset = originalBoxCollider2DOffset;
        BoxCollider2D.size = originalBoxCollider2DSize;
        Velocity = Vector2.zero;
        yield return new ToState { State = nameof(Idle) };
    }
}
