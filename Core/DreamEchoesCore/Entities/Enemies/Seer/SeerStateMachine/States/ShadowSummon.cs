using RingLib.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    IEnumerator<Transition> handFollowVerySlow(GameObject hand, Func<Vector3> targetPos)
    {
        while (true)
        {
            if (!FacingTarget())
            {
                yield return new CoroutineTransition { Routine = Turn() };
            }
            var handPosition = hand.transform.position;
            var heroPosition = targetPos();
            if (gameObject.transform.localScale.x < 0)
            {
                heroPosition.x = 2 * gameObject.transform.position.x - heroPosition.x;
            }
            var direction = (heroPosition - handPosition).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float normalizeAngle(float angle)
            {
                while (angle < -180)
                {
                    angle += 360;
                }
                while (angle > 180)
                {
                    angle -= 360;
                }
                return angle;
            }
            angle = normalizeAngle(angle - 180 - 8);
            var currentAngle = normalizeAngle(hand.transform.localRotation.eulerAngles.z);
            var deltaAngle = angle - currentAngle;
            var speed = 1;
            var newAngle = currentAngle + deltaAngle * speed * Time.deltaTime;
            hand.transform.localRotation = Quaternion.Euler(0, 0, newAngle);
            yield return new NoTransition();
        }
    }

    IEnumerator<Transition> handSweep(GameObject hand)
    {
        hand.transform.localEulerAngles = new Vector3(0, 0, -30);
        while (true)
        {
            float normalizeAngle(float angle)
            {
                while (angle < -180)
                {
                    angle += 360;
                }
                while (angle > 180)
                {
                    angle -= 360;
                }
                return angle;
            }
            var currentAngle = normalizeAngle(hand.transform.localRotation.eulerAngles.z);
            var deltaAngle = 1;
            var speed = Config.LaserSweepSpeed;
            var newAngle = currentAngle + deltaAngle * speed * Time.deltaTime;
            if (newAngle > 90)
            {
                yield break;
            }
            hand.transform.localRotation = Quaternion.Euler(0, 0, newAngle);
            yield return new NoTransition();
        }
    }

    [State]
    private IEnumerator<Transition> ShadowSummon()
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

        // LaserStart
        speak.PlayOneShot(animator.ShadowWord);
        Rigidbody2D.gravityScale = 0;
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("LaserBegin") };

        // LaserFly
        var hand = animator.gameObject.transform.Find("LaserHand").gameObject;
        var aim = hand.transform.Find("LaserAim").gameObject;
        var att = hand.transform.Find("LaserAtt").gameObject;
        var hit = animator.LaserHit;
        aim.SetActive(false);
        att.SetActive(false);
        hit.SetActive(false);
        animator.PlayAnimation("LaserFly");

        RingLib.Log.LogInfo("", "Laser Attack Wait");
        hand.transform.localEulerAngles = new Vector3(0, 0, -30);
        /*
        yield return new CoroutineTransition
        {
            Routines = [
                // bodyFollow(),
                //handFollow(hand, () => Target().Position()),
                new WaitFor { Seconds = Config.LaserAttackIdleWait }
            ]
        };
        */
        /*
        aim.SetActive(true);
        RingLib.Log.LogInfo("", "Laser Attack Aim");
        yield return new CoroutineTransition
        {
            Routines = [
                //  bodyFollow(),
                handFollow(hand, () => Target().Position()),
                new WaitFor { Seconds = Config.LaserAttackAimWait }
           ]
        };*/
        aim.SetActive(false);
        att.SetActive(true);
        animator.PlayLaserFire();
        Velocity = Vector2.zero;
        RingLib.Log.LogInfo("", "Laser Attack Laser");
        /*
        yield return new CoroutineTransition
        {
            Routines = [
                handFollowVerySlow(hand, () => Target().Position()),
                //ddPrevent(),
                new WaitFor { Seconds = 4 }
            ]
        };
        */
        yield return new CoroutineTransition
        {
            Routines = [
                handSweep(hand)
            ]
        };
        yield return new CoroutineTransition { Routine = Turn() };
        yield return new CoroutineTransition
        {
            Routines = [
                handSweep(hand)
            ]
        };
        att.SetActive(false);
        hit.SetActive(false);

        // LaserEnd1
        Rigidbody2D.gravityScale = Config.GravityScale;
        Velocity = new Vector2(0, Config.LaserBodyEndVelY);
        animator.PlayAnimation("LaserEnd1");
        yield return new WaitTill { Condition = Landed };
        animator.PlayLandSound();

        // LaserEnd2
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("LaserEnd2") };

        UnlockStun();
        yield return new ToState { State = nameof(Idle) };
    }
}
