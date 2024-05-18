using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> ControlledRun()
    {
    RunStart: var direction = inputManager.Direction;
        if (direction == 0)
        {
            yield return new ToState { State = nameof(ControlledIdle) };
        }
        var velocityX = Config.RunVelocityX * direction;
        if (direction != Direction())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        animator.PlayAnimation("RunStart");
        var startDuration = animator.ClipLength("RunStart");
        var timer = 0f;
        while (timer < startDuration)
        {
            var newDirection = inputManager.Direction;
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
        animator.PlayAnimation("Run");
        while (true)
        {
            var newDirection = inputManager.Direction;
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
        animator.PlayAnimation("RunEnd");
        var endDuration = animator.ClipLength("RunEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var newDirection = inputManager.Direction;
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
