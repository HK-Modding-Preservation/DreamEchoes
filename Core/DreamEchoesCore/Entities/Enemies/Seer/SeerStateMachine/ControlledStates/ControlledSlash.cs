using RingLib.StateMachine;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;

internal class ControlledSlash : State<SeerStateMachine>
{
    public override Transition Enter()
    {
        StartCoroutine(Routine());
        return new CurrentState();
    }
    private IEnumerator<Transition> Routine()
    {
        var direction = StateMachine.Direction();
        var velocityX = StateMachine.Config.ControlledSlashVelocityX * direction;
        StateMachine.Velocity = Vector2.zero;
        IEnumerable<Transition> Slash(string slash)
        {
            var previousVelocityX = StateMachine.Velocity.x;
            StateMachine.InputManager.AttackPressed = false;
            var nextSlash = false;
            var duration = StateMachine.Animator.PlayAnimation(slash);
            var timer = 0f;
            while (timer < duration)
            {
                var currentVelocityX = Mathf.Lerp(previousVelocityX, velocityX, timer / duration);
                StateMachine.Velocity = new Vector2(currentVelocityX, 0);
                nextSlash |= StateMachine.InputManager.AttackPressed;
                timer += Time.deltaTime;
                yield return new CurrentState();
            }
            if (!nextSlash)
            {
                yield return new ToState { State = typeof(ControlledIdle) };
            }
        }
        foreach (var slash in new string[] { "Slash1", "Slash2", "Slash3" })
        {
            foreach (var transition in Slash(slash))
            {
                yield return transition;
            }
        }
        StateMachine.InputManager.AttackPressed = false;
        yield return new ToState { State = typeof(ControlledIdle) };
    }
    public override void Exit(bool interrupted)
    {
        if (interrupted)
        {
            StateMachine.Reset();
        }
    }
}
