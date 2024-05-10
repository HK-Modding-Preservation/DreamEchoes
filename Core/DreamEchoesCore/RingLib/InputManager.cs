using HKMirror.Reflection;
using UnityEngine;

namespace RingLib;

internal class InputManager : MonoBehaviour
{
    public bool LeftPressed;
    public bool RightPressed;
    public float Direction => LeftPressed ? -1 : RightPressed ? 1 : 0;
    public bool AttackPressed;
    private HeroActions heroActions;
    private bool leftPressed;
    private bool rightPressed;
    private bool attackPressed;
    public InputManager()
    {
        heroActions = HeroController.instance.Reflect().inputHandler.inputActions;
    }
    void Update()
    {
        if (leftPressed != heroActions.left.IsPressed)
        {
            if (!leftPressed)
            {
                LeftPressed = true;
                RightPressed = false;
            }
            else
            {
                LeftPressed = false;
                RightPressed = rightPressed;
            }
            leftPressed = heroActions.left.IsPressed;
        }
        if (rightPressed != heroActions.right.IsPressed)
        {
            if (!rightPressed)
            {
                RightPressed = true;
                LeftPressed = false;
            }
            else
            {
                RightPressed = false;
                LeftPressed = leftPressed;
            }
            rightPressed = heroActions.right.IsPressed;
        }
        if (attackPressed != heroActions.attack.IsPressed)
        {
            AttackPressed = attackPressed;
            attackPressed = heroActions.attack.IsPressed;
        }
    }
}
