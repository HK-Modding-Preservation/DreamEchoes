using DreamEchoesCore.Misc;
using RingLib;
using UnityEngine;

namespace DreamEchoesCore.Scenes;

internal class DreamEchoesSeer
{
    public static void Initialize(string sceneName)
    {
        if (sceneName != "DreamEchoesSeer")
        {
            return;
        }

        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initializing DreamEchoesSeer");

        GameObject prefab;
        GameObject instance;
        foreach (var (key, value) in Preload.Names)
        {
            if (key != "Dream_Backer_Shrine")
            {
                continue;
            }
            prefab = DreamEchoesCore.GetPreloaded(key, value);
            instance = Object.Instantiate(prefab);
            foreach (var spriteRender in instance.GetComponentsInChildren<SpriteRenderer>(true))
            {
                var color = spriteRender.color;
                color.r = Mathf.Min(color.r + 0.2f, 1);
                spriteRender.color = color;
            }
            if (value.StartsWith("CameraLockArea"))
            {
                var boxCollider = instance.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(60.5565f, 24.5745f);
            }
            instance.SetActive(true);
        }

        prefab = DreamEchoesCore.GetPreloaded("Mines_34", "mine_summit_statue");
        instance = Object.Instantiate(prefab);
        instance.transform.position = new Vector3(90, 31, 26);
        instance.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        instance.SetActive(true);

        prefab = DreamEchoesCore.GetPreloaded("RestingGrounds_04", "dreamer_statue/Dreamer_statue_horn_left");
        instance = Object.Instantiate(prefab);
        instance.transform.position = new Vector3(75, 30, 26);
        instance.transform.localScale = new Vector3(2, 2, 1);
        instance.SetActive(true);

        prefab = DreamEchoesCore.GetPreloaded("RestingGrounds_04", "dreamer_statue/Dreamer_statue_horn_right");
        instance = Object.Instantiate(prefab);
        instance.transform.position = new Vector3(105, 30, 26);
        instance.transform.localScale = new Vector3(2, 2, 1);
        instance.SetActive(true);

        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initialized DreamEchoesSeer");
    }
}
