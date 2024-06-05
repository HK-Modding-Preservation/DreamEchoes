using UnityEngine;

namespace DreamEchoesCore.Utils;

public class WeaverCoreDream : MonoBehaviour
{
    private WeaverCore.Components.SpriteFlasher spriteFlasher;
    private void Start()
    {
        spriteFlasher = GetComponentInChildren<WeaverCore.Components.SpriteFlasher>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Dream Attack")
        {
            spriteFlasher.flashDreamImpact();
        }
    }
}
