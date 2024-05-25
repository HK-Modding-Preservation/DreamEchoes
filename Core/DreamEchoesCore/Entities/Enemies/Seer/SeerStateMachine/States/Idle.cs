using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private RandomSelector<string> idleRandomSelector = new([
        new(value: nameof(Run), weight: 2, maxCount: 2, maxMiss: 5),
        new(value: nameof(EvadeJump), weight: 2, maxCount: 2, maxMiss: 5),
        new(value: nameof(Parry), weight: 1, maxCount: 1, maxMiss: 5)
    ]);

    [State]
    private IEnumerator<Transition> Idle()
    {
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        animator.PlayAnimation("Idle");
        yield return new WaitFor { Seconds = Config.IdleDuration };
        yield return new ToState { State = idleRandomSelector.Get() };
    }
}
