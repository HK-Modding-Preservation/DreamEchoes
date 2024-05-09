using RingLib.StateMachine;
using RingLib.StateMachine.Transition;
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
        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.RunVelocityX * direction;
        var startDuration = StateMachine.Animator.PlayAnimation("RunStart");
        var timer = 0f;
        while (timer < startDuration)
        {
            var currentVelocityX = Mathf.Lerp(0, velocityX, timer / startDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new CurrentState();
        }
        StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Run");
        var inputActions = StateMachine.InputActions;
        while (inputActions.left.IsPressed || inputActions.right.IsPressed)
        {
            direction = inputActions.left.IsPressed ? -1 : 1;
            if (StateMachine.Direction() != direction)
            {
                StateMachine.Turn();
            }
            yield return new CurrentState();
        }
        var endDuration = StateMachine.Animator.PlayAnimation("RunEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, timer / endDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new CurrentState();
        }
        yield return new ToState { State = typeof(ControlledIdle) };
    }
}