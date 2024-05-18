using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private RandomSelector<string> attackRandomSelector = new([
        new(nameof(Dash), 1, 2),
        new(nameof(Slash), 1, 2),
        new(nameof(Hug), 1, 2),
        new(nameof(Parry), 999, 2)
    ]);

    [State]
    private IEnumerator<Transition> Attack()
    {
        yield return new ToState { State = attackRandomSelector.Get() };
    }
}
