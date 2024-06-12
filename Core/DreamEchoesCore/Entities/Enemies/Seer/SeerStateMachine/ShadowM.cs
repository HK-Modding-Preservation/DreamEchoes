using RingLib.StateMachine;
using RingLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class ShadowM : StateMachine
{
    public GameObject Seer;

    private List<GameObject> shadows = new List<GameObject>();
    private List<ShadowStateMachine> fsms = new List<ShadowStateMachine>();
    private List<Vector3> points = new List<Vector3>();

    private IEnumerable<Transition> WaitForReady()
    {
        while (true)
        {
            var allReady = true;
            for (int i = 0; i < shadows.Count; i++)
            {
                var shadow = fsms[i];
                if (!shadow.ready)
                {
                    allReady = false;
                    break;
                }
            }
            if (allReady)
            {
                break;
            }
            yield return new NoTransition();
        }
    }

    private RandomSelector<int> attacksel = new([
       new(value: 0, weight: 1, maxCount: 1, maxMiss: 6),
        new(value: 1, weight: 1, maxCount: 1, maxMiss: 6),
        new(value: 2, weight: 1, maxCount: 1, maxMiss: 6),
        new(value: 3, weight: 1, maxCount: 1, maxMiss: 6)
   ]);

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
            var shadowStateMachine = fsms[i];
            shadowStateMachine.flyok = true;
        }
        while (true)
        {
            /*
            yield return new CoroutineTransition
            {
                Routine = WaitForReady()
            };
            */
            var currentIdleCount = seerStateMachine.IdleCount;
            while (true)
            {
                yield return new NoTransition();
                var allReady = true;
                for (int i = 0; i < shadows.Count; i++)
                {
                    var shadow = fsms[i];
                    if (!shadow.ready)
                    {
                        allReady = false;
                        break;
                    }
                }
                if (seerStateMachine.IdleCount == currentIdleCount)
                {
                    allReady = false;
                }
                if (allReady)
                {
                    break;
                }
            }
            int trycount = 0;
            List<int> indices = [0, 1, 2, 3];
            while (true)
            {
                ++trycount;
                indices = indices.Shuffle().ToList();
                var success = true;
                for (int i = 0; i < indices.Count; i++)
                {
                    if (indices[i] == i)
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    break;
                }
            }
            RingLib.Log.LogInfo("", "tried " + trycount);
            var thistime = attacksel.Get();
            var thistime2 = attacksel.Get();
            for (int i = 0; i < shadows.Count; i++)
            {
                if (i != thistime && i != thistime2)
                {
                    continue;
                }
                var shadow = fsms[i];
                shadow.ready = false;
                shadow.plsattack = true;
                shadow.target = points[indices[i]];
                shadow.target = HeroController.instance.gameObject.transform.position;
                shadow.target.y = 24;
            }
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
            shadowStateMachine.manager = this;
            var currentY = shadow.transform.position.y;
            shadowStateMachine.originaY = currentY;
            shadow.transform.Translate(0, -20, 0);
            shadows.Add(shadow);
            fsms.Add(shadowStateMachine);
            points.Add(new Vector3(shadow.transform.position.x, 24, 0));
        }
    }
}
