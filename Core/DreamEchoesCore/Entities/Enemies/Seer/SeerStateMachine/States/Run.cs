
using DreamEchoesCore.RingLib.StateMachine;
using System.Collections;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Run : State<SeerStateMachine>
{
    public override Type? Enter()
    {
        StartCoroutine(Routine());
        return null;
    }
    private IEnumerator Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            StateMachine.Turn();
        }
        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.RunVelocityX * direction;
        var startDuration = StateMachine.Animator.PlayAnimation("RunStart");
        var timer = 0f;
        while (timer < startDuration)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, timer / startDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return 0;
        }
        StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Run");
        yield return StateMachine.Config.RunDuration;
        var endDuration = StateMachine.Animator.PlayAnimation("RunEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, timer / endDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return 0;
        }
        yield return typeof(Idle);
    }
}