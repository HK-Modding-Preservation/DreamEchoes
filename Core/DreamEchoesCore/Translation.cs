using RingLib;

namespace DreamEchoesCore;

internal partial class DreamEchoesCore : Mod
{
    public const string RING_NAME = "RING_NAME";
    public const string RING_WORDS = "RING_WORDS";
    public const string YILE_NAME = "YILE_NAME";
    public const string YILE_WORDS = "YILE_WORDS";

    public const string YI_NAME = "YI_NAME";
    public const string YI_WORDS_1 = "YI_WORDS_1";
    public const string YI_WORDS_2 = "YI_WORDS_2";
    public const string YI_WORDS_3 = "YI_WORDS_3";
    public const string YI_WORDS_4 = "YI_WORDS_4";
    public const string YI_WORDS_5 = "YI_WORDS_5";
    public const string YI_WORDS_6 = "YI_WORDS_6";
    public const string YI_WORDS_7 = "YI_WORDS_7";

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
                case YI_NAME:
                    return "伊";
                case YI_WORDS_1:
                    return "又见面啦，小伙伴！你也是被先知捡回来的吗？";
                case YI_WORDS_2:
                    return "在圣巢之冠听到的微弱声音使我有点入了迷，全然没注意到有只会投射水晶的虫顺着阴暗狭窄的隧道也到达了顶端……它发射的那些水晶尖刺把我击落到了这里";
                case YI_WORDS_3:
                    return "……很悠扬的声音，又是如此细小，必须全神贯注才能听清……";
                case YI_WORDS_4:
                    return "我可能需要在这里修整一下，先知给了我一些文献让我不至于特别无聊，想听听看吗？我刚看完一些。";
                case YI_WORDS_5:
                    return "……低语之根是生根的梦境，它们充满精华，分散在圣巢的各处……";
                case YI_WORDS_6:
                    return "……使用梦之钉劈砍未长成的低语之根可以打断他们的生长，它们就像杂草一样需要定期清理……";
                case YI_WORDS_7:
                    return "……大多数低语之根会慢慢消散，但当低语之根汇聚的梦境拥有了历史底蕴，它们则会得以保存，梦钉的劈砍会使得他们更为强壮……";
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
                case RING_NAME:
                    return "NearRing";
                case RING_WORDS:
                    return "Greetings, traveler. Are you in my dream, or am I in yours? Anyway, we're both far from home!";
                case YILE_NAME:
                    return "Yile";
                case YILE_WORDS:
                    return "Join the Fuzzy Moth Cult! (cute emoticon.txt)";
                case YI_NAME:
                    return "Yi";
                case YI_WORDS_1:
                    return "We meet again, fellow traveller! Were you also picked up by the Seer?";
                case YI_WORDS_2:
                    return "The faint chimes I heard at Hallownest's Crown mesmerized me, so I failed to notice a creature casting crystals passing through the dark, narrow tunnels to the summit... Its crystal spikes knocked me down here.";
                case YI_WORDS_3:
                    return "...Such a melodious chime, yet so faint, one must focus completely to hear it...";
                case YI_WORDS_4:
                    return "I might need to rest here a while; The lady gave me some literature so I wouldn't be utterly bored. Care to listen? I've just finished some.";
                case YI_WORDS_5:
                    return "...The Whispering Roots are roots of dreams, brimming with essence, spread throughout Hallownest...";
                case YI_WORDS_6:
                    return "...Using the Dream Nail to cut through the immature roots can halt their growth; they're like weeds that need regular clearing...";
                case YI_WORDS_7:
                    return "...Most roots will slowly fade, but when the dreams gathered by the roots have historical substance, they are preserved, and the cuts from a Dream Nail make them thrive...";
                case SEER_NAME:
                    return "Seer";
                case SEER_DESC:
                    return "Warm God of Protection";
                default:
                    return null;
            }
        }
    }
}
