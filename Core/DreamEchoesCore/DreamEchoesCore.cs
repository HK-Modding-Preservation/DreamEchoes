using Modding;
using Modding.Utils;
using RingLib.Utils;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace DreamEchoesCore;

internal class SaveSettings
{
    public bool seenSeer = false;
    public bool sentYiFlower = false;
    public bool yiflw = false;
    public bool yiflw2 = false;
    public bool yileft = false;
}

internal partial class DreamEchoesCore : RingLib.Mod, ILocalSettings<SaveSettings>
{
    public new static DreamEchoesCore Instance;

    public SaveSettings SaveSettings { get; private set; }

    public bool DisableWallJump = false;

    private bool renderColliders = false;

    public Sprite flower1;
    public Sprite flower2;

    public DreamEchoesCore() : base(
        "DreamEchoesCore", "1.0.0.0",
        Utils.Preload.Names,
        ["WeaverCore", "SFCore", "MoreGodhomeSpaceMod", "DreamEchoes"])
    {
        Instance = this;

        flower1 = LoadFlower("DreamEchoesCore.Resources.YiFlower1.png");
        flower2 = LoadFlower("DreamEchoesCore.Resources.YiFlower2.png");

        SFCore.ItemHelper.AddNormalItem(flower1, "yiflw", YI_FLOWER_1_NAME, YI_FLOWER_1_DESC);
        SFCore.ItemHelper.AddNormalItem(flower2, "yiflw2", YI_FLOWER_2_NAME, YI_FLOWER_2_DESC);
    }

    public void OnLoadLocal(SaveSettings s)
    {
        RingLib.Log.LogInfo(GetType().Name, "Loading local settings");
        RingLib.Log.LogInfo(GetType().Name, $"Seen Seer: {s.seenSeer}");
        SaveSettings = s;
    }

    public SaveSettings OnSaveLocal()
    {
        RingLib.Log.LogInfo(GetType().Name, "Saving local settings");
        RingLib.Log.LogInfo(GetType().Name, $"Seen Seer: {SaveSettings.seenSeer}");
        return SaveSettings;
    }

    public static Sprite LoadFlower(string path)
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
        MemoryStream memoryStream = new((int)stream.Length);
        stream.CopyTo(memoryStream);
        stream.Close();
        var bytes = memoryStream.ToArray();
        memoryStream.Close();
        Texture2D texture2D = new(0, 0);
        texture2D.LoadImage(bytes, true);
        float originalPixelsPerUnit = texture2D.width;  // Assuming square textures for simplicity
        float scaledPixelsPerUnit = originalPixelsPerUnit / 1.4f;
        Sprite newSprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), scaledPixelsPerUnit);
        return newSprite;
    }

    public override void ModStart()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
        On.GrassCut.ShouldCut += GrassCutShouldCut;
        On.HeroController.CanWallJump += HeroControllerCanWallJump;
        On.HeroController.CanWallSlide += HeroControllerCanWallSlide;
#if DEBUG
        On.HeroController.Update += HeroControllerUpdate;
#endif
        ModHooks.GetPlayerBoolHook += ModHooks_GetPlayerBoolHook;
    }

    private bool ModHooks_GetPlayerBoolHook(string name, bool orig)
    {
        if (name == "yiflw")
        {
            return SaveSettings.yiflw;
        }
        if (name == "yiflw2")
        {
            return SaveSettings.yiflw2;
        }
        return orig;
    }

    private void ActiveSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
    {
        Scenes.DreamEchoesSeer.Initialize(to.name);
        Scenes.RestingGrounds_07.Initialize(to.name);
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

    private bool HeroControllerCanWallJump(On.HeroController.orig_CanWallJump orig, HeroController self)
    {
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (currentScene.name == "DreamEchoesSeer" && DisableWallJump)
        {
            return false;
        }
        return orig(self);
    }

    private bool HeroControllerCanWallSlide(On.HeroController.orig_CanWallSlide orig, HeroController self)
    {
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (currentScene.name == "DreamEchoesSeer" && DisableWallJump)
        {
            return false;
        }
        return orig(self);
    }

    private void HeroControllerUpdate(On.HeroController.orig_Update orig, HeroController self)
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            renderColliders = !renderColliders;
            foreach (var stateMachine in RingLib.StateMachine.StateMachine.GetInstances())
            {
                stateMachine.gameObject.GetOrAddComponent<ColliderRenderer>().Enabled = renderColliders;
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
