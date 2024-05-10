using UnityEngine;

namespace RingLib.Attacks;

internal class NailSlash : Attack
{
    public int DamageHero;
    public int DamageEnemy;
    private GameObject damageHero;
    private GameObject damageEnemy;
    private GameObject damageEnemyTinker;
    private PolygonCollider2D originalCollider2D;

    private void Start()
    {
        var damageHeroPrefab = Preloader.Get("GG_Sly/Battle Scene/Sly Boss/S1");
        damageHero = Instantiate(damageHeroPrefab, Vector3.zero, Quaternion.identity, transform);
        damageHero.transform.localScale = Vector3.one;
        damageHero.name = "DamageHero";
        Destroy(damageHero.GetComponent<PolygonCollider2D>());
        damageHero.GetComponent<DamageHero>().damageDealt = DamageHero;
        damageHero.SetActive(true);

        damageEnemy = new GameObject("DamageEnemy");
        damageEnemy.layer = LayerMask.NameToLayer("Attack");
        damageEnemy.transform.parent = transform;
        damageEnemy.transform.localPosition = Vector3.zero;
        damageEnemy.transform.localScale = Vector3.one;
        var damageEnemies = damageEnemy.AddComponent<DamageEnemies>();
        var damageEnemiesSlash = HeroController.instance.transform.Find("Attacks").Find("Slash").gameObject.LocateMyFSM("damages_enemy");
        damageEnemies.attackType = (AttackTypes)damageEnemiesSlash.FsmVariables.GetFsmInt("attackType").Value;
        damageEnemies.circleDirection = damageEnemiesSlash.FsmVariables.GetFsmBool("circleDirection").Value;
        damageEnemies.damageDealt = DamageEnemy;
        damageEnemies.direction = damageEnemiesSlash.FsmVariables.GetFsmFloat("direction").Value;
        damageEnemies.ignoreInvuln = damageEnemiesSlash.FsmVariables.GetFsmBool("Ignore Invuln").Value;
        damageEnemies.magnitudeMult = damageEnemiesSlash.FsmVariables.GetFsmFloat("magnitudeMult").Value;
        damageEnemies.moveDirection = damageEnemiesSlash.FsmVariables.GetFsmBool("moveDirection").Value;
        damageEnemies.specialType = (SpecialTypes)damageEnemiesSlash.FsmVariables.GetFsmInt("Special Type").Value;
        damageEnemy.SetActive(false);

        damageEnemyTinker = new GameObject("DamageEnemyTinker");
        damageEnemyTinker.layer = LayerMask.NameToLayer("Tinker");
        damageEnemyTinker.transform.parent = transform;
        damageEnemyTinker.transform.localPosition = Vector3.zero;
        damageEnemyTinker.transform.localScale = Vector3.one;
        damageEnemyTinker.SetActive(false);

        originalCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (damageHero.GetComponent<PolygonCollider2D>() == null)
        {
            var collider2D = damageHero.AddComponent<PolygonCollider2D>();
            ComponentPatcher<PolygonCollider2D>.Patch(collider2D, originalCollider2D, ["isTrigger", "points"]);
        }
        if (damageEnemy.GetComponent<PolygonCollider2D>() == null)
        {
            var collider2D = damageEnemy.AddComponent<PolygonCollider2D>();
            ComponentPatcher<PolygonCollider2D>.Patch(collider2D, originalCollider2D, ["isTrigger", "points"]);
        }
        if (damageEnemyTinker.GetComponent<PolygonCollider2D>() == null)
        {
            var collider2D = damageEnemyTinker.AddComponent<PolygonCollider2D>();
            ComponentPatcher<PolygonCollider2D>.Patch(collider2D, originalCollider2D, ["isTrigger", "points"]);
        }
        if (Hero)
        {
            damageHero.SetActive(false);
            damageEnemy.SetActive(true);
            damageEnemyTinker.SetActive(true);
        }
        else
        {
            damageHero.SetActive(true);
            damageEnemy.SetActive(false);
            damageEnemyTinker.SetActive(false);
        }
    }
}
