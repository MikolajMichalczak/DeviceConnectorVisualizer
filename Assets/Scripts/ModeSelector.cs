using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ModeSelector : MonoBehaviour
{
    public enum Mode
    {
        view,
        rotate,
        move,
        push,
        pull
    }

    public GameObject panel;
    public Mode mode = Mode.view;
    public static ModeSelector Selector { get; private set; }

    private void Awake()
    {
        if (Selector != null && Selector != this)
        {
            Destroy(this);
        }
        else
        {
            Selector = this;
        }
    }

    public void SelectView()
    {
        mode = Mode.view;
    }

    public void SelectMove()
    {
        mode = Mode.move;
    }

    public void SelectRotate()
    {
        mode = Mode.rotate;
    }

    public void SelectPull()
    {
        mode = Mode.pull;
    }

    public void SelectPush()
    {
        mode = Mode.push;
    }

}
