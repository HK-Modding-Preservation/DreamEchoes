using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private IEnumerator<Transition> ParrySwapSlash()
    {
        if (!FacingTarget())
        {
            yield return new CoroutineTransition
            {
                Routine = Turn()
            };
        }
        var velocityX = (Target().Position().x - Position.x);
        velocityX *= Config.SlashVelocityXScale;
        var minVelocityX = Config.ControlledSlashVelocityX;
        if (Mathf.Abs(velocityX) < minVelocityX)
        {
            velocityX = Mathf.Sign(velocityX) * minVelocityX;
        }
        Velocity = Vector2.zero;

        IEnumerator<Transition> Slash(string slash)
        {
            if (!FacingTarget())
            {
                velocityX *= -1;
                Velocity *= -1;
                yield return new CoroutineTransition
                {
                    Routine = Turn()
                };
            }
            var previousVelocityX = Velocity.x;
            Transition updater(float normalizedTime)
            {
                var currentVelocityX = Mathf.Lerp(previousVelocityX, velocityX, normalizedTime);
                Velocity = new Vector2(currentVelocityX, 0);
                return new NoTransition();
            }
            yield return new CoroutineTransition
            {
                Routine = animator.PlayAnimation(slash, updater)
            };
        }
        foreach (var slash in new string[] { "Slash1", "Slash2" })
        {
            yield return new CoroutineTransition
            {
                Routine = Slash(slash)
            };
        }

        Velocity = Vector2.zero;
    }

    [State]
    private IEnumerator<Transition> Parry()
    {
        // ParryStart
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("ParryStart") };

        // ParryHold
        animator.PlayAnimation("ParryHold");
        var parryed = false;
        yield return new CoroutineTransition
        {
            Routines = [
                new WaitTill
                {
                    Condition = () =>
                    {
                        if (CheckInStateEvent(RingLib.Attacks.NailSlash.OnParryEvent))
                        {
                            parryed = true;
                            return true;
                        }
                        return false;
                    }
                },
                new WaitFor { Seconds = Config.ParryDuration },
            ]
        };

        // ParryCounter
        if (parryed)
        {
            if (!FacingTarget())
            {
                yield return new CoroutineTransition { Routine = Turn() };
            }
            var swapped = false;
            var velocityX = Config.ParryVelocityX * Direction();
            Transition updater(float normalizedTime)
            {
                if (!FacingTarget())
                {
                    swapped = true;
                    return null;
                }
                var currentVelocityX = Mathf.Lerp(0, velocityX, normalizedTime);
                Velocity = new Vector2(currentVelocityX, 0);
                return new NoTransition();
            }
            yield return new CoroutineTransition
            {
                Routine = animator.PlayAnimation("ParryCounter", updater)
            };
            if (swapped)
            {
                Reset();
                yield return new CoroutineTransition
                {
                    Routine = ParrySwapSlash()
                };
            }
            else
            {
                Velocity = Vector2.zero;
                yield return new CoroutineTransition
                {
                    Routine = animator.PlayAnimation("ParryEnd")
                };
            }
        }
        yield return new ToState { State = nameof(Idle) };
    }
}
