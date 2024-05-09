using UnityEngine;

namespace RingLib.Attacks;

internal class NailSlash : Attack
{
    public int DamageHero;
    public int DamageEnemy;
    private GameObject damageHero;
    private GameObject damageEnemy;
    private PolygonCollider2D originalCollider2D;
    private void Start()
    {
        var template = Preloader.Get("GG_Sly/Battle Scene/Sly Boss/S1");
        damageHero = Instantiate(template, Vector3.zero, Quaternion.identity, transform);
        damageHero.transform.localScale = Vector3.one;
        damageHero.name = "DamageHero";
        Destroy(damageHero.GetComponent<PolygonCollider2D>());
        damageHero.GetComponent<DamageHero>().damageDealt = DamageHero;
        damageHero.SetActive(true);
        originalCollider2D = GetComponent<PolygonCollider2D>();
    }
    private void Update()
    {
        if (damageHero.GetComponent<PolygonCollider2D>() == null)
        {
            var parryableNailSlashCollider = damageHero.AddComponent<PolygonCollider2D>();
            ComponentPatcher<PolygonCollider2D>.Patch(parryableNailSlashCollider, originalCollider2D, ["isTrigger", "points"]);
        }
    }
    public override void SetType(bool hero)
    {
        if (hero)
        {
            damageHero.SetActive(false);
            damageEnemy.SetActive(true);
        }
        else
        {
            damageHero.SetActive(true);
            damageEnemy.SetActive(false);
        }
    }
}
