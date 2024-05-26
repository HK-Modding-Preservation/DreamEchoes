using DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;
using Modding.Utils;
using RingLib.Attacks;
using RingLib.Components;
using UnityEngine;
using WeaverCore.Utilities;

namespace DreamEchoesCore.Entities.NPCs
{
    internal class ControlSeer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var self = HeroController.instance;
            var control = self.gameObject.GetOrAddComponent<Control>();
            if (!control.HasControlled)
            {
                if (!self.controlReqlinquished)
                {
                    var seerPrefab = WeaverAssets.LoadAssetFromBundle<GameObject, DreamEchoes.DreamEchoes>("Seer");
                    var seer = GameObject.Instantiate(seerPrefab, self.transform.position, Quaternion.identity);
                    seer.GetComponent<SeerStateMachine>().StartState = nameof(SeerStateMachine.ControlledIdle);
                    GameObject.Destroy(seer.GetComponent<WeaverCore.Components.PlayerDamager>());
                    foreach (var attack in seer.GetComponentsInChildren<Attack>(true))
                    {
                        attack.Hero = true;
                    }
                    control.InstallControlled(seer);
                }
                else
                {
                    RingLib.Log.LogError(GetType().Name, "Cannot install controlled while control is relinquished");
                }
            }
            else
            {
                RingLib.Log.LogError(GetType().Name, "Controlled already installed");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var self = HeroController.instance;
            var control = self.gameObject.GetOrAddComponent<Control>();
            if (control.HasControlled)
            {
                control.UninstallControlled();
            }
            else
            {
                RingLib.Log.LogError(GetType().Name, "Controlled not installed");
            }
        }
    }
}
