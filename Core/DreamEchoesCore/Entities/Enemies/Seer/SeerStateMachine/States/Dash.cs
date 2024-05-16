using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Dash : State<SeerStateMachine>
{
    public override IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            yield return new CoroutineTransition { Routine = StateMachine.Turn() };
        }

        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.DashVelocityX * direction;
        StateMachine.BoxCollider2D.offset = StateMachine.Config.DashStartColliderOffset;
        StateMachine.BoxCollider2D.size = StateMachine.Config.DashStartColliderSize;
        Transition startUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = StateMachine.Animator.PlayAnimation("DashStart", startUpdater)
        };

        StateMachine.BoxCollider2D.offset = StateMachine.Config.DashColliderOffset;
        StateMachine.BoxCollider2D.size = StateMachine.Config.DashColliderSize;
        StateMachine.Rigidbody2D.gravityScale = 0;
        StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Dash");
        yield return new WaitFor { Seconds = StateMachine.Config.DashDuration };

        StateMachine.BoxCollider2D.offset = StateMachine.Config.DashEndColliderOffset;
        StateMachine.BoxCollider2D.size = StateMachine.Config.DashEndColliderSize;
        StateMachine.Rigidbody2D.gravityScale = StateMachine.Config.GravityScale;
        Transition endUpdater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, normalizedTime);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = StateMachine.Animator.PlayAnimation("DashEnd", endUpdater)
        };
        StateMachine.BoxCollider2D.offset = StateMachine.OriginalBoxCollider2DOffset;
        StateMachine.BoxCollider2D.size = StateMachine.OriginalBoxCollider2DSize;
        yield return new ToState { State = typeof(Idle) };
    }
}
