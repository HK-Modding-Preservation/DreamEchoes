using System.Collections;
using UnityEngine;

namespace DreamEchoesCore.RingLib.StateMachine;

internal class Coroutine
{
    private IEnumerator enumerator;
    private float time;
    public Coroutine(IEnumerator enumerator)
    {
        this.enumerator = enumerator;
        time = 0;
    }
    public string Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (time > 0)
            {
                return null;
            }
        }
        if (enumerator.MoveNext())
        {
            object current = enumerator.Current;
            if (current is Type nextState)
            {
                return nextState.Name;
            }
            if (current is float waitForSecondsFloat)
            {
                time = waitForSecondsFloat;
                return null;
            }
            if (current is int waitForSecondsInt)
            {
                time = waitForSecondsInt;
                return null;
            }
            Log.LogError(GetType().Name, $"Invalid yield return {current}");
        }
        return null;
    }
}
