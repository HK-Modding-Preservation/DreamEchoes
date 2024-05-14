using RingLib.StateMachine;
using RingLib.Utils;
using System;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Attack : State<SeerStateMachine>
{
    private RandomSelector<Type> randomSelector = new([
        new(typeof(Dash), 1, 2),
        new(typeof(Slash), 1, 2)
    ]);

    public override Transition Enter()
    {
        return new ToState { State = randomSelector.Get() };
    }
}
