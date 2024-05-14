using RingLib.StateMachine;
using RingLib.Utils;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Run : State<SeerStateMachine>
{
    private RandomSelector<Type> randomSelector = new([
        new(typeof(Dash), 1, 2),
        new(typeof(Slash), 1, 2)
    ]);

    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }

    private IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            yield return new CoroutineTransition { Routine = StateMachine.Turn() };
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
            yield return new CurrentState();
        }
        StateMachine.Velocity = new Vector2(velocityX, 0);
        StateMachine.Animator.PlayAnimation("Run");
        yield return new WaitFor { Seconds = StateMachine.Config.RunDuration };
        var endDuration = StateMachine.Animator.PlayAnimation("RunEnd");
        timer = 0f;
        while (timer < endDuration)
        {
            var currentVelocityX = Mathf.Lerp(velocityX, 0, timer / endDuration);
            StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            timer += Time.deltaTime;
            yield return new CurrentState();
        }
        yield return new ToState { State = typeof(Attack) };
    }
}
