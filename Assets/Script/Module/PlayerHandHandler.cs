using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.Extras;

public class PlayerHandHandler : MonoBehaviour
{
    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;

    // Start is called before the first frame update
    void Start()
    {
        leftPointer.PointerIn += PointerInside;
        leftPointer.PointerOut += PointerOutside;
        leftPointer.PointerClick += PointerClick;

        rightPointer.PointerIn += PointerInside;
        rightPointer.PointerOut += PointerOutside;
        rightPointer.PointerClick += PointerClick;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        Debug.Log("pointer clicked: " + e.target.name);
        //if (e.target.name == "StartButton")
        // {
        //     SceneManager.LoadScene(sceneName: "PotteryMoldingScene");
        // }
        if (e.target.tag == "Interactor")
        {
            Debug.Log("Interactor: " + e.target.name);
            
        }
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        Debug.Log("pointer exited: " + e.target.name);
    }
}
