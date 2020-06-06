using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.Extras;

public class StartButton : MonoBehaviour
{
    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;

    private void Awake()
    {
        leftPointer.PointerIn += PointerInside;
        leftPointer.PointerOut += PointerOutside;
        leftPointer.PointerClick += PointerClick;

        rightPointer.PointerIn += PointerInside;
        rightPointer.PointerOut += PointerOutside;
        rightPointer.PointerClick += PointerClick;
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        Debug.Log("clicked " + e.target.name);
        if (e.target.name == "Button")
        {
            Debug.Log("Button was clicked");
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button")
        {
            Debug.Log("Button was entered");
        }
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "Button")
        {
            Debug.Log("Button was exited");
        }
    }
}
