using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private RandomSelector<string> idleRandomSelector = new([
        new(nameof(Run), 1, 2),
        new(nameof(EvadeJump), 1, 2)
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
        yield return new WaitFor { Seconds = config.IdleDuration };
        yield return new ToState { State = idleRandomSelector.Get() };
    }
}
