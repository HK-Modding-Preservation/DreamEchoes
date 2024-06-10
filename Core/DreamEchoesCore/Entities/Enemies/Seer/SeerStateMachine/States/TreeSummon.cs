using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> TreeSummon()
    {
        // JumpStart
        var jumpRadiusMin = Config.EvadeJumpRadiusMin;
        var jumpRadiusMax = Config.EvadeJumpRadiusMax;
        var jumpRadius = 0;
        var targetPosX = (minX + maxX) / 2;
        var targetXLeft = targetPosX - jumpRadius;
        var targetXRight = targetPosX + jumpRadius;
        float targetX;
        if (Mathf.Abs(Position.x - targetXLeft) < Mathf.Abs(Position.x - targetXRight))
        {
            targetX = targetXRight;
        }
        else
        {
            targetX = targetXLeft;
        }
        var velocityX = (targetX - Position.x) * Config.EvadeJumpVelocityXScale;
        var velocityY = Config.EvadeJumpVelocityY;
        if (Mathf.Sign(velocityX) != Direction())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpStart") };

        // JumpAscend
        Velocity = new Vector2(velocityX, velocityY);
        animator.PlayAnimation("JumpAscend");
        yield return new WaitTill { Condition = () => Velocity.y <= 0 };

        // JumpDescend
        animator.PlayAnimation("JumpDescend");
        yield return new WaitTill { Condition = Landed };
        animator.PlayLandSound();

        // JumpEnd
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpEnd") };

        // speak.PlayOneShot(animator.Slash1Words);
        // animator.PlaySlash2Sound();
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("TreeSummon") };

        UnlockStun();
        yield return new ToState { State = nameof(Idle) };
    }
}
