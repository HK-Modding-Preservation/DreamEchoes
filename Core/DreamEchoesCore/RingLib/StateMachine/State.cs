using System.Collections;

namespace DreamEchoesCore.RingLib.StateMachine;

internal class StateBase
{
    public StateMachine StateMachine { get; set; }
    public virtual Type? Enter() { return null; }
    public virtual void Exit(bool interrupted) { }
    public virtual Type? Update() { return null; }
    public void StartCoroutine(IEnumerator routine)
    {
        if (StateMachine == null)
        {
            Log.LogError(GetType().Name, $"StateMachine is null");
            return;
        }
        StateMachine.StartCoroutine(routine);
    }
}
internal class State<TStateMachine> : StateBase where TStateMachine : StateMachine
{
    public new TStateMachine StateMachine
    {
        get => base.StateMachine as TStateMachine;
    }
}
