using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    private RandomSelector<string> attackRandomSelector = new([
        new(value: nameof(Dash), weight: 1, maxCount: 2, maxMiss: 5),
        //new(value: nameof(Slash), weight: 1, maxCount: 2, maxMiss: 5),
        //new(value: nameof(Hug), weight: 1, maxCount: 2, maxMiss: 5),
        //new(value: nameof(TeleSlash), weight: 1, maxCount: 2, maxMiss: 5),
        new(value: nameof(Laser), weight: 1, maxCount: 2, maxMiss: 5),
    ]);

    [State]
    private IEnumerator<Transition> Attack()
    {
        yield return new ToState { State = attackRandomSelector.Get() };
    }
}
