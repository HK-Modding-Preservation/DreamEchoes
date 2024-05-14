using RingLib.StateMachine;
using System.Collections.Generic;
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
            yield return new CoroutineTransition
            {
                Routine = StateMachine.Turn()
            };
        }

        var velocityX = (StateMachine.Target().Position().x - StateMachine.Position.x);
        velocityX *= StateMachine.Config.SlashVelocityXScale;
        var minVelocityX = StateMachine.Config.ControlledSlashVelocityX;
        if (velocityX > -minVelocityX && velocityX < minVelocityX)
        {
            velocityX = Mathf.Sign(velocityX) * minVelocityX;
        }
        StateMachine.Velocity = Vector2.zero;

        IEnumerator<Transition> Slash(string slash)
        {
            if (!StateMachine.FacingTarget())
            {
                velocityX *= -1;
                StateMachine.Velocity *= -1;
                yield return new CoroutineTransition
                {
                    Routine = StateMachine.Turn()
                };
            }
            var previousVelocityX = StateMachine.Velocity.x;
            void updater(float normalizedTime)
            {
                var currentVelocityX = Mathf.Lerp(previousVelocityX, velocityX, normalizedTime);
                StateMachine.Velocity = new Vector2(currentVelocityX, 0);
            }
            yield return new CoroutineTransition
            {
                Routine = StateMachine.Animator.PlayAnimation(slash, updater)
            };
        }
        foreach (var slash in new string[] { "Slash1", "Slash2", "Slash3" })
        {
            yield return new CoroutineTransition
            {
                Routine = Slash(slash)
            };
        }

        yield return new ToState { State = typeof(Idle) };
    }
}
