using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;

internal class ControlledRun : State<SeerStateMachine>
{
    public override IEnumerator<Transition> Routine()
    {
    RunStart: var direction = StateMachine.InputManager.Direction;
        if (direction == 0)
        {
            yield return new ToState { State = typeof(ControlledIdle) };
        }
        var velocityX = StateMachine.Config.RunVelocityX * direction;
        if (direction != StateMachine.Direction())
        {
            yield return new CoroutineTransition { Routine = StateMachine.Turn() };
        }
        StateMachine.Animator.PlayAnimation("RunStart");
        var startDuration = StateMachine.Animator.ClipLength("RunStart");
        var timer = 0f;
        while (timer < startDuration)
        {
            var newDirection = StateMachine.InputManager.Direction;
            if (newDirection != direction)
            {
                yield return new ToState { State = typeof(ControlledIdle) };
            }
            var currentVelocityX = Mathf.Lerp(0, velocityX, timer / startDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new NoTransition();
        }
    Run: StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Run");
        while (true)
        {
            var newDirection = StateMachine.InputManager.Direction;
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
        StateMachine.Animator.PlayAnimation("RunEnd");
        var endDuration = StateMachine.Animator.ClipLength("RunEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var newDirection = StateMachine.InputManager.Direction;
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
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new NoTransition();
        }
        yield return new ToState { State = typeof(ControlledIdle) };
    }
}
