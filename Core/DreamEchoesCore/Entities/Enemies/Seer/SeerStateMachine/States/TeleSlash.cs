using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private IEnumerator<Transition> TeleSlashSlash()
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
            if (slash.EndsWith("1"))
            {
                speak.PlayOneShot(animator.Slash1Words);
            }
            else if (slash.EndsWith("2"))
            {
                speak.PlayOneShot(animator.Slash2Words);
            }
            else if (slash.EndsWith("3"))
            {
                speak.PlayOneShot(animator.Slash3Words);
            }
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
    private IEnumerator<Transition> TeleSlash()
    {
        var currentY = Position.y;
        var heroX = Target().Position().x;
        var candidateXs = new float[] { heroX - Config.TeleSlashX, heroX + Config.TeleSlashX };
        float candidateX = 0;
        while (true)
        {
            candidateX = candidateXs[Random.Range(0, 2)];
            if (candidateX > minX && candidateX < maxX)
            {
                break;
            }
        }
        var candidateY = currentY + Config.TeleSlashY;

        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("Tele1") };

        Position = new Vector2(candidateX, candidateY);
        Rigidbody2D.gravityScale = 0;
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("Tele2") };


        heroX = Target().Position().x;
        candidateXs = new float[] { heroX - Config.TeleSlashXClose, heroX + Config.TeleSlashXClose };
        candidateX = 0;
        while (true)
        {
            candidateX = candidateXs[Random.Range(0, 2)];
            if (candidateX > minX && candidateX < maxX)
            {
                break;
            }
        }
        candidateY = currentY;
        Rigidbody2D.gravityScale = Config.GravityScale;
        Position = new Vector2(candidateX, candidateY);
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("Tele3") };
        yield return new CoroutineTransition
        {
            Routine = TeleSlashSlash()
        };

        Velocity = Vector2.zero;
        yield return new ToState { State = nameof(Idle) };
    }
}
