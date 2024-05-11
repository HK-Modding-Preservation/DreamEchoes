using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Slash : State<SeerStateMachine>
{
    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }
    private IEnumerator<Transition> Routine()
    {
        if (!StateMachine.FacingTarget())
        {
            StateMachine.Turn();
        }
        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.SlashVelocityX * direction;
        StateMachine.Velocity = Vector2.zero;
        IEnumerable<Transition> Slash(string slash)
        {
            var previousVelocityX = StateMachine.Velocity.x;
            var duration = StateMachine.Animator.PlayAnimation(slash);
            var timer = 0f;
            while (timer < duration)
            {
                var currentVelocityX = Mathf.Lerp(previousVelocityX, velocityX, timer / duration);
                StateMachine.Velocity = new Vector2(currentVelocityX, 0);
                timer += Time.deltaTime;
                yield return new CurrentState();
            }
        }
        foreach (var slash in new string[] { "Slash1", "Slash2", "Slash3" })
        {
            foreach (var transition in Slash(slash))
            {
                yield return transition;
            }
        }
        yield return new ToState { State = typeof(Idle) };
    }
    public override void Exit(bool interrupted)
    {
        if (interrupted)
        {
            StateMachine.ResetAttacks();
        }
    }
}