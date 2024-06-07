using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Laser()
    {
        // JumpStart
        var jumpRadius = Config.LaserDistance;
        var targetXLeft = Target().Position().x - jumpRadius;
        var targetXRight = Target().Position().x + jumpRadius;
        float targetX;
        if (Mathf.Abs(Position.x - targetXLeft) > Mathf.Abs(Position.x - targetXRight))
        {
            targetX = targetXRight;
        }
        else
        {
            targetX = targetXLeft;
        }
        var velocityX = (targetX - Position.x) * Config.EvadeJumpVelocityXScale;
        var velocityY = Config.EvadeJumpVelocityY;
        if (!FacingTarget())
        {
            yield return new CoroutineTransition
            {
                Routine = Turn()
            };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("JumpStart") };

        // JumpAscend
        Velocity = new Vector2(velocityX, velocityY);
        animator.PlayAnimation("JumpAscend");
        yield return new WaitTill { Condition = () => Velocity.y <= 0 };


        // LaserStart
        Rigidbody2D.gravityScale = 0;
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("LaserBegin") };

        // LaserFly
        animator.PlayAnimation("LaserFly");
        var hand = animator.gameObject.transform.Find("LaserHand").gameObject;
        var aim = hand.transform.Find("LaserAim").gameObject;
        var att = hand.transform.Find("LaserAtt").gameObject;
        IEnumerator<Transition> bodyFollow()
        {
            while (true)
            {
                if (!FacingTarget())
                {
                    yield return new CoroutineTransition { Routine = Turn() };
                }
                var jumpRadius = Config.LaserDistance;
                var targetXLeft = Target().Position().x - jumpRadius;
                var targetXRight = Target().Position().x + jumpRadius;
                float targetX;
                if (targetXLeft < minX || targetXLeft > maxX)
                {
                    targetX = targetXRight;
                }
                else if (targetXRight < minX || targetXRight > maxX)
                {
                    targetX = targetXLeft;
                }
                else if (Mathf.Abs(Position.x - targetXLeft) > Mathf.Abs(Position.x - targetXRight))
                {
                    targetX = targetXRight;
                }
                else
                {
                    targetX = targetXLeft;
                }
                Velocity = new Vector2((targetX - Position.x) * Config.LaserVelocityXScale, 0);
                yield return new NoTransition();
            }
        }
        IEnumerator<Transition> handFollow()
        {
            while (true)
            {
                var handPosition = hand.transform.position;
                var heroPosition = Target().Position();
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
                var newAngle = currentAngle + deltaAngle * Config.LaserHandFollowSpeed * Time.deltaTime;
                hand.transform.localRotation = Quaternion.Euler(0, 0, newAngle);
                yield return new NoTransition();
            }
        }
        aim.SetActive(true);
        yield return new CoroutineTransition
        {
            Routines = [
                new WaitFor { Seconds = 10 },
                bodyFollow(),
                handFollow(),
            ]
        };
        aim.SetActive(false);

        // LaserEnd1
        Rigidbody2D.gravityScale = Config.GravityScale;
        Velocity = new Vector2(0, Config.LaserEndVelY);
        animator.PlayAnimation("LaserEnd1");
        yield return new WaitTill { Condition = Landed };

        // LaserEnd2
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("LaserEnd2") };

        yield return new ToState { State = nameof(Idle) };
    }
}
