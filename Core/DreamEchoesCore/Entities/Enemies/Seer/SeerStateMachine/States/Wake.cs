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
        // WakeLook
        if (FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        animator.PlayAnimation("WakeLook");
        float distance()
        {
            var myPosition = Position;
            var targetPosition = Target().Position();
            var distance = Vector2.Distance(myPosition, targetPosition);
            return distance;
        }
        yield return new WaitTill { Condition = () => distance() < Config.WakeDistance };

        // WakeIdle
        animator.Roof.SetActive(true);
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("WakeIdle") };

        // WakeWake
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("WakeWake") };
        GameManager.instance.AudioManager.ApplyMusicCue(musicCue, 0, 0, false);
        yield return new ToState { State = nameof(Idle) };
    }
}
