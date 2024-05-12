using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;

internal class ControlledRun : State<SeerStateMachine>
{
    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }
    private IEnumerator<Transition> Routine()
    {
    RunStart: var direction = StateMachine.InputManager.Direction;
        if (direction == 0)
        {
            yield return new ToState { State = typeof(ControlledIdle) };
        }
        var velocityX = StateMachine.Config.RunVelocityX * direction;
        if (direction != StateMachine.Direction())
        {
            StateMachine.Turn();
        }
        var startDuration = StateMachine.Animator.PlayAnimation("RunStart");
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
            yield return new CurrentState();
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
            yield return new CurrentState();
        }
        var endDuration = StateMachine.Animator.PlayAnimation("RunEnd");
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
            yield return new CurrentState();
        }
        yield return new ToState { State = typeof(ControlledIdle) };
    }
}
