using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> EvadeJump()
    {
        // JumpStart
        var jumpRadiusMin = config.EvadeJumpRadiusMin;
        var jumpRadiusMax = config.EvadeJumpRadiusMax;
        var jumpRadius = UnityEngine.Random.Range(jumpRadiusMin, jumpRadiusMax);
        var targetXLeft = Target().Position().x - jumpRadius;
        var targetXRight = Target().Position().x + jumpRadius;
        float targetX;
        if (Mathf.Abs(Position.x - targetXLeft) < Mathf.Abs(Position.x - targetXRight))
        {
            targetX = targetXRight;
        }
        else
        {
            targetX = targetXLeft;
        }
        var velocityX = (targetX - Position.x) * config.EvadeJumpVelocityXScale;
        if (Mathf.Sign(velocityX) != Direction())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpStart") };

        // JumpAscend
        Velocity = new Vector2(velocityX, config.EvadeJumpVelocityY);
        animator.PlayAnimation("JumpAscend");
        yield return new WaitTill { Condition = () => Velocity.y <= 0 };

        // JumpDescend
        animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = () => Landed() };

        // JumpEnd
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpEnd") };
        yield return new ToState { State = nameof(Attack) };
    }
}
