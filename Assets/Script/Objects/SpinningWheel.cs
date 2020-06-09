using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SpinningWheel : MonoBehaviour
{
    public float TERMINAL_SPEED = 5.0f;
    public float ACCELERATION = 0.1f;

    // reference
    Transform spinningArea;

    // variables
    float rotationSpeed = 0.0f;
    float rotationAccelation = 0.0f;

    public SteamVR_Action_Boolean interactionUI;

    private void Awake()
    {
        spinningArea = this.transform.Find("SpinningArea");
    }

    // Update is called once per frame
    void Update()
    {
        bool isPressingTrigger = interactionUI.GetState(SteamVR_Input_Sources.Any);
        // SteamVR의 Interact UI 버튼을 누르고 있을 시에만 작동하게 합니다. (혹은 키보드의 R)
        if (isPressingTrigger || Input.GetKey(KeyCode.R))
        {
            rotationAccelation += ACCELERATION;
            if (rotationAccelation >= TERMINAL_SPEED)
            {
                rotationAccelation = TERMINAL_SPEED;
            }
        }
        else
        {
            rotationAccelation -= ACCELERATION;
            if (rotationAccelation <= 0) rotationAccelation = 0;
        }

        //rotationSpeed += rotationAccelation;
        spinningArea.Rotate(new Vector3(0, -rotationAccelation, 0));
    }
}
