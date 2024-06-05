using RingLib;

namespace DreamEchoesCore;

internal partial class DreamEchoesCore : Mod
{
    public const string RING_NAME = "RING_NAME";
    public const string RING_WORDS = "RING_WORDS";
    public const string YILE_NAME = "YILE_NAME";
    public const string YILE_WORDS = "YILE_WORDS";
    public const string BXN_NAME = "BXN_NAME";
    public const string BXN_WORDS = "BXN_WORDS";

    public const string YI_NAME = "YI_NAME";
    public const string YI_WORDS_1 = "YI_WORDS_1";
    public const string YI_WORDS_2 = "YI_WORDS_2";
    public const string YI_WORDS_3 = "YI_WORDS_3";
    public const string YI_WORDS_4 = "YI_WORDS_4";
    public const string YI_WORDS_5 = "YI_WORDS_5";
    public const string YI_WORDS_6 = "YI_WORDS_6";
    public const string YI_WORDS_7 = "YI_WORDS_7";

    public const string YI_GFWORDS_1 = "YI_GFWORDS_1";
    public const string YI_GFWORDS_2 = "YI_GFWORDS_2";
    public const string YI_GFWORDS_3 = "YI_GFWORDS_3";

    public const string SEER_NAME = "SEER_NAME";
    public const string SEER_DESC = "SEER_DESC";

    public const string SEERDREAM_1 = "SEERDREAM_1";
    public const string SEERDREAM_2 = "SEERDREAM_2";
    public const string SEERDREAM_3 = "SEERDREAM_3";

    public const string YI_FLOWER_1_NAME = "YI_FLOWER_1_NAME";
    public const string YI_FLOWER_1_DESC = "YI_FLOWER_1_DESC";
    public const string YI_FLOWER_2_NAME = "YI_FLOWER_2_NAME";
    public const string YI_FLOWER_2_DESC = "YI_FLOWER_2_DESC";

    public const string YI_SEND_FLOWER1 = "YI_SEND_FLOWER1";
    public const string YI_SEND_FLOWER2 = "YI_SEND_FLOWER2";
    public const string YI_SEND_FLOWER3 = "YI_SEND_FLOWER3";
    public const string YI_SEND_FLOWER4 = "YI_SEND_FLOWER4";
    public const string YI_SEND_FLOWER5 = "YI_SEND_FLOWER5";

    public override string Translate(string key, string sheetTitle)
    {
        RingLib.Log.LogInfo("", "Translating " + key + " from " + sheetTitle);

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
                case BXN_NAME:
                    return "北小鸟";
                case BXN_WORDS:
                    return "吾虽年迈，但骨钉未老，若有挑战，必挥舞骨钉，破除迷雾，帝王之翼伴我随行。";
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
                case SEERDREAM_1:
                    return "……永远铭记……";
                case SEERDREAM_2:
                    return "……曾经温和的光……现在如此灼热……";
                case SEERDREAM_3:
                    return "……向光而生……与光同尘……";
                case YI_GFWORDS_1:
                    return "好熟悉的花香……好娇弱的花……你是从哪里找到的……？";
                case YI_GFWORDS_2:
                    return "--送出礼物？--";
                case YI_GFWORDS_3:
                    return "还是第一次有虫给我送花……谢谢你的好意！这朵花很美，但是它太脆弱了，轻微的颠簸和碰撞都会使它凋落……这样独一无二的珍品应当送给更值得的虫，不是么？";
                case YI_FLOWER_1_NAME:
                    return "无名野花";
                case YI_FLOWER_1_DESC:
                    return "伊给小骑士的礼物，有种让虫安心的淡淡幽香\r\n\r\n这朵花很坚强，不会因为持有者受伤而损毁";
                case YI_FLOWER_2_NAME:
                    return "凋零的花";
                case YI_FLOWER_2_DESC:
                    return "伊给小骑士的礼物，因为某种原因凋谢了\r\n\r\n虽说花已凋落，但是余下的部分又组成了另一种形态的花";
                case YI_SEND_FLOWER1:
                    return "先知奶奶的小屋真让人安心，但我也该启程了，那钟声实在是让我好奇……我这也有一朵花是给你的……";
                case YI_SEND_FLOWER2:
                    return "--接受礼物？--";
                case YI_SEND_FLOWER3:
                    return "这朵花送给你，它会在你历尽黑暗时尽它最大的努力保护你！";
                case YI_SEND_FLOWER4:
                    return "啊……好吧，如果你不喜欢它的话……";
                case YI_SEND_FLOWER5:
                    return "再见啦，探险家，说不准我们之后还能在其他地方相逢呢（笑";
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
                case BXN_NAME:
                    return "BeiXiaoNiao";
                case BXN_WORDS:
                    return "Though time has left its mark on me, my nail remains sharp. Should challenges arise, I will wield my nail to sever difficulties, with Monarch Wings accompanying me.";
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
                case SEERDREAM_1:
                    return "...Never forget...";
                case SEERDREAM_2:
                    return "...Once a gentle light... now so scorching...";
                case SEERDREAM_3:
                    return "...Born towards the light... united with the light...";
                case YI_GFWORDS_1:
                    return "How familiar the scent of this flower... How delicate the flower... Where did you find it...?";
                case YI_GFWORDS_2:
                    return "--Give the flower?--";
                case YI_GFWORDS_3:
                    return "It's the first time a bug has given me flowers... Thank you for your kindness! It's beautiful, yet quite fragile; the slightest jostle or bump can cause it to wilt... Such a unique treasure should be given to a bug more deserving, don't you think?";
                case YI_FLOWER_1_NAME:
                    return "Unnamed Flower";
                case YI_FLOWER_1_DESC:
                    return "A gift from Yi to the Knight, a faint scent that calms bugs.\r\n\r\nIt's strong and will not be damaged even if the holder is injured.";
                case YI_FLOWER_2_NAME:
                    return "Wilted Flower";
                case YI_FLOWER_2_DESC:
                    return "A gift from Yi to the Knight, it has wilted for some reason.\r\n\r\nAlthough wilted, the remnants have formed a different type of flower.";
                case YI_SEND_FLOWER1:
                    return "The Seer's little cottage is so comforting, but I must be on my way, too. That chimes I heard piqued my curiosity... I have a flower for you, too...";
                case YI_SEND_FLOWER2:
                    return "--Accept the gift？--";
                case YI_SEND_FLOWER3:
                    return "This flower is for you; it will do its best to protect you as you brave the darkness!";
                case YI_SEND_FLOWER4:
                    return "Ah... Well, if you don't like it...";
                case YI_SEND_FLOWER5:
                    return "Farewell then. Who knows, our paths may cross again soon (laughs).";
                default:
                    return null;
            }
        }
    }
}
