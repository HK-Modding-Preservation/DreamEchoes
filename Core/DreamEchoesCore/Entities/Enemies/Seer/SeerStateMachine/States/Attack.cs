using RingLib.StateMachine;
using RingLib.Utils;
using System;
using System.Collections.Generic;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Attack : State<SeerStateMachine>
{
    private RandomSelector<Type> randomSelector = new([
        // new(typeof(Dash), 1, 2),
        // new(typeof(Slash), 1, 2),
        new(typeof(Hug), 1, 9999)
    ]);

    public override IEnumerator<Transition> Routine()
    {
        yield return new ToState { State = randomSelector.Get() };
    }
}
