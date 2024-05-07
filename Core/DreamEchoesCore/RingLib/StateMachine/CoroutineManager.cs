using System.Collections;

namespace DreamEchoesCore.RingLib.StateMachine;

internal class CoroutineManager
{
    private List<Coroutine> activeCoroutines = new();
    public void StartCoroutine(IEnumerator coroutine)
    {
        activeCoroutines.Add(new Coroutine(coroutine));
    }
    public String UpdateCoroutines()
    {
        foreach (var coroutine in activeCoroutines)
        {
            var nextState = coroutine.Update();
            if (nextState != null)
            {
                return nextState;
            }
        }
        return null;
    }
    public void StopCoroutines()
    {
        activeCoroutines.Clear();
    }
}
