﻿using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Stun()
    {
        // StunStart
        Reset();
        PlayStunEffect();
        speak.PlayOneShot(animator.StunWords);
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("StunStart") };

        // StunAir
        BoxCollider2D.offset = Config.StunColliderOffset;
        BoxCollider2D.size = Config.StunColliderSize;
        var velocityXMax = Config.StunVelocityX;
        var velocityX = velocityXMax * -Direction();
        var velocityY = Config.StunVelocityY;
        Velocity = new Vector2(velocityX, velocityY);
        animator.PlayAnimation("StunAir");
        yield return new WaitTill { Condition = Landed };
        animator.PlayLandSound();

        // StunLand
        Velocity = Vector2.zero;
        animator.PlayAnimation("StunLand");
        IEnumerator<Transition> checkHit()
        {
            var currentCount = stunCount;
            while (currentCount == stunCount)
            {
                yield return new NoTransition();
            }
        }
        yield return new CoroutineTransition
        {
            Routines = [
                checkHit(),
                new WaitFor { Seconds = Config.StunDuration },
            ]
        };

        // StunEnd
        BoxCollider2D.offset = originalBoxCollider2DOffset;
        BoxCollider2D.size = originalBoxCollider2DSize;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("StunEnd") };
        stunCount = 0;
        yield return new ToState { State = nameof(Idle) };
    }
}
