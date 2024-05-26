using RingLib.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal partial class SeerStateMachine : EntityStateMachine
{
    [State]
    private IEnumerator<Transition> Defeated()
    {
        // DefeatedStart
        var damageHero = GetComponent<DamageHero>();
        damageHero.damageDealt = 0;
        Reset();
        PlayStunEffect();
        if (!FacingTarget())
        {
            yield return new CoroutineTransition { Routine = Turn() };
        }
        Velocity = Vector2.zero;
        yield return new CoroutineTransition { Routine = animator.PlayAnimation("StunStart") };

        // DefeatedAir
        BoxCollider2D.offset = Config.StunColliderOffset;
        BoxCollider2D.size = Config.StunColliderSize;
        var velocityXMax = Config.StunVelocityX;
        var velocityX = velocityXMax * -Direction();
        var velocityY = Config.StunVelocityY;
        Velocity = new Vector2(velocityX, velocityY);
        animator.PlayAnimation("StunAir");
        yield return new WaitTill { Condition = Landed };

        // DefeatedLand
        Velocity = Vector2.zero;
        animator.PlayAnimation("StunLand");

        // DefeatedEnd
        var bossSceneController = BossSceneController.Instance;
        bossSceneController.EndBossScene();
    }
}
