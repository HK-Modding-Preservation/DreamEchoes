using GlobalEnums;
using UnityEngine;

namespace DreamEchoesCore.Utils;

public class DDNot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            return;
        }
        RingLib.Log.LogInfo("DDNot", "hit" + other.gameObject.name);
        if (other.gameObject.name == "HeroBox")
        {
            var hero = HeroController.instance;
            if (hero.damageMode == DamageMode.HAZARD_ONLY)
            {
                hero.damageMode = DamageMode.FULL_DAMAGE;
                RingLib.Log.LogInfo("", "don't ddark");
            }
        }
    }
}
