using UnityEngine;

namespace RingLib.EntityManagement;

internal class DeactivateAfter : MonoBehaviour
{
    public float Seconds;
    private void Update()
    {
        Seconds -= Time.deltaTime;
        if (Seconds <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
