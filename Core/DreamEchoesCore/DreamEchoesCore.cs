using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;
using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine.ControlledStates;
using Modding;
using Modding.Utils;
using RingLib;
using RingLib.Attacks;
using UnityEngine;
using WeaverCore.Utilities;

namespace DreamEchoesCore;

public class DreamEchoesCore : Mod
{
    public static DreamEchoesCore Instance { get; private set; }
    public DreamEchoesCore() : base("DreamEchoesCore")
    {
        Instance = this;
#if DEBUG
        RingLib.Log.LoggerInfo = Log;
#endif
        RingLib.Log.LoggerError = LogError;
    }
    public override string GetVersion() => "1.0.0.0";
    public override List<(string, string)> GetPreloadNames()
    {
        Preloader.PreloadNames.Add(("RestingGrounds_07", "Dream Moth"));
        return Preloader.GetPreloadNames();
    }
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        Preloader.Initialize(preloadedObjects);
        On.HeroController.Update += HeroControllerUpdate;
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
        orig(self);
    }
}
