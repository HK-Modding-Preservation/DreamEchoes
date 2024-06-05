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
        ["WeaverCore", "DreamEchoes", "MoreGodhomeSpaceMod"])
    {
        Instance = this;
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

    public static Sprite LoadTexture(string path)
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
        MemoryStream memoryStream = new((int)stream.Length);
        stream.CopyTo(memoryStream);
        stream.Close();
        var bytes = memoryStream.ToArray();
        memoryStream.Close();
        Texture2D texture2D = new(0, 0);
        texture2D.LoadImage(bytes, true);
        Sprite newSprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
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
        flower1 = LoadTexture("DreamEchoesCore.Resources.YiFlower1.png");
        flower2 = LoadTexture("DreamEchoesCore.Resources.YiFlower2.png");
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
