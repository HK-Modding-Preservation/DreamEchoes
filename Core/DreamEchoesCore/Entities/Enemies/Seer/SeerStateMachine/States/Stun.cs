using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Stun()
    {
        // StunStart
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("StunStart") };

        // StunAir
        BoxCollider2D.offset = config.StunColliderOffset;
        BoxCollider2D.size = config.StunColliderSize;
        var velocityX = config.StunVelocityX * -Direction();
        var velocityY = config.StunVelocityY;
        Velocity = new Vector2(velocityX, velocityY);
        animator.PlayAnimation("StunAir");
        yield return new WaitTill { Condition = Landed };

        // StunLand
        Velocity = Vector2.zero;
        animator.PlayAnimation("StunLand");
        yield return new WaitFor { Seconds = config.StunDuration };

        // StunEnd
        BoxCollider2D.offset = originalBoxCollider2DOffset;
        BoxCollider2D.size = originalBoxCollider2DSize;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("StunEnd") };
        yield return new ToState { State = nameof(Idle) };
    }
}
