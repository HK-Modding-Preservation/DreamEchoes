using RingLib;

namespace DreamEchoesCore;

internal partial class DreamEchoesCore : Mod
{
    public const string RING_NAME = "RING_NAME";
    public const string RING_WORDS = "RING_WORDS";
    public const string YILE_NAME = "YILE_NAME";
    public const string YILE_WORDS = "YILE_WORDS";

    public const string SEER_NAME = "SEER_NAME";
    public const string SEER_DESC = "SEER_DESC";

    public override string Translate(string key)
    {
        var currentLanguage = Language.Language.CurrentLanguage();
        if (currentLanguage == Language.LanguageCode.ZH)
        {
            switch (key)
            {
                case RING_NAME:
                    return "近环";
                case RING_WORDS:
                    return "旅行者你好呀！是你在我的梦中，还是我在你的梦中？无论如何，我们都在很遥远的地方哦！";
                case YILE_NAME:
                    return "伊乐";
                case YILE_WORDS:
                    return "加入毛茸茸蛾子教吧！（可爱颜文字.txt）";
                case SEER_NAME:
                    return "先知";
                case SEER_DESC:
                    return "温良的守护神";
                default:
                    return null;
            }
        }
        else
        {
            switch (key)
            {
                case SEER_NAME:
                    return "Seer";
                case SEER_DESC:
                    return "Warm God of Protection";
                case RING_NAME:
                    return "NearRing";
                case RING_WORDS:
                    return "Greetings, traveler. Are you in my dream, or am I in yours? Either way, we're both far from home!";
                case YILE_NAME:
                    return "Yile";
                case YILE_WORDS:
                    return "Join the Fuzzy Moth Cult! (cute emoticon.txt)";
                default:
                    return null;
            }
        }
    }
}
