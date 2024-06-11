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
        IdleCount += 1;
        if (treeStateMachine.status >= 1)
        {
            treeStateMachine.status += 1;
        }
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        animator.PlayAnimation("Idle");
        yield return new WaitFor { Seconds = Config.IdleDuration };

        if (entityHealth.Health <= treeHP)
        {
            treeHP = int.MinValue;
            LockStun();
            yield return new ToState { State = nameof(TreeSummon) };
        }

        if (entityHealth.Health <= shadowHP)
        {
            RingLib.Log.LogInfo("ShadowSummon", "Current health is " + entityHealth.Health + " and shadowHP is " + shadowHP);
            shadowHP = int.MinValue;
            LockStun();
            yield return new ToState { State = nameof(ShadowSummon) };
        }

        yield return new ToState { State = idleRandomSelector.Get() };
    }
}
