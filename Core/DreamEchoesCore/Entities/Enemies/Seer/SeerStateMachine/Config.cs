using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class Config
{
    public float GravityScale = 2;

    public float IdleDuration = 0.25f;

    public float RunVelocityX = 10;
    public float RunDuration = 0.45f;

    public float EvadeJumpRadiusMin = 5;
    public float EvadeJumpRadiusMax = 7.5f;
    public float EvadeJumpVelocityXScale = 2;
    public float EvadeJumpVelocityY = 40;

    public Vector2 DashStartColliderOffset = new(0, -0.7f);
    public Vector2 DashStartColliderSize = new(1.2f, 2.6f);
    public Vector2 DashColliderOffset = Vector2.zero;
    public Vector2 DashColliderSize = new(4, 1.2f);
    public Vector2 DashEndColliderOffset = new(0, -0.7f);
    public Vector2 DashEndColliderSize = new(1.2f, 2.6f);
    public float DashVelocityX = 50;
    public float DashDuration = 0.1f;

    public float SlashVelocityXScale = 2;
    public float ControlledSlashVelocityX = 7.5f;

    public float HugVelocityX = 7.5f;
    public float HugVelocityY = 5;
    public float HugRadiantNailSpeed = 20;

    public int StunThreshold = 3;
    public Vector2 StunColliderOffset = new(0, -0.75f);
    public Vector2 StunColliderSize = new(1.2f, 2.5f);
    public float StunVelocityX = 20;
    public float StunVelocityY = 20;
    public float StunDuration = 2;
}
