using UnityEngine;

namespace DreamEchoesCore.Entities.NPCs;

internal class NeedSeer : MonoBehaviour
{
    private void Awake()
    {
        if (!DreamEchoesCore.Instance.SaveSettings.seenSeer)
        {
            gameObject.SetActive(false);
        }
    }
}
