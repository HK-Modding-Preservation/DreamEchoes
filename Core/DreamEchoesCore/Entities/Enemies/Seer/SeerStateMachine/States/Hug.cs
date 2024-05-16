using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Hug : State<SeerStateMachine>
{
    public override IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            yield return new CoroutineTransition { Routine = StateMachine.Turn() };
        }

        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.HugVelocityX * -direction;
        var velocityY = StateMachine.Config.HugVelocityY;
        Transition updater(float normalizedTime)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
            var currentVelocityY = Mathf.Lerp(0, velocityY, normalizedTime);
            StateMachine.Velocity = new Vector2(currentVelocityX, currentVelocityY);
            return new NoTransition();
        }
        yield return new CoroutineTransition
        {
            Routine = StateMachine.Animator.PlayAnimation("HugStart", updater)
        };

        StateMachine.Rigidbody2D.gravityScale = 0;
        StateMachine.Velocity = Vector2.zero;
        yield return new CoroutineTransition
        {
            Routine = StateMachine.Animator.PlayAnimation("Hug", updater)
        };

        StateMachine.Rigidbody2D.gravityScale = StateMachine.Config.GravityScale;
        StateMachine.Animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = () => StateMachine.Landed() };
        StateMachine.Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = StateMachine.Animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = typeof(Idle) };
    }
}
