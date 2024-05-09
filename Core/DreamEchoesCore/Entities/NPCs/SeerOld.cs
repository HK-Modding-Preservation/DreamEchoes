using RingLib;
using UnityEngine;

namespace DreamEchoesCore.Entities.NPCs;

internal class SeerOld : MonoBehaviour
{
    private void Start()
    {
        var template = Preloader.Get("RestingGrounds_07/Dream Moth");
        var instance = Instantiate(template, transform.position, transform.rotation, transform);
        instance.SetActive(true);
    }
}
