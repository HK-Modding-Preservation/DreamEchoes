using UnityEngine;

namespace DreamEchoesCore.Entities.Enemies.Seer.SeerStateMachine;

internal class TreeStateMachine : MonoBehaviour
{
    public GameObject Attacks;

    public AudioClip GrowSound;

    public SeerStateMachine seerFSM;

    public void StartAttacks() { }

    public void PlayGrowSound() { }
}
