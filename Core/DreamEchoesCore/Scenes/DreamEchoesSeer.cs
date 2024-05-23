using DreamEchoesCore.Utils;
using RingLib;
using UnityEngine;
using WeaverCore;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Scenes;

internal class DreamEchoesSeer
{
    private class ClearSceneBorders : MonoBehaviour
    {
        private void Update()
        {
            var cleared = false;
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            foreach (var obj in scene.GetRootGameObjects())
            {
                if (obj.name.StartsWith("SceneBorder"))
                {
                    obj.SetActive(false);
                    cleared = true;
                }
            }
            if (cleared)
            {
                Log.LogInfo(typeof(DreamEchoesSeer).Name, "Cleared Scene Borders");
                Object.Destroy(this);
            }
        }
    }

    private class WetEnv : MonoBehaviour
    {
        private void Update()
        {
            var playerData = PlayerData.instance;
            playerData.environmentType = 6;
        }
    }

    private class Music : MonoBehaviour
    {
        private WeaverMusicCue musicCue;

        private void Start()
        {
            musicCue = WeaverAssets.LoadAssetFromBundle<WeaverMusicCue, DreamEchoes.DreamEchoes>("DreamEchoesSeerMusicCue");
        }

        private void Update()
        {
            if (GameManager.instance.AudioManager.CurrentMusicCue != musicCue)
            {
                GameManager.instance.AudioManager.ApplyMusicCue(musicCue, 0, 0, false);
            }
        }
    }

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
                Color.RGBToHSV(color, out float H, out float S, out float V);
                H = (H * 360 + 45) % 360 / 360;
                color = Color.HSVToRGB(H, S, V);
                spriteRender.color = color;
            }
            if (value.StartsWith("CameraLockArea"))
            {
                var boxCollider = instance.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(120.5565f, 24.5745f);
            }
            if (value.StartsWith("dream_scene pieces"))
            {
                var clouds = instance.transform.Find("wp_clouds");
                var cloud = clouds.Find("wp_clouds_0002_1 (80)").gameObject;
                Object.Destroy(cloud);
            }
            if (value == "water_fog")
            {
                var spriteRender = instance.GetComponent<SpriteRenderer>();
                spriteRender.color = new Color(0.6f, 0.41f, 1, 0.55f);
            }
            if (value.StartsWith("dream_fog"))
            {
                var spriteRender = instance.GetComponent<SpriteRenderer>();
                spriteRender.color = new Color(0.6792f, 0.4118f, 0.75f, 0.75f);
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

        instance = new GameObject("ClearSceneBorders");
        instance.AddComponent<ClearSceneBorders>();

        foreach (var (key, value) in Preload.Names)
        {
            if (key != "Ruins1_06")
            {
                continue;
            }
            prefab = DreamEchoesCore.GetPreloaded(key, value);
            instance = Object.Instantiate(prefab);
            instance.transform.Translate(24, 7, 0);
            if (value == "tile_floor")
            {
                for (var i = 0; i < instance.transform.childCount; ++i)
                {
                    var child = instance.transform.GetChild(i);
                    if (child.name != "ruind_horizontal_beam_02")
                    {
                        Object.Destroy(child.gameObject);
                    }
                }
            }
            instance.SetActive(true);
        }

        prefab = DreamEchoesCore.GetPreloaded("Ruins1_27", "_Scenery");
        instance = Object.Instantiate(prefab);
        instance.transform.position = new Vector3(30, 20.05f, 0);
        for (var i = 0; i < instance.transform.childCount; ++i)
        {
            var child = instance.transform.GetChild(i);
            if (child.name != "ruins_rain" && !child.name.Contains("ruin_water_bounce "))
            {
                Object.Destroy(child.gameObject);
            }
            if (child.name == "ruins_rain")
            {
                foreach (var particleSystem in child.GetComponentsInChildren<ParticleSystem>())
                {
                    var color = particleSystem.startColor;
                    Color.RGBToHSV(color, out float H, out float S, out float V);
                    H = (H * 360 + 70) % 360 / 360;
                    color = Color.HSVToRGB(H, S, V + 0.3f);
                    particleSystem.startColor = color;
                }
            }
            if (child.name.Contains("ruin_water_bounce "))
            {
                var index = child.name.Split(' ')[1];
                index = index.Substring(1, index.Length - 2);
                var indexInt = int.Parse(index);
                if (indexInt < 3 || indexInt > 10)
                {
                    Object.Destroy(child.gameObject);
                }

            }
        }
        instance.SetActive(true);

        prefab = DreamEchoesCore.GetPreloaded("Ruins1_27", "_Scenery");
        instance = Object.Instantiate(prefab);
        instance.transform.position = new Vector3(55.1f, 20.05f, 0);
        for (var i = 0; i < instance.transform.childCount; ++i)
        {
            var child = instance.transform.GetChild(i);
            if (!child.name.Contains("ruin_water_bounce "))
            {
                Object.Destroy(child.gameObject);
            }
            if (child.name.Contains("ruin_water_bounce "))
            {
                var index = child.name.Split(' ')[1];
                index = index.Substring(1, index.Length - 2);
                var indexInt = int.Parse(index);
                if (indexInt < 3 || indexInt > 10)
                {
                    Object.Destroy(child.gameObject);
                }

            }
        }
        instance.SetActive(true);

        instance = new GameObject("WetEnv");
        instance.AddComponent<WetEnv>();

        instance = new GameObject("Music");
        instance.AddComponent<Music>();

        Log.LogInfo(typeof(DreamEchoesSeer).Name, "Initialized DreamEchoesSeer");
    }
}
