using UnityEngine;

namespace RingLib.Attacks;

internal class Attack : MonoBehaviour
{
    public virtual void SetType(bool hero)
    {
        Log.LogError(GetType().Name, $"SetType not implemented");
    }
}
