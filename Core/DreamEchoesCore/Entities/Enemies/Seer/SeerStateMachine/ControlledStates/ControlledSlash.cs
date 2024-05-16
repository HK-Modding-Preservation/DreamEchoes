using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> ControlledSlash()
    {
        var direction = Direction();
        var velocityX = config.ControlledSlashVelocityX * direction;
        Velocity = Vector2.zero;
        IEnumerator<Transition> Slash(string slash)
        {
            var previousVelocityX = Velocity.x;
            inputManager.AttackPressed = false;
            var nextSlash = false;
            Transition updater(float normalizedTime)
            {
                var currentVelocityX = Mathf.Lerp(previousVelocityX, velocityX, normalizedTime);
                Velocity = new Vector2(currentVelocityX, 0);
                nextSlash |= inputManager.AttackPressed;
                return new NoTransition();
            }
            yield return new CoroutineTransition
            {
                Routine = animator.PlayAnimation(slash, updater)
            };
            if (!nextSlash)
            {
                yield return new ToState { State = nameof(ControlledIdle) };
            }
        }
        foreach (var slash in new string[] { "Slash1", "Slash2", "Slash3" })
        {
            yield return new CoroutineTransition
            {
                Routine = Slash(slash)
            };
        }
        inputManager.AttackPressed = false;
        yield return new ToState { State = nameof(ControlledIdle) };
    }
}
