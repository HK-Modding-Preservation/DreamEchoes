using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class TreeStateMachine : StateMachine
{
    public GameObject Attacks;

    [State]
    private IEnumerator<Transition> Begin()
    {
        while (true)
        {
            yield return new NoTransition();
        }
    }

    public TreeStateMachine() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    public void StartAttacks()
    {
        var attackPos = Attacks.transform.position;
        attackPos.x = transform.parent.position.x;
        Attacks.transform.position = attackPos;
        Attacks.SetActive(true);
    }
}
