using RingLib.StateMachine;
using System.Collections.Generic;
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
        IEnumerator<Transition> Slash(string slash)
        {
            var previousVelocityX = StateMachine.Velocity.x;
            StateMachine.InputManager.AttackPressed = false;
            var nextSlash = false;
            void updater(float normalizedTime)
            {
                var currentVelocityX = Mathf.Lerp(previousVelocityX, velocityX, normalizedTime);
                StateMachine.Velocity = new Vector2(currentVelocityX, 0);
                nextSlash |= StateMachine.InputManager.AttackPressed;
            }
            yield return new CoroutineTransition
            {
                Routine = StateMachine.Animator.PlayAnimation(slash, updater)
            };
            if (!nextSlash)
            {
                yield return new ToState { State = typeof(ControlledIdle) };
            }
        }
        foreach (var slash in new string[] { "Slash1", "Slash2", "Slash3" })
        {
            yield return new CoroutineTransition
            {
                Routine = Slash(slash)
            };
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
