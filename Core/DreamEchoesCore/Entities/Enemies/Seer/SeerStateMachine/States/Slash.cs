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
        var velocityX = (StateMachine.Target().Position().x - StateMachine.Position.x);
        velocityX *= StateMachine.Config.SlashVelocityXScale;
        var minVelocityX = StateMachine.Config.ControlledSlashVelocityX;
        if (velocityX > -minVelocityX && velocityX < minVelocityX)
        {
            velocityX = Mathf.Sign(velocityX) * StateMachine.Config.ControlledSlashVelocityX;
        }
        StateMachine.Velocity = Vector2.zero;
        IEnumerable<Transition> Slash(string slash)
        {
            if (!StateMachine.FacingTarget())
            {
                velocityX *= -1;
                StateMachine.Velocity *= -1;
                StateMachine.Turn();
            }
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
            StateMachine.Reset();
        }
    }
}
