using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;
using Modding.Utils;
using RingLib;
using RingLib.Attacks;
using RingLib.Components;
using RingLib.Utils;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore.Utilities;

namespace DreamEchoesCore;

internal class DreamEchoesCore : Mod
{
    private bool renederColliders = false;

    public DreamEchoesCore() : base(
        "DreamEchoesCore", "1.0.0.0",
        Utils.Preload.Names,
        new Dictionary<string, string>
        {
            {"SEER_NAME", "先知" },
            {"SEER_DESC", "温良的守护神" }
        },
        ["WeaverCore", "DreamEchoes", "MoreGodhomeSpaceMod"])
    { }

    public override void ModStart()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
        On.GrassCut.ShouldCut += GrassCutShouldCut;
#if DEBUG
        On.HeroController.Update += HeroControllerUpdate;
#endif
    }

    private void ActiveSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        Scenes.DreamEchoesSeer.Initialize(to.name);
    }

    private bool GrassCutShouldCut(On.GrassCut.orig_ShouldCut orig, Collider2D collision)
    {
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (currentScene.name == "DreamEchoesSeer")
        {
            return false;
        }
        return orig(collision);
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
                    seer.GetComponent<SeerStateMachine>().StartState = nameof(SeerStateMachine.ControlledIdle);
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
        if (Input.GetKeyDown(KeyCode.F11))
        {
            GameManager.instance.BeginSceneTransition(new GameManager.SceneLoadInfo
            {
                SceneName = "DreamEchoesSeer",
                EntryGateName = "door_dreamEnter",
                EntryDelay = 0,
                PreventCameraFadeOut = true,
                Visualization = GameManager.SceneLoadVisualizations.GodsAndGlory,
            });
        }
        orig(self);
    }
}
