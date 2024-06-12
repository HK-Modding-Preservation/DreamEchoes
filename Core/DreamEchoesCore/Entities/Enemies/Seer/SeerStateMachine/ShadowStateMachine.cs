using RingLib.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore.Components;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class ShadowStateMachine : StateMachine
{
    public float originaY;
    public bool flyok = false;

    private GameObject hand;
    private GameObject aim;
    private GameObject att;

    public ShadowM manager;

    IEnumerator<Transition> HandFollowV2(Func<Vector3> targetPos, bool isaim)
    {
        while (true)
        {
            var handPosition = hand.transform.position;
            var heroPosition = targetPos();

            var localScale = gameObject.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            if (heroPosition.x > transform.position.x)
            {
                localScale.x *= -1;
            }
            gameObject.transform.localScale = localScale;

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
            var newAngle = currentAngle + deltaAngle * 30 * Time.deltaTime;
            hand.transform.localRotation = Quaternion.Euler(0, 0, angle);
            if (isaim)
            {
                aim.SetActive(true);
                att.SetActive(false);
            }
            else
            {
                att.SetActive(true);
                aim.SetActive(false);
            }
            yield return new NoTransition();
        }
    }

    IEnumerator<Transition> HandFollow(Func<Vector3> targetPos)
    {
        while (true)
        {
            var handPosition = hand.transform.position;
            var heroPosition = targetPos();

            var localScale = gameObject.transform.localScale;
            localScale.x = Mathf.Abs(localScale.x);
            if (heroPosition.x > transform.position.x)
            {
                localScale.x *= -1;
            }
            gameObject.transform.localScale = localScale;

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
            var newAngle = currentAngle + deltaAngle * 30 * Time.deltaTime;
            hand.transform.localRotation = Quaternion.Euler(0, 0, newAngle);
            yield return new NoTransition();
        }
    }

    public bool ready = false;
    public bool plsattack = false;
    public Vector3 target;

    IEnumerator<Transition> DoAttack()
    {
        while (true)
        {
            ready = true;
            while (!plsattack)
            {
                var handPosition = hand.transform.position;
                var heroPosition = HeroController.instance.gameObject.transform.position;

                var localScale = gameObject.transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);
                if (heroPosition.x > transform.position.x)
                {
                    localScale.x *= -1;
                }
                gameObject.transform.localScale = localScale;

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
                var newAngle = currentAngle + deltaAngle * 30 * Time.deltaTime;
                hand.transform.localRotation = Quaternion.Euler(0, 0, newAngle);
                yield return new NoTransition();
            }

            plsattack = false;
            RingLib.Log.LogInfo("", "Laser Attack Aim");
            IEnumerator<Transition> aimwait()
            {
                var seerStateMachine = manager.Seer.GetComponent<SeerStateMachine>();
                var currentIdleCount = seerStateMachine.IdleCount;
                while (currentIdleCount == seerStateMachine.IdleCount)
                {
                    yield return new NoTransition();
                }
            }
            yield return new CoroutineTransition
            {
                Routines = [
                    // bodyFollow(),
                    HandFollowV2(() => target, true),
                    new WaitFor { Seconds = 1f }
                    //aimwait()
                ]
            };
            //var lockedPosition = positionWithOffset();
            var sound = manager.Seer.GetComponent<SeerStateMachine>().animator.LaserFire;
            var audioSource = GetComponent<AudioSource>();
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(sound);
            //animator.PlayLaserFire();
            //Velocity = Vector2.zero;
            RingLib.Log.LogInfo("", "Laser Attack Laser");
            yield return new CoroutineTransition
            {
                Routines = [
                   HandFollowV2(() => target, false),
                    //ddPrevent(),
                    new WaitFor { Seconds = 0.2f }
                ]
            };
            att.SetActive(false);
            aim.SetActive(false);
        }
    }

    private IEnumerator<Transition> Ascend()
    {
        while (!flyok)
        {
            yield return new NoTransition();
        }
        var upVel = UnityEngine.Random.Range(5f, 7f);
        var rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, upVel);
        while (transform.position.y < originaY - 1)
        {
            yield return new NoTransition();
        }
        rigid.velocity = Vector2.zero;
    }

    [State]
    private IEnumerator<Transition> Begin()
    {
        yield return new CoroutineTransition
        {
            Routines = [
                Ascend(),
                HandFollow(() => HeroController.instance.transform.position)
            ]
        };
        yield return new ToState { State = nameof(Follow) };
    }

    [State]
    private IEnumerator<Transition> Follow()
    {
        yield return new CoroutineTransition
        {
            Routine = DoAttack()
        };
    }

    public ShadowStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        var animation = gameObject.transform.Find("Animation").gameObject;
        UnityEngine.Animator animator = animation.GetComponent<UnityEngine.Animator>();
        animator.Play("Idle", -1, UnityEngine.Random.Range(0f, 1f));

        hand = animation.transform.Find("Hand").gameObject;
        aim = hand.transform.Find("LaserAim").gameObject;
        att = hand.transform.Find("LaserAtt").gameObject;

        var playerDamager = gameObject.GetComponentInChildren<PlayerDamager>();
        Destroy(playerDamager);
    }
}
