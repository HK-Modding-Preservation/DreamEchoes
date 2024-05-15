using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;
using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;
using Modding.Utils;
using RingLib;
using RingLib.Attacks;
using RingLib.Components;
using RingLib.Utils;
using UnityEngine;
using WeaverCore.Utilities;

namespace DreamEchoesCore;

internal class DreamEchoesCore : Mod
{
    private bool renederColliders = false;

    public DreamEchoesCore() : base(
        "DreamEchoesCore", "1.0.0.0", [("RestingGrounds_07", "Dream Moth")])
    { }

    public override void ModStart()
    {
#if DEBUG
        On.HeroController.Update += HeroControllerUpdate;
#endif
    }

    private void HeroControllerUpdate(On.HeroController.orig_Update orig, HeroController self)
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            var control = self.gameObject.GetOrAddComponent<Control>();
            if (!control.HasControlled)
            {
                if (!self.controlReqlinquished)
                {
                    var seerPrefab = WeaverAssets.LoadAssetFromBundle<GameObject, DreamEchoes.DreamEchoes>("Seer");
                    var seer = GameObject.Instantiate(seerPrefab, self.transform.position, Quaternion.identity);
                    seer.GetComponent<SeerStateMachine>().StartState = typeof(ControlledIdle);
                    GameObject.Destroy(seer.GetComponent<WeaverCore.Components.PlayerDamager>());
                    foreach (var attack in seer.GetComponentsInChildren<Attack>(true))
                    {
                        attack.Hero = true;
                    }
                    control.InstallControlled(seer);
                }
                else
                {
                    RingLib.Log.LogInfo(GetType().Name, "Cannot install controlled while control is relinquished");
                }
            }
            else
            {
                control.UninstallControlled();
            }
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            renederColliders = !renederColliders;
            foreach (var stateMachine in RingLib.StateMachine.StateMachine.GetInstances())
            {
                stateMachine.gameObject.GetOrAddComponent<ColliderRenderer>().Enabled = renederColliders;
            }
        }
        orig(self);
    }
}
