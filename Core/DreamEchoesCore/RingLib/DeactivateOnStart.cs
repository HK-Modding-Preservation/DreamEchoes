using UnityEngine;

namespace DreamEchoesCore.RingLib;

internal class DeactivateOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
