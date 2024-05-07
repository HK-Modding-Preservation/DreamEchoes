using DreamEchoes.RingLib;
using Modding;
using UnityEngine;

namespace DreamEchoes;

public class DreamEchoesCore : Mod
{
    public static DreamEchoesCore Instance { get; private set; }
    public DreamEchoesCore() : base("DreamEchoesCore")
    {
        Instance = this;
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
    }
}
