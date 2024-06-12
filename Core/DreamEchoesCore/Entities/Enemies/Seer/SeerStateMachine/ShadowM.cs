using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class ShadowM : StateMachine
{
    public GameObject Seer;

    private List<GameObject> shadows = new List<GameObject>();

    [State]
    private IEnumerator<Transition> Begin()
    {
        var seerStateMachine = Seer.GetComponent<SeerStateMachine>();
        while (!seerStateMachine.shadownCanOut)
        {
            yield return new NoTransition();
        }
        for (int i = 0; i < shadows.Count; i++)
        {
            var shadow = shadows[i];
            shadow.SetActive(true);
            var shadowStateMachine = shadow.GetComponent<ShadowStateMachine>();
            shadowStateMachine.flyok = true;
        }
        while (true)
        {
            yield return new NoTransition();
        }
    }

    public ShadowM() : base(
        startState: nameof(Begin),
        globalTransitions: [])
    { }

    protected override void StateMachineStart()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var shadow = transform.GetChild(i).gameObject;
            var shadowStateMachine = shadow.GetComponent<ShadowStateMachine>();
            var currentY = shadow.transform.position.y;
            shadowStateMachine.originaY = currentY;
            shadow.transform.Translate(0, -20, 0);
            shadows.Add(shadow);
        }
    }
}
