using UnityEngine;

namespace DreamEchoesCore.RingLib;

internal class ParryableNailSlash : MonoBehaviour
{
    private GameObject parryableNailSlash;
    private PolygonCollider2D originalCollider2D;
    private void Start()
    {
        var template = Preloader.Get("GG_Sly/Battle Scene/Sly Boss/S1");
        parryableNailSlash = Instantiate(template, Vector3.zero, Quaternion.identity, transform);
        parryableNailSlash.transform.localScale = Vector3.one;
        parryableNailSlash.name = "ParryableNailSlash";
        Destroy(parryableNailSlash.GetComponent<PolygonCollider2D>());
        Destroy(parryableNailSlash.GetComponent<DamageHero>());
        originalCollider2D = GetComponent<PolygonCollider2D>();
        parryableNailSlash.SetActive(true);
    }
    private void Update()
    {
        if (parryableNailSlash.GetComponent<PolygonCollider2D>() == null)
        {
            var parryableNailSlashCollider = parryableNailSlash.AddComponent<PolygonCollider2D>();
            ComponentPatcher<PolygonCollider2D>.Patch(parryableNailSlashCollider, originalCollider2D, ["isTrigger", "points"]);
        }
    }
}
