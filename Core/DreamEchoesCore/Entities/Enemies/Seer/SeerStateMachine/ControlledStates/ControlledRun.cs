using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> ControlledRun()
    {
    RunStart: var direction = InputManager.Direction;
        if (direction == 0)
        {
            yield return new ToState { State = nameof(ControlledIdle) };
        }
        var velocityX = Config.RunVelocityX * direction;
        if (direction != Direction())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Animator.PlayAnimation("RunStart");
        var startDuration = Animator.ClipLength("RunStart");
        var timer = 0f;
        while (timer < startDuration)
        {
            var newDirection = InputManager.Direction;
            if (newDirection != direction)
            {
                yield return new ToState { State = nameof(ControlledIdle) };
            }
            var currentVelocityX = Mathf.Lerp(0, velocityX, timer / startDuration);
            Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new NoTransition();
        }
    Run: Velocity = new Vector2(velocityX, 0);
        Animator.PlayAnimation("Run");
        while (true)
        {
            var newDirection = InputManager.Direction;
            if (newDirection == 0)
            {
                break;
            }
            if (newDirection != direction)
            {
                goto RunStart;
            }
            yield return new NoTransition();
        }
        Animator.PlayAnimation("RunEnd");
        var endDuration = Animator.ClipLength("RunEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var newDirection = InputManager.Direction;
            if (newDirection != 0)
            {
                if (newDirection == direction)
                {
                    goto Run;
                }
                else
                {
                    goto RunStart;
                }
            }
            var currentVelocityX = Mathf.Lerp(velocityX, 0, timer / endDuration);
            Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new NoTransition();
        }
        yield return new ToState { State = nameof(ControlledIdle) };
    }
}
