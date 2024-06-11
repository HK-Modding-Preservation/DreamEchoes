using RingLib.StateMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class ShadowStateMachine : StateMachine
{
    private GameObject hand;

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

    [State]
    private IEnumerator<Transition> Begin()
    {
        yield return new ToState { State = nameof(Follow) };
    }

    [State]
    private IEnumerator<Transition> Follow()
    {
        yield return new CoroutineTransition
        {
            Routine = HandFollow(() => HeroController.instance.transform.position)
        };
    }

    public ShadowStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        var animation = gameObject.transform.Find("Animation").gameObject;
        Animator animator = animation.GetComponent<Animator>();
        animator.Play("Idle", -1, UnityEngine.Random.Range(0f, 1f));

        hand = animation.transform.Find("Hand").gameObject;
    }
}
