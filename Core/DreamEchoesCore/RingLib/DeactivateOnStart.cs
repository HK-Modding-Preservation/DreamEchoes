using UnityEngine;

namespace RingLib;

internal class DeactivateOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
