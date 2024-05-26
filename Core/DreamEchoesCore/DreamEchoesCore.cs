using Modding.Utils;
using RingLib;
using RingLib.Utils;
using UnityEngine;

namespace DreamEchoesCore;

internal partial class DreamEchoesCore : Mod
{
    private bool renederColliders = false;

    public DreamEchoesCore() : base(
        "DreamEchoesCore", "1.0.0.0",
        Utils.Preload.Names,
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
