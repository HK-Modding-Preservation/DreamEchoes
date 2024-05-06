namespace DreamEchoes;

public class DreamEchoesCore : Mod
{
    public DreamEchoesCore() : base("DreamEchoesCore")
    {
    }
    public override string GetVersion() => "1.0.0.0";
    public override List<(string, string)> GetPreloadNames()
    {
        return Preloader.GetPreloadNames();
    }
    public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
    {
        Preloader.Initialize(preloadedObjects);
    }
}
