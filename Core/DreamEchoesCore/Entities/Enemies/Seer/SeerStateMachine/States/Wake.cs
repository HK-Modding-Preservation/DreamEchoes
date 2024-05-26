using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private WeaverMusicCue musicCue;

    [State]
    private IEnumerator<Transition> Wake()
    {
        if (FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        animator.PlayAnimation("WakeIdle");

        float distance()
        {
            return Mathf.Abs(Target().Position().x - Position.x);
        }
        yield return new WaitTill { Condition = () => distance() < Config.WakeDistance };

        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        animator.PlayWakeSound();
        yield return new WaitFor { Seconds = Config.WakeDelay };
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("WakeWake") };

        GameManager.instance.AudioManager.ApplyMusicCue(musicCue, 0, 0, false);
        yield return new ToState { State = nameof(Idle) };
    }
}
