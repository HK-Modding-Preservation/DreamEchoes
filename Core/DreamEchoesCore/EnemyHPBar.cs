// Credit to https://github.com/ygsbzr/Enemy-HP-Bars

using MonoMod.ModInterop;
using System;
using UnityEngine;

namespace DreamEchoesCore;

internal static class EnemyHPBar
{
    [ModImportName(nameof(EnemyHPBar))]
    private static class EnemyHPBarImport
    {
#pragma warning disable 649
        public static Action<GameObject> MarkAsBoss;
        public static Action<GameObject> RefreshHPBar;
#pragma warning restore 649
    }

    static EnemyHPBar() => typeof(EnemyHPBarImport).ModInterop();

    public static void MarkAsBoss(this GameObject go)
    {
        EnemyHPBarImport.MarkAsBoss?.Invoke(go);
    }

    public static void RefreshHPBar(this GameObject go)
    {
        EnemyHPBarImport.RefreshHPBar?.Invoke(go);
    }
}