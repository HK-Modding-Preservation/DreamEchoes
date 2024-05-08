
using DreamEchoesCore.RingLib.StateMachine;
using System.Collections;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.States;

internal class Idle : State<SeerStateMachine>
{
    public override Type? Enter()
    {
        StartCoroutine(Routine());
        return null;
    }
    private IEnumerator Routine()
    {
        yield return StateMachine.Config.IdleDuration;
        yield return typeof(Run);
    }
}